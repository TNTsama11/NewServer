using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOne
{
    /// <summary>
    /// 网络消息
    /// </summary>
    public class SocketMessage
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public int opCode { get; set; } //告诉计算机做什么
        /// <summary>
        /// 子操作
        /// </summary>
        public int subCode { get; set; } //告诉计算机怎么做
        /// <summary>
        /// 参数
        /// </summary>
        public object value { get; set; } //给计算机操作的对象

        public SocketMessage()
        {

        }
        /// <summary>
        /// 构建SocketMessage
        /// </summary>
        /// <param name="opCode">操作数</param>
        /// <param name="subCode">子操作</param>
        /// <param name="value">参数</param>
        public SocketMessage(int opCode,int subCode,object value)
        {
            this.opCode = opCode;
            this.subCode = subCode;
            this.value = value;
        }

        public void Change(int opCode, int subCode, object value)
        {
            this.opCode = opCode;
            this.subCode = subCode;
            this.value = value;
        }
    }
}
