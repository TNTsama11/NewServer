using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerOne;
using GameServer.Logic;
using CommunicationProtocol.Code;

namespace GameServer
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {

        private NetMsgCenter instance;

        public NetMsgCenter Instance
        {
            get {
                 if (instance == null)
                {
                    return instance = new NetMsgCenter();
                }
                return instance;
                 }
        }

        public NetMsgCenter()
        {
            instance = this;
        }

        IHandler accountHandler = new AccHandler();
        IHandler userHandler = new UserHandler();
        IHandler matchHandler = new MatchHandler();
        IHandler gameHandler = new GameHandler();

        public void OnDisconnect(ClientPeer client)
        {
            //顺序要注意依赖关系，越底层的越靠后执行
            matchHandler.OnDisconnect(client);
            gameHandler.OnDisconnect(client);
            userHandler.OnDisconnect(client);
            accountHandler.OnDisconnect(client);                        
        }

        public void OnReceive(ClientPeer client, SocketMessage smg)
        {
            switch (smg.opCode) //通过操作码判断是给什么模块的数据
            {
                case OpCode.ACCOUNT:
                    accountHandler.OnReceive(client, smg.subCode, smg.value);
                    break;
                case OpCode.USER:
                    userHandler.OnReceive(client, smg.subCode, smg.value);
                    break;
                case OpCode.MATCH:
                    matchHandler.OnReceive(client, smg.subCode, smg.value);
                    break;
                case OpCode.GAME:
                    gameHandler.OnReceive(client, smg.subCode, smg.value);
                    break;
                default: break;
            }
        }
    }
}
