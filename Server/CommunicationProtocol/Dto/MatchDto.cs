using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// 匹配房间数据传输对象
    /// </summary>
    [Serializable]
    public class MatchDto
    {
        /// <summary>
        /// 玩家Account与玩家对应数据传输模型
        /// </summary>
        public Dictionary<string, UserDto> accUserDict { get; private set; }
        /// <summary>
        /// 准备的玩家
        /// </summary>
        public List<string> ReadyUserList { get;  set; }

        public MatchDto()
        {
            accUserDict = new Dictionary<string, UserDto>();
            ReadyUserList = new List<string>();
        }
    }
}
