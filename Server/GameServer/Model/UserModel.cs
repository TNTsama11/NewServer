using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 角色数据模型
    /// </summary>
    public class UserModel
    {
        public string Account; //玩家Account
        public string Name; //玩家名
        public int IconID; //头像ID
        public int ModelID; //模型ID
        public int Lv; //等级

        public UserModel(string acc,string name,int lv, int iconID,int modelID)
        {
            this.Account = acc;
            this.Name = name;
            this.IconID = iconID;
            this.ModelID = modelID;
            this.Lv = lv;
        }
    }
}
