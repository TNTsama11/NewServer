using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationProtocol.Dto
{
    /// <summary>
    /// 游戏房间数据传输对象
    /// </summary>
    [Serializable]
    public class GameRoomDto
    {
        public Dictionary<string, UserDto> UserAccDtoDict;
        public Dictionary<string, TransformDto> UserTransDto;
        public Dictionary<string, HpDto> UserHpDict;
        public Dictionary<string, HgDto> UserHgDict;
        public Dictionary<string, KillDto> UserKillDict;

        public GameRoomDto()
        {
            UserAccDtoDict = new Dictionary<string, UserDto>();
            UserTransDto = new Dictionary<string, TransformDto>();
            UserHpDict = new Dictionary<string, HpDto>();
            UserHgDict = new Dictionary<string, HgDto>();
            UserKillDict = new Dictionary<string, KillDto>();
        }
    }
}
