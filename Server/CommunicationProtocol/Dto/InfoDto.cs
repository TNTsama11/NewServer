using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class InfoDto
    {
        public string Account;
        public string message;

        public InfoDto()
        {

        }

        public InfoDto(string msg,string acc = "")
        {
            this.message = msg;
            this.Account = acc;
        }

        public void Change( string msg, string acc = "")
        {
            this.message = msg;
            this.Account = acc;
        }
    }
}
