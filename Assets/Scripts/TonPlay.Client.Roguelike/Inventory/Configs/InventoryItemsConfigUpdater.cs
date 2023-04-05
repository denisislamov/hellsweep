using System;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Player.Configs;
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
			var slotName = (SlotName)Enum.Parse(typeof(SlotName), item.purpose, true);
			var attributeName = (AttributeName)Enum.Parse(typeof(AttributeName), item.feature, true);

			_provider.ConfigsMap[id] = new InventoryItemConfig(
				id: item.id,
				name: item.name,
				rarity: rarity,
				slotName: slotName,
				attributeName: attributeName,
				details: item.details
							 .ToDictionary(
								  _ => _.level,
								  _ => (IInventoryItemDetailConfig)new InventoryItemDetailConfig()
								  {
									  Feature = _.feature,
									  Level = _.level,
									  Value = _.value
								  }));
		}
	}
}