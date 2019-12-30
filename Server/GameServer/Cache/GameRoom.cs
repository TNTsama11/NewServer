using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommunicationProtocol.Dto;
using ServerOne;
using ServerOne.Concurrent;
using ServerOne.mTimer;

namespace GameServer.Cache
{
    /// <summary>
    /// 游戏房间类
    /// </summary>
    public class GameRoom
    {
        /// <summary>
        /// 房间内玩家Account与连接对象的字典
        /// </summary>
        public Dictionary<string, ClientPeer> UserAccClientDict { get; set; }
        /// <summary>
        /// 房间内玩家Account与Transform信息的字典
        /// </summary>
        public Dictionary<string,TransformInfo> UserTransDict { get; set; }
        /// <summary>
        /// 房间内玩家Account与hp的字典
        /// </summary>
        public Dictionary<string,int> UserHpDict { get; set; }
        /// <summary>
        /// 房间内玩家Account与hg的字典
        /// </summary>
        public Dictionary<string,int> UserHgDict { get; set; }
        /// <summary>
        /// 房间内玩家Account与击杀数的字典
        /// </summary>
        public Dictionary<string,int> UserKillDict { get; set; }
        /// <summary>
        /// 房间内生成的道具id与数据的字典
        /// </summary>
        public Dictionary<int, PropsDto> IdPropsDict { get; set; }
        /// <summary>
        /// 房间内玩家Account与子弹类型字典
        /// </summary>
        public Dictionary<string, int> UserArmsDict { get; set; }

        public int id { get; set; }

        public GameRoom(int id)
        {
            this.id = id;
            this.UserAccClientDict = new Dictionary<string, ClientPeer>();
            this.UserTransDict = new Dictionary<string, TransformInfo>();
            this.UserHpDict = new Dictionary<string, int>();
            this.UserHgDict = new Dictionary<string, int>();
            this.UserKillDict = new Dictionary<string, int>();
            this.IdPropsDict = new Dictionary<int, PropsDto>();
            this.UserArmsDict = new Dictionary<string, int>();
        }

