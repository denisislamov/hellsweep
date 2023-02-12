using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService
{
	public class ScreenContext : IScreenContext
	{
		private IScreen _screen;

		public IScreen Screen
		{
			get => _screen;
			set
			{
				if (_screen != null) return;
				_screen = value;
			}
		}

		public static ScreenContext Empty => new ScreenContext();
	}
}