using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class LocationConfigStorage : ILocationConfigStorage, ILocationConfigStorageSelector
	{
		public ILocationConfig Current { get; private set; }

		public void Select(ILocationConfig locationConfig)
		{
			Current = locationConfig;
		}
	}
}