        /// <summary>
        /// 游戏房间是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return UserAccClientDict.Count == 8;
        }
        /// <summary>
        /// 游戏房间是否空了
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UserAccClientDict.Count == 0;
        }
        /// <summary>
        /// 进入游戏房间
        /// </summary>
        /// <param name="acc">Account</param>
        /// <param name="client">连接对象</param>
        public void EnterRoom(string acc,ClientPeer client)
        {
            UserAccClientDict.Add(acc, client);
            TransformInfo trans = new TransformInfo();
            UserTransDict.Add(acc, trans);
            UserHpDict.Add(acc,100);
            UserHgDict.Add(acc, 200);
            UserKillDict.Add(acc, 0);
            UserArmsDict.Add(acc, 0);
        }
        /// <summary>
        /// 退出游戏房间
        /// </summary>
        public void ExitRoom(string acc)
        {
            UserAccClientDict.Remove(acc);
            UserTransDict.Remove(acc);
            UserHpDict.Remove(acc);
            UserHgDict.Remove(acc);
            UserKillDict.Remove(acc);
            UserArmsDict.Remove(acc);
        }
        /// <summary>
        /// 获取玩家hp
        /// </summary>
        public int GetHpByAcc(string acc)
        {
            return UserHpDict[acc];
        }
        /// <summary>
        /// 设置玩家hp
        /// </summary>
        public void SetHp(string acc,int hp)
        {
            UserHpDict[acc] = hp;
        }
        /// <summary>
        /// 获取玩家hg
        /// </summary>
        public int GetHgByAcc(string acc)
        {
            return UserHgDict[acc];
        }
        /// <summary>
        /// 设置玩家hg
        /// </summary>
        public void SetHg(string acc,int hg)
        {
            UserHgDict[acc] = hg;
        }
        /// <summary>
        /// 获取玩家kill
        /// </summary>
        public int GetKillByAcc(string acc)
        {
            return UserKillDict[acc];
        }
        /// <summary>
        /// 设置玩家kill
        /// </summary>
        public void SetKill(string acc,int kill)
        {
            UserKillDict[acc] = kill;
        }
        /// <summary>
        /// 重置hp
        /// 有默认值
        /// </summary>
        public void RefreshHp(string acc,int hp=100)
        {
            UserHpDict[acc] = hp;
        }
        /// <summary>
        /// 重置hg
        /// 有默认值
        /// </summary>
        public void RefreshHg(string acc,int hg = 200)
        {
            UserHgDict[acc] = hg;
        }
        /// <summary>
        /// 刷新kill
        /// 无默认值
        /// </summary>
        public void RefreshKill(string acc,int kill)
        {
            UserKillDict[acc] = kill;
        }

        /// <summary>
        /// 刷新玩家位置信息
        /// </summary>
        public void RefreshTrans(string acc,float[]pos,float[] rota)
        {
            UserTransDict[acc].Change(pos, rota);
        }
        public TransformInfo GetTransByAcc(string acc)
        {
            return UserTransDict[acc];
        }
        public void RefreshTrans(string acc, int[] pos)
        {
            UserTransDict[acc].pos[0] = pos[0];
            UserTransDict[acc].pos[2] = pos[1];
        }
        /// <summary>
        /// 生成一个不和其他玩家冲突的位置（x和z）
        /// </summary>
        public int[] GetRandomPosition()
        {
            Random ra =new Random(Guid.NewGuid().GetHashCode());
            int x = ra.Next(-20, 20);
            int z= ra.Next(-20, 20);
            foreach(var item in UserTransDict.Values)
            {
                if(Math.Abs(x-item.pos[0])>5|| Math.Abs(z - item.pos[2]) > 5)
                {
                    continue;
                }
                else
                {
                    return GetRandomPosition();
                }
            }
            int[] xz = new int[2];
            xz[0] = x;
            xz[1] = z;
            return xz;
        } 
        /// <summary>
        /// 从字典中移除某个道具
        /// </summary>
        public void RemoveProps(int id)
        {
            if (IdPropsDict.ContainsKey(id))
            {
                IdPropsDict.Remove(id);
            }
        }
        /// <summary>
        /// 设置子弹类型
        /// </summary>
        public void SetArms(string acc,int type)
        {
            UserArmsDict[acc] = type;
        }

        public int GetArmsByAcc(string acc)
        {
            if (UserArmsDict.ContainsKey(acc))
            {
                return UserArmsDict[acc];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="value">参数</param>
        /// <param name="client">排除在外的连接对象（一般是当前连接对象）</param>
        public void Broadcast(int opCode, int subCode, object value, ClientPeer client = null)
        {
            foreach(var item in UserAccClientDict.Values)
            {
                if (item == client)
                {
                    continue;
                }
                Tool.PrintMessage("向" + item.clientSocket.RemoteEndPoint.ToString() + "广播信息");
                item.SendMessage(opCode, subCode, value);
            }
        }


        /// <summary>
        /// 随机位置生成随机个数随机类型道具
        /// </summary>
        public CreatPropsDto CreatProps()
        {
            IdPropsDict.Clear(); //先清除以前的
            ConcurrentInt propsId = new ConcurrentInt(-1);
            CreatPropsDto creatPropsDto = new CreatPropsDto();
            Random random = new Random();
            int count = random.Next(5, 20); //随机个数
            List<int> usedPosX = new List<int>(count); //使用过的X坐标
            List<int> usedPosZ = new List<int>(count); //使用过的Z坐标
            Random randomArm = new Random(Guid.NewGuid().GetHashCode());
            for (int i=0; i<count;i++)
            {
                PropsDto propsDto = new PropsDto();
                propsDto.id = propsId.Add_Get();
                propsDto.type = random.Next(0, 3); //随机类型（有几种类型上限就是下限加几 
                if (propsDto.type == 2) //随机武器类型
                {                   
                    propsDto.subType = randomArm.Next(0, 4);
                }
                int posx = RandomPos(usedPosX,46,-45);
                propsDto.posX = posx;
                usedPosX.Add(posx);
                int posz = RandomPos(usedPosZ,46,-45);
                propsDto.posZ = posz;
                usedPosZ.Add(posz);
                creatPropsDto.idPropsTypeDict.Add(propsDto.id, propsDto);
                IdPropsDict.Add(propsDto.id, propsDto);
            }
            return creatPropsDto;
        }
        /// <summary>
        /// 生成没使用过的随机数
        /// 上限取不到
        /// </summary>
        /// <param name="used">使用过的随机数列表</param>
        /// <param name="max">最大值</param>
        /// <param name="mini">最小值</param>
        /// <returns></returns>
        private int RandomPos(List<int> used,int max,int mini)
        {
            Random randomX = new Random(Guid.NewGuid().GetHashCode());
            int x = randomX.Next(mini, max); //上限和下限是场地大小范围
            foreach(var item in used)
            {               
                if (Math.Abs(x-item)>2)
                {
                    continue;
                }
                else
                {
                    return RandomPos(used,max,mini);
                }
            }
            return x;
        }
    }
}
