using TonPlay.Client.Roguelike.Core.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IPlayerModel
	{
		IReadOnlyReactiveProperty<float> Health { get; }

		IReadOnlyReactiveProperty<float> MaxHealth { get; }

		IReadOnlyReactiveProperty<float> Experience { get; }

		IReadOnlyReactiveProperty<float> MaxExperience { get; }

		IMatchProfileGainModel MatchProfileGainModel { get; }

		ISkillsModel SkillsModel { get; }

		void Update(PlayerData data);

		PlayerData ToData();
	}
}