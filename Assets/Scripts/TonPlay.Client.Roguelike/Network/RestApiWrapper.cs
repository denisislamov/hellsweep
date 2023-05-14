using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using UnityEngine;
using UnityEngine.Networking;

namespace TonPlay.Client.Roguelike.Network
{
    public class RestApiWrapper : MonoBehaviour, IRestApiClient
    {
        [SerializeField] private bool _useDebugToken;
        [SerializeField] private string _username;

        [Space(5)]
        [SerializeField] private DebugTokenSettings _debugTokenSettings;
        private string _token;
        private NetworkClient _networkClient;
        [SerializeField]
        private NetworkSettings _networkSettings;

        private void Awake()
        {
             Init();
        }

        public void Init()
        {
            var uri = _useDebugToken ? _debugTokenSettings.GetFullDebugAppUrl(_username) : Application.absoluteURL;
            _token = UriParser.Parse(uri)["token"];
            
            Debug.LogFormat("_token {0}", _token);
            
            _networkClient = new NetworkClient(
                _networkSettings.BaseAddress, 
                new System.TimeSpan(0, 0, 10),
                new SetupTokenDecorator(_token),
                new SetupHeadersDecorator());
        }

        public void Init(bool useDebugToken,
                         DebugTokenSettings debugTokenSettings,
                         NetworkSettings networkSettings, 
                         string username)
        {
           _useDebugToken = useDebugToken;
           _debugTokenSettings = debugTokenSettings;
           _networkSettings = networkSettings;
           _username = username;
        }

        // # Unit - Unit Controller
        [Space(10)]
        [Header("--------- Unit ---------")]
        [SerializeField] private Response<UnitAllResponse> _unitAllResponse;
        [ContextMenu("GetUnitAll")]
        public async UniTask<Response<UnitAllResponse>> GetUnitAll()
        {
            _unitAllResponse = new Response<UnitAllResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UnitAllResponse>");
            var getTask = _networkClient.GetAsync<UnitAllResponse>("v1/unit/all", null);

            var result = await getTask;
            _unitAllResponse = result;

            return result;
        }
        
        // # Skill - Skill Controller
        [Space(10)]
        [Header("--------- Skill ---------")]
        [SerializeField] private Response<SkillAllResponse> _skillAllResponse;
        [ContextMenu("GetSkillAll")]
        public async UniTask<Response<SkillAllResponse>> GetSkillAll()
        {
            _skillAllResponse = new Response<SkillAllResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<SkillAllResponse>");
            var getTask = _networkClient.GetAsync<SkillAllResponse>("v1/skill/all", null);

            var result = await getTask;
            _skillAllResponse = result;

            return result;
        }
        
        // # Boost - Boost controller
        [Space(10)]
        [Header("--------- Boost ---------")]
        [SerializeField] private Response<BoostAllResponse> _boostAllResponse;
        [ContextMenu("GetBoostAll")]
        public async UniTask<Response<BoostAllResponse>> GetBoostAll()
        {
            _boostAllResponse = new Response<BoostAllResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<BoostAllResponse>");
            var getTask = _networkClient.GetAsync<BoostAllResponse>("v1/boost/all", null);

            var result = await getTask;
            _boostAllResponse = result;

            return result;
        }
        
        // # Location - Location controller
        [Space(10)]
        [Header("--------- Location ---------")]
        [SerializeField] private Response<LocationAllResponse> _locationAllResponse;
        [ContextMenu("GetLocationAll")]
        public async UniTask<Response<LocationAllResponse>> GetLocationAll()
        {
            _locationAllResponse = new Response<LocationAllResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<BoostAllResponse>");
            var getTask = _networkClient.GetAsync<LocationAllResponse>("v1/location/all", null);

            var result = await getTask;
            _locationAllResponse = result;

            return result;
        }

