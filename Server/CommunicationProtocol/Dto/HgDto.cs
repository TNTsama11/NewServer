using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class HgDto
    {
        public string Account { get; set; }
        public int Hg { get; set; }

        public HgDto()
        {

        }
        public HgDto(string acc, int hg)
        {
            this.Account = acc;
            this.Hg = hg;
        }
        public void Change(string acc, int hg)
        {
            this.Account = acc;
            this.Hg = hg;
        }
    }
}
