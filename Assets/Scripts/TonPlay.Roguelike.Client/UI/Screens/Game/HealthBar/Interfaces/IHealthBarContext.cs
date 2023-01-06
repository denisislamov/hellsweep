using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Interfaces
{
	public interface IHealthBarContext : IScreenContext
	{
		IReadOnlyReactiveProperty<int> MaxHealth { get; }
		
		IReadOnlyReactiveProperty<int> CurrentHealth { get; }
	}
}