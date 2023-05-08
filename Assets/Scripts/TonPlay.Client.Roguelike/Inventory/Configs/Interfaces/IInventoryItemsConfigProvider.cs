using System.Collections.Generic;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigProvider
	{
		IReadOnlyDictionary<string, IInventoryInnerItemConfig> InnerItemMap { get; }

		IInventoryItemConfig Get(string id);
		IInventoryItemConfig GetConfigByDetailId(string itemDetailId);

		IInventoryItemUpgradePriceConfig GetUpgradePrice(ushort level);
		
		IInventoryInnerItemConfig GetInnerItemConfig(string itemId);

		public Dictionary<string, ItemsGetResponse.Item> GetNextRarityMap();
	}
}