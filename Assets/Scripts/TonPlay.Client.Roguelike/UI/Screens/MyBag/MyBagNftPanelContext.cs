using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	public class MyBagNftPanelContext : ScreenContext, IMyBagNftPanelContext
	{
		public Action ClosePanelAction { get; }

		public MyBagNftPanelContext(Action closePanelAction)
		{
			ClosePanelAction = closePanelAction;
		}
	}
}