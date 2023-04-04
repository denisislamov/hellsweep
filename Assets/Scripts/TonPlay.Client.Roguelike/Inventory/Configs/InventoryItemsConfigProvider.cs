using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemsConfigProvider : IInventoryItemsConfigProvider
	{
		public DictionaryExt<string, IInventoryItemConfig> ConfigsMap { get; } = new DictionaryExt<string, IInventoryItemConfig>();

		public IInventoryItemConfig Get(string id) => ConfigsMap.ContainsKey(id) ? ConfigsMap[id] : default;
	}
}