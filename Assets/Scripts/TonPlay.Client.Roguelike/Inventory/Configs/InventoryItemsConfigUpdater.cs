using System;
using System.Collections.Generic;
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

			var details = new Dictionary<string, IInventoryItemDetailConfig>();
			IInventoryItemDetailConfig previousDetail = null;
			
			for (var index = item.details.Count - 1; index >= 0; index--)
			{
				var remoteDetail = item.details[index];
				var currentDetail = new InventoryItemDetailConfig()
				{
					Feature = remoteDetail.id,
					Level = remoteDetail.level,
					Value = remoteDetail.value,
					Next = previousDetail
				};
				
				details.Add(remoteDetail.id, currentDetail);

				previousDetail = currentDetail;
			}
			
			_provider.ConfigsMap[id] = new InventoryItemConfig(
				id: item.id,
				name: item.name,
				rarity: rarity,
				slotName: slotName,
				attributeName: attributeName,
				details: details);
		}
		
		public void UpdateItemUpgradePrices(ushort level, ItemLevelRatesResponse.Item remoteConfig)
		{
			_provider.UpgradePricesMap[level] = new InventoryItemUpgradePriceConfig(remoteConfig.id, remoteConfig.coins, remoteConfig.blueprints);
		}
	}
}