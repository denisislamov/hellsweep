using System.Collections.Generic;
using System.Linq;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public class UserLevelsInfoModel : IUserLevelsInfoModel
    {
        private readonly UserLevelsInfoData _cached = new UserLevelsInfoData();

        private List<IUserLevelInfoModel> _levelsInfo = new List<IUserLevelInfoModel>();

        public IList<IUserLevelInfoModel> LevelsInfo => _levelsInfo;

        public void Update(UserLevelsInfoData data)
        {
            _levelsInfo = new List<IUserLevelInfoModel>(data.Levels.Count);
			
            for (int i = 0; i < _levelsInfo.Count; i++)
            {
                _levelsInfo[i].Update(data.Levels[i]);
            }
        }

        public UserLevelsInfoData ToData()
        {
            _cached.Levels = _levelsInfo.Select(_ => _.ToData()).ToList(); 
            return _cached;
        }
    }
}