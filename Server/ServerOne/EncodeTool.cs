using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerOne
{
    /// <summary>
    /// 数据包编码的工具类
    /// </summary>
     public  static class EncodeTool
    {
        #region 封装和解析数据包

        /// <summary>
        /// 构造数据包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncodePackage(byte[] data) 
        {
            using (MemoryStream ms=new MemoryStream()) //使用using关键字会自动释放资源
            {
                using (BinaryWriter bw=new BinaryWriter(ms))
                {
                    bw.Write(data.Length); //先写入长度头
                    bw.Write(data); //再写入数据体尾
                    byte[] byteArray = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length); //在内存中高效率拷贝
                    return byteArray;
                }
            }
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="dataCache"></param>
        /// <returns></returns>
        public static byte[] DecodePackage(ref List<byte> dataCache)
        {
            if (dataCache.Count < 4) //如果数据缓存长度小于四字节int长度，无法构成数据包
            {
                //Tool.PrintMessage("数据包长度小于4字节，无法构成数据包");
                return null;
            }
            using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
            {
                using (BinaryReader br=new BinaryReader(ms))
                {
                    int length = br.ReadInt32(); //包头约定长度
                    int dataRemainLength = (int)(ms.Length - ms.Position); //剩余长度
                    if (length > dataRemainLength) //如果包头表示的数据长度大于剩余数据长度说明包不完整
                    {
                        Tool.PrintMessage("数据包剩余长度小于数据包头规定长度，数据包不完整无法解析");
                        return null;                       
                    }
                    byte[] data = br.ReadBytes(length);
                    //更新数据缓存
                    dataCache.Clear(); //先清空
                    dataCache.AddRange(br.ReadBytes(dataRemainLength)); //再将剩余的数据存入
                    return data;
                }
            }
        }

        #endregion

        #region 构造SocketMessage

        /// <summary>
        /// 将SocketMessage转成字节数组发送
        /// </summary>
        /// <param name="smg"></param>
        /// <returns></returns>
        public static byte[] EncodeMessage(SocketMessage smg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(smg.opCode); //写入顺序和读取顺序一致
            bw.Write(smg.subCode);
            if (smg.value != null)
            {
                byte[] valueBytes = EncodeObject(smg.value);
                bw.Write(valueBytes);
            }
            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
            bw.Close();
            ms.Close();
            return data;
        }
        /// <summary>
        /// 将字节数组转成SocketMessage对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SocketMessage DecodeMessage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);
            SocketMessage smg = new SocketMessage();
            smg.opCode = br.ReadInt32();
            smg.subCode = br.ReadInt32();
            if (ms.Length > ms.Position) //如果长度大于当前读取位置说明后面有参数
            {
                byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                object value = DecodeObject(valueBytes);
                smg.value = value;
            }
            br.Close();
            ms.Close();
            return smg;
        }

        #endregion
       
        #region 序列化和反序列化object

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public static byte[] EncodeObject(object value)
        {
            using (MemoryStream ms=new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter(); //C#提供的序列化与反序列化类
                bf.Serialize(ms, value);
                byte[] valueBytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
                return valueBytes;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static object DecodeObject(byte[] valueBytes)
        {
            using(MemoryStream ms=new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value= bf.Deserialize(ms);
                return value;
            }
        }

        #endregion
    }
}
