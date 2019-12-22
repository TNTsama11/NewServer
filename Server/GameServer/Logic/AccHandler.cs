using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerOne;
using CommunicationProtocol.Code;
using GameServer.Cache;

namespace GameServer.Logic
{
    /// <summary>
    /// 账号逻辑层
    /// </summary>
    class AccHandler : IHandler
    {
        AccCache accCache = Caches.acc;

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
                    Login(client);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 玩家登陆
        /// </summary>
        /// <param name="client"></param>
        private void Login(ClientPeer client)
        {
            SingleExcute.Instance.Exeute(()=> {
                accCache.Login(client);
                client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_LOGIN_SREP, accCache.GetAcc(client));
            });
        }
    }
}
