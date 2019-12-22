using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerOne
{
    public delegate void ExeuteDelegate();

    /// <summary>
    /// 单线程方法
    /// 保证应用层单线程调用传递的方法
    /// 避免多线程同时访问数据资源产生错误
    /// </summary>
    public class SingleExcute
    {
        //单例 线程安全
        private static SingleExcute instance = null;
        private static readonly object L = new object();
        public static SingleExcute Instance
        {
            get
            {
                lock (L)
                {
                    if (instance == null)
                    {
                        instance = new SingleExcute();
                    }
                    return instance;
                }
            }
        }


        /// <summary>
        /// 互斥锁
        /// </summary>
        public Mutex mutex;
        private SingleExcute()
        {
            mutex = new Mutex();
        }
        /// <summary>
        /// 单线程逻辑
        /// </summary>
        /// <param name="exeuteDelegate"></param>
        public void Exeute(ExeuteDelegate executeDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                executeDelegate();
                mutex.ReleaseMutex();
            }
        }

    }
}
