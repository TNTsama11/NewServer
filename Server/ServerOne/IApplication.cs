using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOne
{
    public interface IApplication
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void OnDisconnect(ClientPeer client);
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="smg"></param>
        void OnReceive(ClientPeer client, SocketMessage smg);
        ///// <summary>
        ///// 连接
        ///// </summary>
        ///// <param name="client"></param>
        //void OnConnect(ClientPeer client);
    }
}
