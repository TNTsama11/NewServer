using System;
using System.Collections.Generic;
using ServerOne;
using CommunicationProtocol.Dto;

namespace GameServer.DataBaseTools
{
    public class UpdataUserArg
    {
        public ClientPeer Client { get; set; }
        public string Account { get; set; }
        public UserDto Dto { get; set; }
    }
}
