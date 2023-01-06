using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces
{
	public interface IGameView : IView
	{
		IHealthBarView HealthBarView { get; }
	}
}