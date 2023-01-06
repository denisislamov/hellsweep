using TonPlay.Roguelike.Client.Core.Models.Data;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models.Interfaces
{
	public interface IPlayerModel
	{
		IReadOnlyReactiveProperty<int> Health { get; }
		
		IReadOnlyReactiveProperty<int> MaxHealth { get; }
		
		IReadOnlyReactiveProperty<float> Experience { get; }
		
		void Update(PlayerData data);
		
		PlayerData ToData();
	}
}