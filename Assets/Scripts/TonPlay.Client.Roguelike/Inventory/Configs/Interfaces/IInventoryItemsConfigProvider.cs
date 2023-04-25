using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigProvider
	{
		IReadOnlyDictionary<string, IInventoryInnerItemConfig> InnerItemMap { get; }

		IInventoryItemConfig Get(string id);

		IInventoryItemUpgradePriceConfig GetUpgradePrice(ushort level);
		
		IInventoryInnerItemConfig GetInnerItemConfig(string itemId);
	}
}