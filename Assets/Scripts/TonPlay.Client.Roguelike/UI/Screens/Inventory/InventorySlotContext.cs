using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventorySlotContext : ScreenContext, IInventorySlotContext
	{
		public ISlotModel SlotModel { get; }
		public Action ClickCallback { get; }
		
		public InventorySlotContext(ISlotModel slotModel, Action clickCallback)
		{
			SlotModel = slotModel;
			ClickCallback = clickCallback;
		}
	}
}