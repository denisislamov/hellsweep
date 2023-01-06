using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar
{
	public class HealthBarContext : ScreenContext, IHealthBarContext
	{
		public IReadOnlyReactiveProperty<int> MaxHealth { get; }
		
		public IReadOnlyReactiveProperty<int> CurrentHealth { get; }
		
		public HealthBarContext(IReadOnlyReactiveProperty<int> maxHealth, IReadOnlyReactiveProperty<int> currentHealth)
		{
			MaxHealth = maxHealth;
			CurrentHealth = currentHealth;
		}
	}
}