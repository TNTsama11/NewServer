using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// 玩家数据传输对象
    /// </summary>
    [Serializable]
    public class UserDto
    {
        public string Account; //玩家Account
        public string Name; //玩家名
        public int IconID; //头像ID
        public int ModelID; //模型ID
        public int Lv; //等级

        public UserDto()
        {

        }
        public UserDto(string acc,string name,int iconID,int modelID,int lv)
        {
            this.Account = acc;
            this.Name = name;
            this.IconID = iconID;
            this.ModelID = modelID;
            this.Lv = lv;
        }
    }
}
