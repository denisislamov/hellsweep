using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemContext : ScreenContext, IInventoryItemContext
	{
		public string Id { get; }
		
		public string Name { get; }
		public ushort Level { get; }

		public Sprite Icon { get; }
		public Sprite SlotIcon { get; }

		public Color MainColor { get; }
		
		public Material BackgroundGradientMaterial { get; }
		
		public Action ClickCallback { get; }

		public InventoryItemContext(
			string id, 
			Sprite icon, 
			Sprite slotIcon, 
			Color mainColor, 
			Material backgroundGradientMaterial, 
			string name, 
			ushort level, 
			Action clickCallback)
		{
			Id = id;
			Icon = icon;
			MainColor = mainColor;
			BackgroundGradientMaterial = backgroundGradientMaterial;
			Name = name;
			Level = level;
			SlotIcon = slotIcon;
			ClickCallback = clickCallback;
		}
	}
}