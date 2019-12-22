using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Code
{
    public class UserCode
    {
        /// <summary>
        /// 客户端上线请求
        /// </summary>
        public const int USER_ONLINE_CREQ = 0;
        /// <summary>
        /// 服务器对上线响应
        /// </summary>
        public const int USER_ONLIEN_SREP = 1;
        /// <summary>
        /// 客户端获取玩家信息请求
        /// </summary>
        public const int USER_GETUSERINFO_CREQ = 2;
        /// <summary>
        /// 服务器对获取玩家信息响应
        /// </summary>
        public const int USER_GETUSERINFO_SREP = 3;
        /// <summary>
        /// 客户端向服务器上传玩家信息的请求
        /// </summary>
        public const int USER_UPLOADINFO_CREQ = 4;
        /// <summary>
        /// 服务器对上传完成的响应
        /// </summary>
        public const int USER_UPLOADINFO_SREP = 5;

        public const int USER_OFFLINE_CREQ = 6;

        public const int USER_OFFLIEN_SREP = 7;
    }
}
