using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models.Interfaces
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