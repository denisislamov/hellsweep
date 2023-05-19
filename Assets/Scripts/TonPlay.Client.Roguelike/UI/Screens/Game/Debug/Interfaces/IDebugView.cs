using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces
{
	public interface IDebugView : IView
	{
		IButtonView StartFirstBossButtonView { get; }
		
		IButtonView StartSecondBossButtonView { get; }
		
		IButtonView StartThirdBossButtonView { get; }
		
		IButtonView WinButtonView { get; }
		
		IButtonView LoseButtonView { get; }
		
		void SetFramerateText(string text);
		
		void SetEnemyMovementCollisionCount(int value);
	}
}