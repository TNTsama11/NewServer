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
    /// 账号逻辑层
    /// </summary>
    class AccHandler : IHandler
    {
        AccCache accCache = Caches.acc;
        UserCache userCache = Caches.user;

        public void OnDisconnect(ClientPeer client)
        {
            if (accCache.IsOnline(client))
            {
                accCache.OffLine(client);
            }
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccCode.ACC_LOGIN_CREQ:
                    Tool.PrintMessage("收到客户端" + client.clientSocket.RemoteEndPoint.ToString() + "发来的登陆请求");
                    Login(client,value.ToString()); //TODO 在数据库中查询账号信息
                    break;
                case AccCode.ACC_CREATE_CREQ:
                    Create(client);
                    break;
                case AccCode.ACC_RELOAD_CREQ:
                    Reload(client, value.ToString());
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 玩家登陆
        /// </summary>
        /// <param name="client"></param>
        private void Login(ClientPeer client,string acc)
        {
            SingleExcute.Instance.Exeute(()=> {
               // if (accCache.IsOnline(acc)) //如果之前在线就顶下去
              //  {
                //    accCache.OffLine(acc);
              //  }
                accCache.Login(client, acc);
            });
        }

        private void Reload(ClientPeer client,string acc)
        {
            SingleExcute.Instance.Exeute(() =>
            {
                accCache.Reload(client, acc);
            });
        }

        private void Create(ClientPeer client) //创建一个默认值的新账号 并发送个客户端
        {
            SingleExcute.Instance.Exeute(()=> {
                accCache.Create(client);
                userCache.ADD(client,accCache.GetAcc(client));
            });
        }
    }
}
