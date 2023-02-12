using TonPlay.Client.Roguelike.Core.Locations.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
	public interface ILocationConfigStorage
	{
		ILocationConfig Current { get; }
	}
}