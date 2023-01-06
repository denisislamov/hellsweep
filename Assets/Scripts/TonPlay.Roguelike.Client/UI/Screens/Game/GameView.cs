using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views;
using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	public class GameView : View, IGameView
	{
		[SerializeField]
		private HealthBarView _healthBarView;

		public IHealthBarView HealthBarView => _healthBarView;
	}
}