using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerOne;
using CommunicationProtocol.Code;
using GameServer.Cache;
using CommunicationProtocol.Dto;
using GameServer.Model;

namespace GameServer.Logic
{
    /// <summary>
    /// 匹配房间的逻辑层
    /// </summary>
    public class MatchHandler : IHandler
    {

        private MatchCache matchCache = Caches.match;
        private UserCache userCache = Caches.user;
        private GameCache gameCache = Caches.game;

        public void OnDisconnect(ClientPeer client)
        {
            Tool.PrintMessage("执行MatchHandler.OnDisconnect()");
            if (!userCache.IsOnline(client))
            {
                Tool.PrintMessage("执行MatchHandler.OnDisconnect() 判断为没有在线");
                return;
            }
            string acc = userCache.GetAccByClient(client);
            if (matchCache.IsMatchRoom(acc))
            {
                
                Exit(client);
            }

        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.MATCH_ENTER_CREQ:
                    Enter(client);
                    break;
                case MatchCode.MATCH_EXIT_CREQ:
                    Exit(client);
                    break;
                case MatchCode.MATCH_READY_CREQ:
                    Ready(client);
                    break;
                case MatchCode.MATCH_NOTREADY_CREQ:
                    NotReady(client);
                    break;
                default:
                    break;
            }
        }

        private void Enter(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(delegate {
                if (!userCache.IsOnline(client))
                {
                    return;
                }
                string acc = userCache.GetAccByClient(client);
                if (matchCache.IsMatchRoom(acc)) 
                {
                    //重复进入
                    return;
                }
                Tool.PrintMessage("客户端" + client.clientSocket.RemoteEndPoint.ToString() + "进入匹配");
                MatchRoom room = matchCache.Enter(acc,client);

                //广播给其他玩家当前玩家已进入                
                //并广播玩家数据传输对象
                UserModel tempModel = userCache.GetModelByAcc(acc);
                UserDto tempDto = new UserDto(tempModel.Account, tempModel.Name, tempModel.IconID, tempModel.ModelID, tempModel.Lv);
                room.BroadcastUserInfo(OpCode.MATCH, MatchCode.MATCH_ENTER_BROA, tempDto,client);
                MatchDto dto = new MatchDto();
                foreach(var a in room.UserClientDict.Keys)
                {
                    if (!userCache.IsOnline(a))
                    {
                        continue;
                    }
                    UserModel userModel = userCache.GetModelByAcc(a);
                    UserDto userDto = new UserDto(userModel.Account, userModel.Name, userModel.IconID, userModel.ModelID, userModel.Lv);
                    dto.accUserDict.Add(a, userDto);
                }
                dto.ReadyUserList = room.ReadyUserList;
                //将当前房间的数据发给当前玩家
                client.SendMessage(OpCode.MATCH, MatchCode.MATCH_ENTER_SREP, dto);

            });
        }
        
        public void Exit(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(delegate {
                if (!userCache.IsOnline(client))
                {
                    return;
                }                
                string acc = userCache.GetAccByClient(client);
                if (!matchCache.IsMatchRoom(acc))
                {
                    //重复退出
                    return;
                }
                Tool.PrintMessage("客户端" + client.clientSocket.RemoteEndPoint.ToString() + "离开匹配");
                   MatchRoom room= matchCache.Exit(acc);
                //将玩家退出的消息广播给所有人
                room.BroadcastUserInfo(OpCode.MATCH, MatchCode.MATCH_EXIT_BROA, acc);

            });
        }
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="client"></param>
        private void Ready(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(delegate
            {
                if (!userCache.IsOnline(client))
                {
                    return;
                }
                string acc = userCache.GetAccByClient(client);
                if (!matchCache.IsMatchRoom(acc))
                {
                    return;
                }
                Tool.PrintMessage("玩家：" + client.clientSocket.RemoteEndPoint.ToString() + "准备");
                MatchRoom room = matchCache.GetRoom(acc);
                room.Ready(acc);
                //向房间内广播这个玩家准备了
                room.BroadcastUserInfo(OpCode.MATCH, MatchCode.MATCH_READY_BROA, acc, client);
                if(room.IsAllReady())//准备人数是否达标
                {
                    //TODO
                    Tool.PrintMessage("准备人数达到游戏开始条件");
                    //创建一个游戏房间
                    GameRoom gameRoom = gameCache.CreatGameRoom();
                    //将数据托付给GameRoom
                    foreach(var item in room.UserClientDict)
                    {
                        gameCache.AddPlayerToRoom(item.Key,item.Value,gameRoom);
                    }
                    GameRoomDto gameRoomDto = new GameRoomDto();
                    foreach (var item in gameRoom.UserAccClientDict.Keys)
                    {
                        if (!userCache.IsOnline(item))
                        {
                            continue;
                        }
                        UserModel userModel = userCache.GetModelByAcc(item);
                        UserDto userDto = new UserDto(userModel.Account, userModel.Name, userModel.IconID, userModel.ModelID, userModel.Lv);
                        gameRoomDto.UserAccDtoDict.Add(item, userDto);
                        TransformInfo transformInfo = gameRoom.GetTransByAcc(item);
                        TransformDto transformDto = new TransformDto(item,transformInfo.pos,transformInfo.rota);
                        gameRoomDto.UserTransDto.Add(item, transformDto);
                        int hp = gameRoom.GetHpByAcc(item);
                        HpDto hpDto = new HpDto(item, hp);
                        gameRoomDto.UserHpDict.Add(item, hpDto);
                        int hg = gameRoom.GetHgByAcc(item);
                        HgDto hgDto = new HgDto(item, hg);
                        gameRoomDto.UserHgDict.Add(item, hgDto);
                        int kill = gameRoom.GetKillByAcc(item);
                        KillDto killDto = new KillDto(item, kill);
                        gameRoomDto.UserKillDict.Add(item, killDto);
                    }
                    //广播开始游戏
                   // room.BroadcastUserInfo(OpCode.MATCH, MatchCode.MATCH_START_BROA, null);
                    //将游戏房间数据广播给房间内的玩家
                    room.BroadcastUserInfo(OpCode.GAME, GameCode.GAME_START_BROA, gameRoomDto);                     
                    Tool.PrintMessage("房间：" + gameRoom.id + "开始游戏");
                    //清理匹配房间
                    matchCache.ClearRoom(room);
                }
            });
        }
        /// <summary>
        /// 取消准备
        /// </summary>
        /// <param name="client"></param>
        private void NotReady(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(delegate() 
            {
                if (!userCache.IsOnline(client))
                {
                    return;
                }
                string acc = userCache.GetAccByClient(client);
                if (!matchCache.IsMatchRoom(acc))
                {
                    return;
                }
                Tool.PrintMessage("玩家：" + client.clientSocket.RemoteEndPoint.ToString() + "取消准备");
                MatchRoom room = matchCache.GetRoom(acc);
                room.NotReady(acc);
                room.BroadcastUserInfo(OpCode.MATCH, MatchCode.MATCH_NOTREADY_BROA, acc, client);
            });
        }


        


    }
}
