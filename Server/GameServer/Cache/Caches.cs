using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    /// <summary>
    /// 缓存层管理类
    /// </summary>
    public class Caches
    {
        public static AccCache acc { get; set; }
        public static UserCache user { get; set; }
        public static MatchCache match { get; set; }
        public static GameCache game { get; set; }
        static Caches()
        {
            acc = new AccCache();
            user = new UserCache();
            match = new MatchCache();
            game = new GameCache();
        }
    }
}
