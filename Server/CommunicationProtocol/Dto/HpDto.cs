using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class HpDto
    {
        public  string Account { get; set; }
        public int Hp { get; set; }

        public HpDto()
        {

        }
        public HpDto(string acc,int hp)
        {
            this.Account = acc;
            this.Hp = hp;
        }
        public void Change(string acc, int hp)
        {
            this.Account = acc;
            this.Hp = hp;
        }
    }
}