        // # Info - Info controller
        [Space(10)]
        [Header("--------- Info ---------")]
        [SerializeField] private Response<InfoLevelAllResponse> _infoLevelAllResponse;
        [ContextMenu("GetInfoLevelAll")]
        public async UniTask<Response<InfoLevelAllResponse>> GetInfoLevelAll()
        {
            _infoLevelAllResponse = new Response<InfoLevelAllResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<InfoLevelAllResponse>");
            var getTask = _networkClient.GetAsync<InfoLevelAllResponse>("v1/info/level/all", null);

            var result = await getTask;
            _infoLevelAllResponse = result;

            return result;
        }
        
        // # Item - Item controller
        [Space(10)]
        [Header("--------- Item ---------")]
        [SerializeField] private ItemPutBody _itemPutBody;
        [SerializeField] private Response<ItemPutResponse> _itemPutResponse;
        [ContextMenu("PutItem")]
        public async UniTask<Response<ItemPutResponse>> PutItem(ItemPutBody value)
        {
            _itemPutResponse = new Response<ItemPutResponse>();

            Common.Utilities.Logger.Log($"_networkClient.PutAsync<ItemPutResponse> {value}");
            var putTask = _networkClient.PutAsync<ItemPutResponse, ItemPutBody>("v1/item", value);

            var result = await putTask;
            _itemPutResponse = result;

            return result;
        }
        
        [SerializeField] private ItemLevelUpPutBody _itemLevelUpPutBody;
        [SerializeField] private Response<ItemLevelUpPutResponse> _itemLevelUpPutResponse;
        [ContextMenu("PutItem")]
        public async UniTask<Response<ItemLevelUpPutResponse>> PutItemLevelUp(string id, bool isMax)
        {
            _itemLevelUpPutResponse = new Response<ItemLevelUpPutResponse>();

            Common.Utilities.Logger.Log($"_networkClient.PutAsync<ItemLevelUpPutResponse> {id} {isMax}");
            var putTask = _networkClient.PutAsync<ItemLevelUpPutResponse, ItemLevelUpPutBody>(
                $"v1/item/levelUp?id={id}&isMax={isMax}", 
                null);

            var result = await putTask;
            _itemLevelUpPutResponse = result;

            return result;
        }
        
        [SerializeField] private ItemMergePostBody _itemMergePostBody;
        [SerializeField] private Response<ItemMergeResponse> _itemMergeResponse;
        [ContextMenu("MergeItems")]
        public async UniTask<Response<ItemMergeResponse>> PostItemMerge(ItemMergePostBody value)
        {
            _itemMergeResponse = new Response<ItemMergeResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostItemMerge<ItemMergeResponse> {value}");
            var postTask = _networkClient.PostAsync<ItemMergeResponse, ItemMergePostBody>("/v1/item/merge", value);

            try
            
            {
                var result = await postTask;
                _itemMergeResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"PostItemMerge exception {e}");
                return _itemMergeResponse;
            }
        }
        
        [SerializeField] private Response<UserItemResponse> _itemLootResponse;
        [ContextMenu("PostItemLoot")]
        public async UniTask<Response<UserItemResponse>> PostItemLoot(RarityName rarity)
        {
            _itemLootResponse = new Response<UserItemResponse>();

            Common.Utilities.Logger.Log($"_networkClient.PostAsync<UserItemResponse> {rarity}");
            var postTask = _networkClient.PostAsync<UserItemResponse>($"v1/item/loot?rarity={rarity.ToString().ToUpperInvariant()}", null);

            var result = await postTask;
            
            _itemLootResponse = result;

            return result;
        }

