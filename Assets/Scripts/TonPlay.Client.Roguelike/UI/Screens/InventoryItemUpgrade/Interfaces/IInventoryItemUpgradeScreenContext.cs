using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces
{
	public interface IInventoryItemUpgradeScreenContext : IScreenContext
	{
		IInventoryItemModel Item { get; }
		
		Action<IInventoryItemModel> EquipButtonCallback { get; }
		
		bool IsEquipped { get; }
	}
}