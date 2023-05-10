using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserShopLoadingService : IUserLoadingService
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		public UserShopLoadingService(
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
		{
			var model = _metaGameModelProvider.Get().ShopModel;
			var data = model.ToData();
			
			var packsResponse = await _restApiClient.GetShopPacksAll();
			if (packsResponse.successful && packsResponse.response != null)
			{
				for (var i = 0; i < packsResponse.response.items.Count; i++)
				{
					var remotePack = packsResponse.response.items[i];
					
					data.Packs.Add(new ShopPackData()
					{
						Id = remotePack.id,
						Price = remotePack.priceTon,
						Rewards = new ShopPackRewardsData()
						{
							Blueprints = remotePack.blueprints,
							Coins = remotePack.coins,
							Energy = remotePack.energy,
							HeroSkins = remotePack.heroSkins,
							KeysCommon = remotePack.keysCommon,
							KeysUncommon = remotePack.keysUncommon,
							KeysRare = remotePack.keysRare,
							KeysLegendary = remotePack.keysLegendary,
						}
					});
				}
			}
			
			var resourcesResponse = await _restApiClient.GetShopResourcesAll();
			if (resourcesResponse.successful && resourcesResponse.response != null)
			{
				if (resourcesResponse.response.items != null)
				{
					TryAddResourceData(data, resourcesResponse.response.items.COMMON,"resource_items_common", RarityName.COMMON, ShopResourceType.Items);
					TryAddResourceData(data, resourcesResponse.response.items.UNCOMMON,"resource_items_uncommon", RarityName.UNCOMMON, ShopResourceType.Items);
					TryAddResourceData(data, resourcesResponse.response.items.RARE,"resource_items_rare", RarityName.RARE, ShopResourceType.Items);
					TryAddResourceData(data, resourcesResponse.response.items.LEGENDARY,"resource_items_legendary", RarityName.LEGENDARY, ShopResourceType.Items);
				}
				
				if (resourcesResponse.response.keys != null)
				{
					TryAddResourceData(data, resourcesResponse.response.keys.COMMON,"resource_keys_common", RarityName.COMMON, ShopResourceType.Keys);
					TryAddResourceData(data, resourcesResponse.response.keys.UNCOMMON,"resource_keys_uncommon", RarityName.UNCOMMON, ShopResourceType.Keys);
					TryAddResourceData(data, resourcesResponse.response.keys.RARE,"resource_keys_rare", RarityName.RARE, ShopResourceType.Keys);
					TryAddResourceData(data, resourcesResponse.response.keys.LEGENDARY,"resource_keys_legendary", RarityName.LEGENDARY, ShopResourceType.Keys);
				}

				TryAddResourceData(data, resourcesResponse.response.energy,"resource_energy", RarityName.COMMON, ShopResourceType.Energy);
				TryAddResourceData(data, resourcesResponse.response.blueprints,"resource_blueprints", RarityName.COMMON, ShopResourceType.Blueprints);
				TryAddResourceData(data, resourcesResponse.response.coins,"resource_coins", RarityName.COMMON, ShopResourceType.Coins);
			}
			
			model.Update(data);
		}
		
		private static void TryAddResourceData(ShopData data, ShopResourcesResponse.Resource resourceResponse, string id, RarityName rarity, ShopResourceType type)
		{
			if (resourceResponse == null || resourceResponse.amount <= 0) return;
			
			data.Resources.Add(new ShopResourceData()
			{
				Id = id,
				Rarity = rarity,
				Amount = resourceResponse.amount,
				Price = resourceResponse.price,
				Type = type
			});
		}

		public class Factory : PlaceholderFactory<UserShopLoadingService>
		{
		}
	}
}