using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenContext
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