using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Code
{
    public class AccCode
    {
        /// <summary>
        /// 客户端登陆请求
        /// </summary>
        public const int ACC_LOGIN_CREQ = 0;
        /// <summary>
        /// 服务器登陆响应
        /// </summary>
        public const int ACC_LOGIN_SREP = 1;

        public const int ACC_LOGIN_NOACC = 2;

        public const int ACC_CREATE_CREQ = 3;
        public const int ACC_CREATE_SREP = 4;
        public const int ACC_RELOAD_CREQ = 5;
        public const int ACC_RELOAD_SREP = 6;
    }
}
