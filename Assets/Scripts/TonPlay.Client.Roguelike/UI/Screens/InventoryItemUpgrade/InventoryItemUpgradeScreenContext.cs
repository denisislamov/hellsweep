using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	public class InventoryItemUpgradeScreenContext : ScreenContext, IInventoryItemUpgradeScreenContext
	{
		public IInventoryItemModel Item { get; }
		public Action<IInventoryItemModel> EquipButtonCallback { get; }
		public bool IsEquipped { get; }

		public InventoryItemUpgradeScreenContext(IInventoryItemModel item, Action<IInventoryItemModel> equipButtonCallback, bool isEquipped)
		{
			Item = item;
			EquipButtonCallback = equipButtonCallback;
			IsEquipped = isEquipped;
		}
	}
}