using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerOne;
using CommunicationProtocol.Code;
using GameServer.Cache;
using GameServer.Model;
using CommunicationProtocol.Dto;

namespace GameServer.Logic
{
    /// <summary>
    /// 玩家数据模块逻辑层
    /// </summary>
    class UserHandler : IHandler
    {
        private UserCache userCache = Caches.user;
        private AccCache accCache = Caches.acc;

        public void OnDisconnect(ClientPeer client)
        {
            if (userCache.IsOnline(client))
            {
                userCache.Offline(client);
            }
            
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.USER_ONLINE_CREQ:
                    Online(client);
                    break;
                case UserCode.USER_GETUSERINFO_CREQ:
                    GetUserInfo(client);
                    break;
                case UserCode.USER_UPLOADINFO_CREQ:
                    ReceiveUserInfo(client, value as UserDto);
                    break;
                case UserCode.USER_OFFLINE_CREQ:
                    OffLine(client);
                    break;
                default:
                    break;
            }
        }

        private void Online(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(()=> {
                if (!accCache.IsOnline(client))
                {
                    //客户端Account不在线
                    return;
                }
                string account = accCache.GetAcc(client);
                if (!userCache.IsExist(account))
                {
                    client.SendMessage(OpCode.USER, UserCode.USER_ONLIEN_SREP, null);//不存在该Account对应的玩家数据模型
                    return;
                }
                Tool.PrintMessage("客户端" + client.clientSocket.RemoteEndPoint.ToString() + "的角色已上线");
                
                userCache.Online(client, account);

                client.SendMessage(OpCode.USER, UserCode.USER_ONLIEN_SREP, 0);//上线成功
            });
        }

        private void OffLine(ClientPeer client)
        {
            if (!accCache.IsOnline(client))
            {
                //客户端Account不在线
                return;
            }
            string account = accCache.GetAcc(client);
            if (!userCache.IsExist(account))
            {               
                return;
            }
            userCache.Offline(client);
        }

        private void GetUserInfo(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(()=> {
                if (!accCache.IsOnline(client))
                {
                    //客户端Account不在线
                    return;
                }
                string account = accCache.GetAcc(client);
                UserModel userModel = userCache.GetModelByAcc(account);
                UserDto userDto = new UserDto(userModel.Account, userModel.Name, userModel.IconID, userModel.ModelID, userModel.Lv);
                client.SendMessage(OpCode.USER, UserCode.USER_GETUSERINFO_SREP, userDto);
                //将玩家数据发送给客户端
            });
        }
        /// <summary>
        /// 接受客户端发来的玩家信息并存入缓存层
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userDto"></param>
        private void ReceiveUserInfo(ClientPeer client,UserDto userDto)
        {
            if (userCache.IsExist(userDto.Account))
            {
                userCache.Cover(client, userDto.Account, userDto.Name, userDto.Lv, userDto.IconID, userDto.ModelID);
            }
            else
            {
                userCache.ADD(client, userDto.Account, userDto.Name, userDto.Lv, userDto.IconID, userDto.ModelID);
            }

            client.SendMessage(OpCode.USER, UserCode.USER_UPLOADINFO_SREP, 0);
        }
    }
}
