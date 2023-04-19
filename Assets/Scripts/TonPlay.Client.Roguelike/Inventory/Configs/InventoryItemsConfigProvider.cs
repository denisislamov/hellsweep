using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemsConfigProvider : IInventoryItemsConfigProvider
	{
		public DictionaryExt<string, IInventoryItemConfig> ConfigsMap { get; } = new DictionaryExt<string, IInventoryItemConfig>();
		public DictionaryExt<ushort, IInventoryItemUpgradePriceConfig> UpgradePricesMap { get; } = new DictionaryExt<ushort, IInventoryItemUpgradePriceConfig>();

		public IInventoryItemConfig Get(string id) => !string.IsNullOrWhiteSpace(id) && ConfigsMap.ContainsKey(id) 
			? ConfigsMap[id] 
			: default;
		
		public IInventoryItemUpgradePriceConfig GetUpgradePrice(ushort level) => UpgradePricesMap.ContainsKey(level) 
			? UpgradePricesMap[level] 
			: default;
	}
}