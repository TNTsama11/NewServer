using ServerOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Cache
{
    /// <summary>
    /// 账号缓存层
    /// </summary>
    public class AccCache
    {
        /// <summary>
        /// 保存GUID账号
        /// </summary>
        private Queue<string> accQueue = new Queue<string>();
        /// <summary>
        /// 存储账号和客户端连接对象的对应字典
        /// </summary>
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>(); //为了方便访问所以双向映射
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();
        /// <summary>
        /// 判断账号是否在线
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }
        /// <summary>
        /// 判断客户端是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }
        /// <summary>
        /// 玩家上线方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        public void Onlie(ClientPeer client,string account)
        {
            accClientDict.Add(account, client);
            clientAccDict.Add(client, account);
            Tool.PrintMessage("客户端" + client.clientSocket.RemoteEndPoint.ToString() + "的账号已上线");
        }
        /// <summary>
        /// 玩家下线方法
        /// (根据连接对象)
        /// </summary>
        /// <param name="clietn"></param>
        public void OffLine(ClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
            accQueue.Enqueue(account);//重用账号
            Tool.PrintMessage("执行OffLine()");
        }
        /// <summary>
        /// 玩家下线方法
        /// (根据账号)
        /// </summary>
        /// <param name="account"></param>
        public void OffLine(string account)
        {
            ClientPeer client = accClientDict[account];
            accClientDict.Remove(account);
            clientAccDict.Remove(client);
            accQueue.Enqueue(account);
        }

        public void Login(ClientPeer client)
        {
          string tempAccount=  DistributionAccount(client);
            Onlie(client, tempAccount);
        }

        /// <summary>
        /// 为连接进来的玩家分配账号
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string DistributionAccount(ClientPeer client)
        {
            //TODO 
            //将账号分配给玩家存到玩家数据缓存
            if (accQueue.Count==0)
            {
                accQueue.Enqueue(Guid.NewGuid().ToString("N"));
            }
            return accQueue.Dequeue();
        }
        public string GetAcc(ClientPeer client)
        {
            return clientAccDict[client];
        }
    }
}
