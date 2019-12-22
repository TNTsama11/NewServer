using System;
using System.Threading;

namespace ServerOne.mTimer
{
    /// <summary>
    /// 计时器
    /// </summary>
    public class MTimer
    {
        public Timer timer { get; set; }
        public ClientPeer client { get; set; }
        public int t { get; set; }
    }
}
