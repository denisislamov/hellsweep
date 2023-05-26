using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	public class MyBagPopupView : View, IMyBagPopupView
	{
		[SerializeField]
		private TMP_Text _titleText;

		[SerializeField] 
		private ButtonView _itemsButtonView;
		
		[SerializeField] 
		private ButtonView _nftButtonView;
		
		[SerializeField] 
		private ButtonView _allButtonView;
		
		[SerializeField]
		private ButtonView _closeButtonView;

		[SerializeField]
		private RectTransform _scrollLayoutTransform;
		
		[SerializeField] 
		private MyBagNftPanelView _nftPanelView;
		
		[SerializeField] 
		private MyBagItemsPanelView _itemsPanelView;

		public IButtonView CloseButtonView => _closeButtonView;
		
		public IButtonView ItemsButtonView => _itemsButtonView;
		
		public IButtonView NFTButtonView => _nftButtonView;
		
		public IButtonView AllButtonView => _allButtonView;
		public IMyBagNftPanelView NftPanelView => _nftPanelView;
		public IMyBagItemsPanelView ItemsPanelView => _itemsPanelView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void RefreshLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollLayoutTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollLayoutTransform);
		}
	}
}