using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerOne.Concurrent;
using ServerOne;

namespace GameServer.Cache
{
    /// <summary>
    /// 匹配房间缓存层
    /// </summary>
    public class MatchCache
    {
        /// <summary>
        /// 用户和房间映射
        /// </summary>
        private Dictionary<string, int> userRoomDict = new Dictionary<string, int>();
        /// <summary>
        /// 房间和房间模型映射
        /// </summary>
        private Dictionary<int, MatchRoom> roomRoomModelDict = new Dictionary<int, MatchRoom>();
        /// <summary>
        /// 重用房间
        /// </summary>
        Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();

        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 判断玩家是否在房间内
        /// </summary>
        /// <returns></returns>
        public bool IsMatchRoom(string acc)
        {
            return userRoomDict.ContainsKey(acc);
        }

        /// <summary>
        /// 有房间就进入
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public MatchRoom Enter(string acc,ClientPeer client)
        {
            foreach(MatchRoom mr in roomRoomModelDict.Values)
            {
                if (mr.IsFull()) //满了就跳过
                {
                    continue;
                }
                mr.EnterRoom(acc,client); //让玩家进入房间
                userRoomDict.Add(acc, mr.id);//存到字典
                return mr;
            }
            //如果没房间就创建一个
            MatchRoom room = null;
            if (roomQueue.Count > 0) //如果有重用就取出来
            {
                room = roomQueue.Dequeue();
            }
            else
            {
                room = new MatchRoom(id.Add_Get());
            }
            room.EnterRoom(acc,client);
            roomRoomModelDict.Add(room.id, room);
            userRoomDict.Add(acc, room.id);
            return room;
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public MatchRoom Exit(string acc)
        {
            Tool.PrintMessage("执行MatchCache.Exit()");
            int id = userRoomDict[acc];
            MatchRoom room = roomRoomModelDict[id];
            room.ExitRoom(acc);
            if (room.ReadyUserList.Contains(acc))
            {
                room.NotReady(acc);
            }
                userRoomDict.Remove(acc);
            
            if (room.IsEmpty())
            {
                roomRoomModelDict.Remove(id);
                roomQueue.Enqueue(room);
            }
            return room;
        }
        /// <summary>
        /// 获取玩家所在的房间
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public MatchRoom GetRoom(string acc)
        {
            int id = userRoomDict[acc];
            MatchRoom room = roomRoomModelDict[id];
            return room;
        }
        /// <summary>
        /// 清理房间
        /// </summary>
        public void ClearRoom(MatchRoom room)
        {
            roomRoomModelDict.Remove(room.id);
            foreach(var u in room.UserClientDict.Keys)
            {
                userRoomDict.Remove(u);
            }
            room.UserClientDict.Clear();
            room.ReadyUserList.Clear();
            //重用房间？存疑
        }
    }
}
