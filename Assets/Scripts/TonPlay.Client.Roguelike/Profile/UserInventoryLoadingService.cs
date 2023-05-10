using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserInventoryLoadingService : IUserLoadingService
	{
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		
		public UserInventoryLoadingService(
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
		{
			var itemsResponseTask = _restApiClient.GetUserItems();
			var slotsResponseTask = _restApiClient.GetUserSlots();
			var inventoryResponseTask = _restApiClient.GetUserInventory();

			var itemsResponse = await itemsResponseTask;
			var slotsResponse = await slotsResponseTask;
			var inventoryResponse = await inventoryResponseTask;

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel.InventoryModel;
			var data = model.ToData();
			
			data.Items.Clear();
			data.Slots.Clear();
			data.MergeSlots.Clear();
			
			for (var i = 0; i < itemsResponse.response.items.Count; i++)
			{
				var itemData = itemsResponse.response.items[i];
				var innerItemConfig = _inventoryItemsConfigProvider.GetInnerItemConfig(itemData.itemId);
				
				data.Items.Add(new InventoryItemData()
				{
					Id = itemData.id,
					ItemId = itemData.itemId,
					DetailId = itemData.itemDetailId,
				});
			}
			
			for (var i = 0; i < slotsResponse.response.items.Count; i++)
			{
				var slotData = slotsResponse.response.items[i];
				var slotName = (SlotName) Enum.Parse(typeof(SlotName), slotData.purpose, true);
				
				data.Slots.Add(slotName, new SlotData()
				{
					Id = slotData.id,
					SlotName = slotName,
					ItemId = slotData.userItemId
				});
			}

			for (var i = 0; i < 3; i++)
			{
				data.MergeSlots.Add(new SlotData()
				{
					Id = string.Empty,
					SlotName = SlotName.NECK,
					ItemId = string.Empty
				});
			}

			data.BlueprintsArms = inventoryResponse.response.blueprintsArms;
			data.BlueprintsBody = inventoryResponse.response.blueprintsBody;
			data.BlueprintsBelt = inventoryResponse.response.blueprintsBelt;
			data.BlueprintsFeet = inventoryResponse.response.blueprintsFeet;
			data.BlueprintsNeck = inventoryResponse.response.blueprintsNeck;
			data.BlueprintsWeapon = inventoryResponse.response.blueprintsWeapon;

			data.CommonKeys = inventoryResponse.response.keysCommon;
			data.UncommonKeys = inventoryResponse.response.keysUncommon;
			data.RareKeys = inventoryResponse.response.keysRare;
			data.LegendaryKeys = inventoryResponse.response.keysLegendary;
			
			model.Update(data);
		}
		
		public class Factory : PlaceholderFactory<UserInventoryLoadingService>
		{
		}
	}
}