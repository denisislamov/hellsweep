using TonPlay.Roguelike.Client.Core.Models.Data;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models.Interfaces
{
	public interface IPlayerModel
	{
		IReadOnlyReactiveProperty<float> Health { get; }
		
		IReadOnlyReactiveProperty<float> MaxHealth { get; }
		
		IReadOnlyReactiveProperty<float> Experience { get; }
		
		IReadOnlyReactiveProperty<float> MaxExperience { get; }
		
		ISkillsModel SkillsModel { get; }
		
		void Update(PlayerData data);
		
		PlayerData ToData();
	}
}