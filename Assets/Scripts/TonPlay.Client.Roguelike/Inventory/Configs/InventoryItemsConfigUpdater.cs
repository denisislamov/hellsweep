using System;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemsConfigUpdater : IInventoryItemsConfigUpdater
	{
		private readonly InventoryItemsConfigProvider _provider;

		public InventoryItemsConfigUpdater(InventoryItemsConfigProvider provider)
		{
			_provider = provider;
		}

		public void Update(string id, ItemsGetResponse.Item item)
		{
			var rarity = (RarityName)Enum.Parse(typeof(RarityName), item.rarity, true);
			
			_provider.ConfigsMap[id] = new InventoryItemConfig(id: item.id, name: item.name, rarity: rarity);
		}
	}
}