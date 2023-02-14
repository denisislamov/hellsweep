using TonPlay.Client.Common.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces
{
	public interface ILevelProgressBarContext : IScreenContext
	{
		IReadOnlyReactiveProperty<int> Level { get; }
		IReadOnlyReactiveProperty<float> Value { get; }
		IReadOnlyReactiveProperty<float> MaxValue { get; }
	}
}