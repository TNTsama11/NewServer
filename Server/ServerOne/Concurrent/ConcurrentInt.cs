using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOne.Concurrent
{
    /// <summary>
    /// 线程安全的int
    /// </summary>
    public class ConcurrentInt
    {
        private int value;
        public ConcurrentInt(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// 增加获取
        /// </summary>
        /// <returns></returns>
        public int Add_Get()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }
        /// <summary>
        /// 减少获取
        /// </summary>
        /// <returns></returns>
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return value;
        }
    }
}
