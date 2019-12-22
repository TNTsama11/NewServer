using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// Transform 数据传输对象
    /// </summary>
    [Serializable]
    public class TransformDto
    {
        public string Account { get; set; }

        /// <summary>
        /// position [x,y,z]
        /// </summary>
        public float[] pos = new float[3];
        /// <summary>
        /// rotation [x,y,z]
        /// </summary>
        public float[] rota = new float[3];

        public TransformDto()
        {

        }
        public TransformDto(string acc,float[] pos, float[] rota)
        {
            this.Account = acc;
            this.pos = pos;
            this.rota = rota;
        }
        public void Change(string acc, float[] pos, float[] rota)
        {
            this.Account = acc;
            this.pos = pos;
            this.rota = rota;
        }
    }
}
