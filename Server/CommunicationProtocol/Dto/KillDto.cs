using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class KillDto
    {
        public string Account { get; set; }
        public int Kill { get; set; }

        public KillDto()
        {

        }
        public KillDto(string acc, int kill)
        {
            this.Account = acc;
            this.Kill = kill;
        }
        public void Change(string acc, int kill)
        {
            this.Account = acc;
            this.Kill = kill;
        }
    }
}
