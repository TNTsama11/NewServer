using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class ArmsDto
    {
        public string Account { get; set; }
        public int Type { get; set; }

        public ArmsDto()
        {

        }

        public ArmsDto(string acc,int type)
        {
            this.Account = acc;
            this.Type = type;
        }
    }
}
