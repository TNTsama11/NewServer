using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 在控制台输出含有时间戳的信息
    /// </summary>
    public class Printer
    {
        /// <summary>
        /// 控制台输出带时间戳的信息
        /// </summary>
        /// <param name="message">信息</param>
        private void PrintMessage(string message)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString().ToString() + "-" + message);
        }
    }
}
