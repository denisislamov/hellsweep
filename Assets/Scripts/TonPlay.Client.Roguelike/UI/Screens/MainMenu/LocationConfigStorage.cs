using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class LocationConfigStorage : ILocationConfigStorage, ILocationConfigStorageSelector
	{
		private readonly ReactiveProperty<ILocationConfig> _current = new ReactiveProperty<ILocationConfig>();
		
		public IReadOnlyReactiveProperty<ILocationConfig> Current => _current;

		public void Select(ILocationConfig locationConfig)
		{
			_current.SetValueAndForceNotify(locationConfig);
		}
	}
}