using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IBossModel : IModel<BossData>
	{
		IReadOnlyReactiveProperty<bool> Exists { get; }
		
		IReadOnlyReactiveProperty<float> Health { get; }

		IReadOnlyReactiveProperty<float> MaxHealth { get; }
	}
}