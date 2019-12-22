using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    [Serializable]
    public class CreatPropsDto
    {
        public Dictionary<int, PropsDto> idPropsTypeDict;

        public CreatPropsDto()
        {
            idPropsTypeDict = new Dictionary<int, PropsDto>();
        }
    }
}
