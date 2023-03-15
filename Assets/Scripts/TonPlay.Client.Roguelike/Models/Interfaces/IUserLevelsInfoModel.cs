using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public interface IUserLevelsInfoModel : IModel<UserLevelsInfoData>
    {
        IList<IUserLevelInfoModel> LevelsInfo { get; }
    }
}