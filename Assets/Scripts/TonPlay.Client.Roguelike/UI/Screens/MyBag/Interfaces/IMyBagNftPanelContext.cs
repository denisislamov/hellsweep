using System;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces
{
	public interface IMyBagNftPanelContext : IScreenContext
	{
		Action ClosePanelAction { get; }
	}
}