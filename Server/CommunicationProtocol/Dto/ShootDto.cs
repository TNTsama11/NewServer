using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class ShootDto
    {
        public string Account;

        public ShootDto()
        {

        }

        public ShootDto(string acc)
        {
            this.Account = acc;
        }

        public void Change(string acc)
        {
            this.Account = acc;
        }
    }
}
