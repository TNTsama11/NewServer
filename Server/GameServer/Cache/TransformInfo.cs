using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    /// <summary>
    /// Transform信息类
    /// </summary>
    public class TransformInfo
    {
        /// <summary>
        /// position [x,y,z]
        /// </summary>
        public float[] pos=new float[3];
        /// <summary>
        /// rotation [x,y,z]
        /// </summary>
        public float[] rota=new float[3];

        public TransformInfo()
        {

        }
        public TransformInfo(float[]pos,float[]rota)
        {
            this.pos = pos;
            this.rota = rota;
        }
        public void Change(float[] pos, float[] rota)
        {
            this.pos = pos;
            this.rota = rota;
        }
    }
}
