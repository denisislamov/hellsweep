using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
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
					if (resourcesResponse.response.items.COMMON != null && resourcesResponse.response.items.COMMON.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_items_common",
							Rarity = RarityName.COMMON, 
							Amount = resourcesResponse.response.items.COMMON.amount, 
							Price = resourcesResponse.response.items.COMMON.price,
							Type = ShopResourceType.Items
						});
					}
					
					if (resourcesResponse.response.items.UNCOMMON != null && resourcesResponse.response.items.UNCOMMON.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_items_uncommon",
							Rarity = RarityName.UNCOMMON, 
							Amount = resourcesResponse.response.items.UNCOMMON.amount, 
							Price = resourcesResponse.response.items.UNCOMMON.price,
							Type = ShopResourceType.Items
						});
					}
					
					if (resourcesResponse.response.items.RARE != null && resourcesResponse.response.items.RARE.amount != 0l)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_items_rare",
							Rarity = RarityName.RARE, 
							Amount = resourcesResponse.response.items.RARE.amount, 
							Price = resourcesResponse.response.items.RARE.price,
							Type = ShopResourceType.Items
						});
					}
					
					if (resourcesResponse.response.items.LEGENDARY != null && resourcesResponse.response.items.LEGENDARY.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_items_legendary",
							Rarity = RarityName.LEGENDARY, 
							Amount = resourcesResponse.response.items.LEGENDARY.amount, 
							Price = resourcesResponse.response.items.LEGENDARY.price,
							Type = ShopResourceType.Items
						});
					}
				}

				if (resourcesResponse.response.keys != null)
				{
					if (resourcesResponse.response.keys.COMMON != null && resourcesResponse.response.keys.COMMON.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_keys_common",
							Rarity = RarityName.COMMON, 
							Amount = resourcesResponse.response.keys.COMMON.amount, 
							Price = resourcesResponse.response.keys.COMMON.price,
							Type = ShopResourceType.Keys
						});
					}
					
					if (resourcesResponse.response.keys.UNCOMMON != null && resourcesResponse.response.keys.UNCOMMON.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_keys_uncommon",
							Rarity = RarityName.UNCOMMON, 
							Amount = resourcesResponse.response.keys.UNCOMMON.amount, 
							Price = resourcesResponse.response.keys.UNCOMMON.price,
							Type = ShopResourceType.Keys
						});
					}
					
					if (resourcesResponse.response.keys.RARE != null && resourcesResponse.response.keys.RARE.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_keys_rare",
							Rarity = RarityName.RARE, 
							Amount = resourcesResponse.response.keys.RARE.amount, 
							Price = resourcesResponse.response.keys.RARE.price,
							Type = ShopResourceType.Keys
						});
					}
					
					if (resourcesResponse.response.keys.LEGENDARY != null && resourcesResponse.response.keys.LEGENDARY.amount != 0)
					{
						data.Resources.Add(new ShopResourceData(){
							Id = "resource_keys_legendary",
							Rarity = RarityName.LEGENDARY, 
							Amount = resourcesResponse.response.keys.LEGENDARY.amount, 
							Price = resourcesResponse.response.keys.LEGENDARY.price,
							Type = ShopResourceType.Keys
						});
					}
				}

				if (resourcesResponse.response.energy != null)
				{
					data.Resources.Add(new ShopResourceData()
					{
						Id = "resource_energy",
						Rarity = RarityName.COMMON,
						Amount = resourcesResponse.response.energy.amount,
						Price = resourcesResponse.response.energy.price,
						Type = ShopResourceType.Energy
					});
				}

				if (resourcesResponse.response.blueprints != null)
				{
					data.Resources.Add(new ShopResourceData()
					{
						Id = "resource_blueprints",
						Rarity = RarityName.COMMON,
						Amount = resourcesResponse.response.blueprints.amount,
						Price = resourcesResponse.response.blueprints.price,
						Type = ShopResourceType.Blueprints
					});
				}

				if (resourcesResponse.response.coins != null)
				{
					data.Resources.Add(new ShopResourceData(){
						Id = "resource_coins",
						Rarity = RarityName.COMMON, 
						Amount = resourcesResponse.response.coins.amount, 
						Price = resourcesResponse.response.coins.price,
						Type = ShopResourceType.Coins
					});
				}
			}
			
			model.Update(data);
		}

		public class Factory : PlaceholderFactory<UserShopLoadingService>
		{
		}
	}
}