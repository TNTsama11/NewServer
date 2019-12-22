using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// 道具信息的数据传输对象
    /// </summary>
    [Serializable]
    public class PropsDto
    {
        public int type;
        public int id;
        public int subType;

        public int posX;
        public int posY;
        public int posZ;
    }
}
