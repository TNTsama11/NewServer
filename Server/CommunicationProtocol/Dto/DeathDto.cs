using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// 玩家死亡信息的传输对象
    /// </summary>
    [Serializable]
    public class DeathDto
    {
        public string KillerAccount { get; set; }
        public string Reason { get; set; }
        public string VictimAccount { get; set; }

        public DeathDto()
        {

        }

        public DeathDto(string Kacc,string Vacc,string reason)
        {
            this.KillerAccount = Kacc;
            this.VictimAccount = Vacc;
            this.Reason = reason;
        }
    }
}
