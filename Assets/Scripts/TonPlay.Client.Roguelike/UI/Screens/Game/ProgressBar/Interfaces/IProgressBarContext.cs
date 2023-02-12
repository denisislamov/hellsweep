using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Interfaces
{
	public interface IProgressBarContext : IScreenContext
	{
		IReadOnlyReactiveProperty<float> MaxValue { get; }
		
		IReadOnlyReactiveProperty<float> CurrentValue { get; }
	}
}