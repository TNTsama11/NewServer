using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOne
{
    /// <summary>
    /// 工具类
    /// </summary>
  public  class Tool
    {
        /// <summary>
        /// 控制台输出带时间戳的信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void PrintMessage(string message)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString().ToString() + "-" + message);
        }
    }
}
