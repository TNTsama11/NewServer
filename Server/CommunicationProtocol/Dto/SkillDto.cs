using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class SkillDto
    {
        public string Account { get; set; }
        public int SkillType { get; set; }

        public SkillDto()
        {

        }

        public SkillDto(string acc,int type)
        {
            this.Account = acc;
            this.SkillType = type;
        }
    }
}
