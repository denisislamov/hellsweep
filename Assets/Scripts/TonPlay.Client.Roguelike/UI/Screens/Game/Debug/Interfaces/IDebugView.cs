using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces
{
	public interface IDebugView : IView
	{
		void SetFramerateText(string text);
		
		void SetEnemyMovementCollisionCount(int value);
	}
}