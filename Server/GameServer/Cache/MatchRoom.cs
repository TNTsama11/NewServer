using ServerOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Cache
{
    public class MatchRoom
    {
        public int id { get; private set; }
        /// <summary>
        /// 房间内玩家列表
        /// </summary>
        public Dictionary<string,ClientPeer> UserClientDict { get; private set; }
        /// <summary>
        /// 已准备的玩家列表
        /// </summary>
        public List<string> ReadyUserList { get; private set; }

        public MatchRoom(int id)
        {
            this.id = id;
            this.UserClientDict = new Dictionary<string,ClientPeer>();
            this.ReadyUserList = new List<string>();
        }
        /// <summary>
        /// 房间是否满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return UserClientDict.Count == 8;
        }
        /// <summary>
        /// 房间是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UserClientDict.Count == 0;
        }
        /// <summary>
        /// 房间内玩家是否都准备
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            return ReadyUserList.Count >= 2&& ReadyUserList.Count==UserClientDict.Count;
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="acc"></param>
        public void EnterRoom(string acc,ClientPeer client)
        {
            UserClientDict.Add(acc,client);
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="acc"></param>
        public void ExitRoom(string acc)
        {
            UserClientDict.Remove(acc);            
        }
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="acc"></param>
        public void Ready(string acc)
        {
            ReadyUserList.Add(acc);
        }
        /// <summary>
        /// 取消准备
        /// </summary>
        /// <param name="acc"></param>
        public void NotReady(string acc)
        {
            ReadyUserList.Remove(acc);
        }
        /// <summary>
        /// 广播房间内玩家信息
        /// </summary>
        public void BroadcastUserInfo(int opCode,int subCode,object value,ClientPeer client=null)
        {
            Tool.PrintMessage("执行BroadcastUserInfo()");
            foreach(var c in UserClientDict.Values)
            {
                if (c == client)
                {
                    continue;
                }
                Tool.PrintMessage("向" + c.clientSocket.RemoteEndPoint.ToString() + "广播信息");
                c.SendMessage(opCode, subCode, value);
            }
        }
    }
}
