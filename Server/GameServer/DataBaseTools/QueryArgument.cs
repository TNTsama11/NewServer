using System;
using System.Collections.Generic;
using ServerOne;

namespace GameServer.DataBaseTools
{
    public class QueryArgument
    {
        public string Account { get; set; }
        public ClientPeer client { get; set; }
    }
}