        [SerializeField] private ItemMergePostBodyV2 _itemMergePostBodyV2;
        [SerializeField] private Response<ItemMergeResponse> _itemMergeResponseV2;
        [ContextMenu("MergeItemsV2")]
        public async UniTask<Response<ItemMergeResponse>> PostItemMergeV2(ItemMergePostBodyV2 value)
        {
            _itemMergeResponseV2 = new Response<ItemMergeResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostItemMerge<ItemMergeResponse> {value}");
            var postTask = _networkClient.PostAsync<ItemMergeResponse, ItemMergePostBodyV2>("/v2/item/merge", value);

            try
            
            {
                var result = await postTask;
                _itemMergeResponseV2 = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"PostItemMerge exception {e}");
                return _itemMergeResponseV2;
            }
        }
        
        [SerializeField] private Response<ItemsGetResponse> _itemsGetResponse;
        [ContextMenu("GetAllItems")]
        public async UniTask<Response<ItemsGetResponse>> GetAllItems()
        {
            _itemsGetResponse = new Response<ItemsGetResponse>();

            Common.Utilities.Logger.Log($"_networkClient.GetAsync<ItemsGetResponse> {_itemsGetResponse}");
            var putTask = _networkClient.GetAsync<ItemsGetResponse>("v1/item/all", null);

            var result = await putTask;
            _itemsGetResponse = result;

            return result;
        }
        
        [SerializeField] private Response<ItemLevelRatesResponse> _itemLevelRatesResponse;
        [ContextMenu("GetAllItems")]
        public async UniTask<Response<ItemLevelRatesResponse>> GetItemLevelRatesAll()
        {
            _itemLevelRatesResponse = new Response<ItemLevelRatesResponse>();

            Common.Utilities.Logger.Log($"_networkClient.GetAsync<ItemLevelRatesResponse> {_itemLevelRatesResponse}");
            var putTask = _networkClient.GetAsync<ItemLevelRatesResponse>("v1/item/lvl/rate/all", null);

            var result = await putTask;
            _itemLevelRatesResponse = result;

            return result;
        }

        [ContextMenu("DeleteItem")]
        public async UniTask<Response<string>> DeleteItem(string slotId)
        {
            Common.Utilities.Logger.Log($"_networkClient.DeleteAsync<string> {slotId}");
            var deleteTask = _networkClient.DeleteAsync<string>("v1/item/" + slotId, "");
            var result = await deleteTask;
            
            return result;
        }

        // # Game - Game controller
        [Space(10)]
        [Header("--------- Game ---------")]
        [SerializeField] private Response<GameSessionResponse> _gameSessionResponse;
        [ContextMenu("GetGameSession")]
        public async UniTask<Response<GameSessionResponse>> GetGameSession()
        {
            _gameSessionResponse = new Response<GameSessionResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<GameSessionResponse>");
            var getTask = _networkClient.GetAsync<GameSessionResponse>("v1/game/session", null);

            try
            {
                var result = await getTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"GetGameSession exception {e}");
                return _gameSessionResponse;
            }
        }

        [ContextMenu("PostGameSession - GameSessionPutBody")]
        public async UniTask<Response<GameSessionResponse>> PostGameSessionClose(CloseGameSessionPostBody value)
        {
            _gameSessionResponse = new Response<GameSessionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PutAsync<GameSessionResponse> {value}");
            var postTask = _networkClient.PostAsync<GameSessionResponse, CloseGameSessionPostBody>("v1/game/session/close", value);

            try
            
            {
                var result = await postTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"GetGameSession exception {e}");
                return _gameSessionResponse;
            }
        }

        [ContextMenu("PostGameSession")]
        public async UniTask<Response<GameSessionResponse>> PostGameSession(OpenGameSessionPostBody value)
        {
            _gameSessionResponse = new Response<GameSessionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<GameSessionResponse> {value}");
            var postTask = _networkClient.PostAsync<GameSessionResponse, OpenGameSessionPostBody>("v1/game/session", value);

            try
            {
                var result = await postTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"GetGameSession exception {e}");
                return _gameSessionResponse;
            }
        }
        
        [SerializeField] private Response<GamePropertiesResponse> _gamePropertiesResponse;
        [ContextMenu("GetGameProperties")]
        public async UniTask<Response<GamePropertiesResponse>> GetGameProperties()
        {
            _gamePropertiesResponse = new Response<GamePropertiesResponse>(); 
            Common.Utilities.Logger.Log("_networkClient.GetAsync<GamePropertiesResponse>");
            var getTask = _networkClient.GetAsync<GamePropertiesResponse>("v1/game/properties", null);

            try
            {
                var result = await getTask;
                _gamePropertiesResponse = result;
                
                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"GetGameProperties exception {e}");
                return _gamePropertiesResponse;
            }
        }

        [ContextMenu("PostGameProperties")]
        public async UniTask<Response<GamePropertiesResponse>> PostGameProperties(GamePropertiesResponse value)
        {
            _gamePropertiesResponse = new Response<GamePropertiesResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostGameProperties<GamePropertiesResponse> {value}");
            
            var postTask = _networkClient.PostAsync<GamePropertiesResponse, GamePropertiesResponse>("v1/game/properties", value);

            try
            {
                var result = await postTask;
                _gamePropertiesResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Common.Utilities.Logger.Log($"GetGameSession exception {e}");
                return _gamePropertiesResponse;
            }
        }

        // # User - User Controller
        [Space(10)]
        [Header("--------- User ---------")]
       
        [SerializeField] private Response<UserXpResponse> _userXpResponse;
        [ContextMenu("GetUserXp")]
        public async UniTask<Response<UserXpResponse>> GetUserXp()
        {
            _userXpResponse = new Response<UserXpResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserXpResponse>");
            var getTask = _networkClient.GetAsync<UserXpResponse>("v1/user/xp", null);

            var result = await getTask;
            _userXpResponse = result;

            return result;
        }

        [SerializeField] private Response<UserSummaryResponse> _userSummaryResponse;
        [ContextMenu("GetUserSummary")]
        public async UniTask<Response<UserSummaryResponse>> GetUserSummary()
        {
            _userSummaryResponse = new Response<UserSummaryResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserSummaryResponse>");
            var getTask = _networkClient.GetAsync<UserSummaryResponse>("v1/user/summary", null);

            var result = await getTask;
            _userSummaryResponse = result;
            
            return result;
        }

        [SerializeField] private Response<UserSlotsResponse> _userSlotsResponse;
        [ContextMenu("GetUserSlots")]
        public async UniTask<Response<UserSlotsResponse>> GetUserSlots()
        {
            _userSlotsResponse = new Response<UserSlotsResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserSlotsResponse>");
            var getTask = _networkClient.GetAsync<UserSlotsResponse>("/v1/user/slots", null);

            var result = await getTask;
            _userSlotsResponse = result;
            
            return result;
        }
        
        [SerializeField] private Response<UserInventoryResponse> _userInventoryResponse;
        [ContextMenu("GetUserItems")]
        public async UniTask<Response<UserInventoryResponse>> GetUserInventory()
        {
            _userInventoryResponse = new Response<UserInventoryResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserInventoryResponse>");
            var getTask = _networkClient.GetAsync<UserInventoryResponse>("v1/user/inventory", null);

            var result = await getTask;
            _userInventoryResponse = result;
            
            return result;
        }

        [SerializeField] private Response<UserItemsResponse> _userItemsResponse;
        [ContextMenu("GetUserItems")]
        public async UniTask<Response<UserItemsResponse>> GetUserItems()
        {
            _userItemsResponse = new Response<UserItemsResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserItemsResponse>");
            var getTask = _networkClient.GetAsync<UserItemsResponse>("v1/user/item/all", null);

            var result = await getTask;
            _userItemsResponse = result;
            
            return result;
        }

        [SerializeField] private Response<UserBalanceResponse> _userBalanceResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<Response<UserBalanceResponse>> GetUserBalance()
        {
            _userBalanceResponse = new Response<UserBalanceResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserBalanceResponse>");
            var getTask = _networkClient.GetAsync<UserBalanceResponse>("v1/user/balance", null);

            var result = await getTask;
            _userBalanceResponse = result;

            return result;
        }
        
        [SerializeField] private Response<UserLocationsResponse> _userLocationsResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<Response<UserLocationsResponse>> GetUserLocations()
        {
            _userLocationsResponse = new Response<UserLocationsResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserLocationsResponse>");
            var getTask = _networkClient.GetAsync<UserLocationsResponse>("v1/user/locations", null);

            var result = await getTask;
            _userLocationsResponse = result;

            return result;
        }
        
        [SerializeField] private Response<ShopPacksResponse> _shopPacksResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<Response<ShopPacksResponse>> GetShopPacksAll()
        {
            _shopPacksResponse = new Response<ShopPacksResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<UserLocationsResponse>");
            var getTask = _networkClient.GetAsync<ShopPacksResponse>("v1/market/pack/rate/all", null);

            var result = await getTask;
            _shopPacksResponse = result;

            return result;
        }
        
        [SerializeField] private Response<ShopResourcesResponse> _shopResourcesResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<Response<ShopResourcesResponse>> GetShopResourcesAll()
        {
            _shopResourcesResponse = new Response<ShopResourcesResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<ShopResourcesResponse>");
            var getTask = _networkClient.GetAsync<ShopResourcesResponse>("v1/market/rate/all", null);

            var result = await getTask;
            _shopResourcesResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _paymentTransactionResponse;
        [ContextMenu("GetPaymentTransaction")]
        public async UniTask<Response<PaymentTransactionResponse>> GetPaymentTransaction(string txId)
        {
            _paymentTransactionResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log("_networkClient.GetAsync<PaymentTransactionResponse>");
            var getTask = _networkClient.GetAsync<PaymentTransactionResponse>($"v1/market/tx/payment?id={txId}", null);

            var result = await getTask;
            _paymentTransactionResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _buyMarketPackResponse;
        [ContextMenu("PostBuyMarketPack")]
        public async UniTask<Response<PaymentTransactionResponse>> PostBuyMarketPack(BuyMarketPackPostBody body)
        {
            _buyMarketPackResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<PaymentTransactionResponse> {body.packId}");
            var postTask = _networkClient.PostAsync<PaymentTransactionResponse>("v1/market/buy/pack", body);

            var result = await postTask;
            _buyMarketPackResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _buyMarketEnergyResponse;
        [ContextMenu("PostBuyMarketEnergy")]
        public async UniTask<Response<PaymentTransactionResponse>> PostBuyMarketEnergy()
        {
            _buyMarketEnergyResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<PaymentTransactionResponse>");
            var postTask = _networkClient.PostAsync<PaymentTransactionResponse>("v1/market/buy/energy", null);

            var result = await postTask;
            _buyMarketEnergyResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _buyMarketBlueprintsResponse;
        [ContextMenu("PostBuyMarketBlueprints")]
        public async UniTask<Response<PaymentTransactionResponse>> PostBuyMarketBlueprints()
        {
            _buyMarketBlueprintsResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<PaymentTransactionResponse>");
            var postTask = _networkClient.PostAsync<PaymentTransactionResponse>("v1/market/buy/blueprints", null);

            var result = await postTask;
            _buyMarketBlueprintsResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _buyMarketKeysResponse;
        [ContextMenu("PostBuyMarketKeys")]
        public async UniTask<Response<PaymentTransactionResponse>> PostBuyMarketKeys(RarityName rarityName)
        {
            _buyMarketKeysResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<PaymentTransactionResponse>");
            var postTask = _networkClient.PostAsync<PaymentTransactionResponse>($"v1/market/buy/keys?rarity={rarityName.ToString().ToUpperInvariant()}", null);

            var result = await postTask;
            _buyMarketKeysResponse = result;

            return result;
        }
        
        [SerializeField] private Response<PaymentTransactionResponse> _buyMarketItemResponse;
        [ContextMenu("PostBuyMarketItem")]
        public async UniTask<Response<PaymentTransactionResponse>> PostBuyMarketItem(BuyMarketItemPostBody body)
        {
            _buyMarketItemResponse = new Response<PaymentTransactionResponse>();
            Common.Utilities.Logger.Log($"_networkClient.PostAsync<PaymentTransactionResponse> {body.itemDetailId}");
            var postTask = _networkClient.PostAsync<PaymentTransactionResponse>($"v1/market/buy/item", body);

            var result = await postTask;
            _buyMarketItemResponse = result;

            return result;
        }
    }
}
