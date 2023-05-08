using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemContext : ScreenContext, IInventoryItemContext
	{
		public IReadOnlyReactiveProperty<string> UserItemId { get; }
		
		public string Name { get; }
		public Sprite Icon { get; }
		public Sprite SlotIcon { get; }

		public Color MainColor { get; }

		public IReadOnlyReactiveProperty<bool> IsEquipped { get; }
		public IReadOnlyReactiveProperty<MergeStates> MergeState { get; }
		
		public Material BackgroundGradientMaterial { get; }
		
		public Action ClickCallback { get; }

		public InventoryItemContext(
			IReadOnlyReactiveProperty<string> userItemId, 
			Sprite icon, 
			Sprite slotIcon, 
			Color mainColor, 
			Material backgroundGradientMaterial, 
			string name, 
			IReadOnlyReactiveProperty<bool> isEquipped,
			IReadOnlyReactiveProperty<MergeStates> mergeState,
			Action clickCallback)
		{
			UserItemId = userItemId;
			Icon = icon;
			MainColor = mainColor;
			BackgroundGradientMaterial = backgroundGradientMaterial;
			Name = name;
			IsEquipped = isEquipped;
			MergeState = mergeState;
			SlotIcon = slotIcon;
			ClickCallback = clickCallback;
		}
	}
}