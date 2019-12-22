using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Model;
using ServerOne;

namespace GameServer.Cache
{
    /// <summary>
    /// 玩家数据缓存层
    /// </summary>
    public class UserCache
    {
        /// <summary>
        /// 玩家Account与玩家数据模型映射的字典
        /// </summary>
        private Dictionary<string, UserModel> accountModelDict = new Dictionary<string, UserModel>();
        /// <summary>
        /// Account与客户端连接对象的字典
        /// </summary>
        private Dictionary<string, ClientPeer> accountClientDict = new Dictionary<string, ClientPeer>();
        /// <summary>
        /// 客户端连接对象与Account的字典
        /// </summary>
        private Dictionary<ClientPeer, string> clientAccountDict = new Dictionary<ClientPeer, string>();

        /// <summary>
        /// 将Account和对应的玩家信息保存到字典
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="lv"></param>
        /// <param name="iconID"></param>
        /// <param name="modelID"></param>
        public void ADD(ClientPeer client,string acc,string name,int lv,int iconID,int modelID)
        {
            UserModel userModel = new UserModel(acc, name, lv, iconID, modelID);
            accountModelDict.Add(userModel.Account, userModel);
        }
        /// <summary>
        /// 如果字典有这个Account就覆盖
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="lv"></param>
        /// <param name="iconID"></param>
        /// <param name="modelID"></param>
        public void Cover(ClientPeer client, string acc, string name, int lv, int iconID, int modelID)
        {
            accountModelDict[acc].Name = name;
            accountModelDict[acc].Lv = lv;
            accountModelDict[acc].IconID = iconID;
            accountModelDict[acc].ModelID = modelID;
        }
        /// <summary>
        /// 判断字典里是否存在该Account
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool IsExist(string acc)
        {
            return accountModelDict.ContainsKey(acc);
        }
        /// <summary>
        /// 通过Account获取玩家模型
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public UserModel GetModelByAcc(string acc)
        {
            UserModel model = accountModelDict[acc];
            return model;
        }
        /// <summary>
        /// 判断是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientAccountDict.ContainsKey(client);
        }
        /// <summary>
        /// 判断是否在线
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool IsOnline(string acc)
        {
            return accountClientDict.ContainsKey(acc);
        }
        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="acc"></param>
        public void Online(ClientPeer client,string acc)
        {
            clientAccountDict.Add(client, acc);
            accountClientDict.Add(acc, client);
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            string acc = clientAccountDict[client];
            clientAccountDict.Remove(client);
            accountClientDict.Remove(acc);
        }
        /// <summary>
        /// 通过客户端连接对象获取玩家数据模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelByClient(ClientPeer client)
        {
            string acc = clientAccountDict[client];
            return accountModelDict[acc];
        }
        /// <summary>
        /// 通过Account获取客户端连接对象
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public ClientPeer GetClientByAcc(string acc)
        {
            if (accountClientDict.ContainsKey(acc))
            {
                return accountClientDict[acc];
            }
            else
            {
                throw new Exception("没有此Account的连接对象");
            }

        }
        /// <summary>
        /// 通过连接对象获取Account
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string GetAccByClient(ClientPeer client)
        {
            if (clientAccountDict.ContainsKey(client))
            {
                return clientAccountDict[client];
            }
            else
            {
                throw new Exception("没有此连接对象的Account");
            }
        }
    }
}
