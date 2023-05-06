using System.Collections.Generic;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigUpdater
	{
		void Update(string id, ItemsGetResponse.Item item);
		
		void UpdateItemUpgradePrices(ushort level, ItemLevelRatesResponse.Item remoteConfig);
		void UpdateItemRarenessConfigs(List<ItemsGetResponse.Item> responseItems);
	}
}