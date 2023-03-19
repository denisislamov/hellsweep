using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
	public interface ILocationConfigStorage
	{
		IReadOnlyReactiveProperty<ILocationConfig> Current { get; }
	}
}