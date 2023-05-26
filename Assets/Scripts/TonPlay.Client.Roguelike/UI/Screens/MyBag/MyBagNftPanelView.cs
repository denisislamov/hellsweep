using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	public class MyBagNftPanelView : View, IMyBagNftPanelView
	{
		[SerializeField] 
		private ButtonView _closeButtonView;
		
		public IButtonView CloseButtonView => _closeButtonView;
	}
}