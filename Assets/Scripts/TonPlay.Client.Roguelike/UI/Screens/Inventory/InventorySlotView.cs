using System;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventorySlotView : View, IInventorySlotView
	{
		[SerializeField]
		private ButtonView _buttonView;
		
		[SerializeField]
		private Image _icon;
		
		[SerializeField]
		private RawImage _background;

		[SerializeField]
		private Image _emptyIcon;
		
		public IObservable<Unit> OnClick => _buttonView.OnClick;

		public IButtonView ButtonView => _buttonView;
		
		public void SetEmptyState(bool state)
		{
			_emptyIcon.gameObject.SetActive(state);
			_icon.gameObject.SetActive(!state);
			_background.gameObject.SetActive(!state);
		}
		
		public void SetIcon(Sprite sprite)
		{
			_icon.sprite = sprite;
		}
		
		public void SetBackgroundMaterial(Material material)
		{
			_background.material = material;
		}
	}
}