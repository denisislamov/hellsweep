using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network;
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
            Debug.LogFormat("_networkClient.GetAsync<UnitAllResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<SkillAllResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<BoostAllResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<BoostAllResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<InfoLevelAllResponse>");
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

            Debug.LogFormat("_networkClient.PutAsync<ItemPutResponse> {0}", value);
            var putTask = _networkClient.PutAsync<ItemPutResponse, ItemPutBody>("v1/item", value);

            var result = await putTask;
            _itemPutResponse = result;

            return result;
        }

        [SerializeField] private Response<ItemsGetResponse> _itemsGetResponse;
        [ContextMenu("GetAllItems")]
        public async UniTask<Response<ItemsGetResponse>> GetAllItems()
        {
            _itemsGetResponse = new Response<ItemsGetResponse>();

            Debug.LogFormat("_networkClient.GetAsync<ItemsGetResponse> {0}", _itemsGetResponse);
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

            Debug.LogFormat("_networkClient.GetAsync<ItemLevelRatesResponse> {0}", _itemLevelRatesResponse);
            var putTask = _networkClient.GetAsync<ItemLevelRatesResponse>("v1/item/lvl/rate/all", null);

            var result = await putTask;
            _itemLevelRatesResponse = result;

            return result;
        }

        [ContextMenu("DeleteItem")]
        public async UniTask<Response<string>> DeleteItem(string slotId)
        {
            Debug.LogFormat("_networkClient.DeleteAsync<string> {0}", slotId);
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
            Debug.LogFormat("_networkClient.GetAsync<GameSessionResponse>");
            var getTask = _networkClient.GetAsync<GameSessionResponse>("v1/game/session", null);

            try
            {
                var result = await getTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameSession exception {0}", e);
                return _gameSessionResponse;
            }
        }

        [ContextMenu("PostGameSession - GameSessionPutBody")]
        public async UniTask<Response<GameSessionResponse>> PostGameSessionClose(CloseGameSessionPostBody value)
        {
            _gameSessionResponse = new Response<GameSessionResponse>();
            Debug.LogFormat("_networkClient.PutAsync<GameSessionResponse> {0}", value);
            var postTask = _networkClient.PostAsync<GameSessionResponse, CloseGameSessionPostBody>("v1/game/session/close", value);

            try
            
            {
                var result = await postTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameSession exception {0}", e);
                return _gameSessionResponse;
            }
        }

        [ContextMenu("PostGameSession")]
        public async UniTask<Response<GameSessionResponse>> PostGameSession(OpenGameSessionPostBody value)
        {
            _gameSessionResponse = new Response<GameSessionResponse>();
            Debug.LogFormat("_networkClient.PostAsync<GameSessionResponse> {0}", value);
            var postTask = _networkClient.PostAsync<GameSessionResponse, OpenGameSessionPostBody>("v1/game/session", value);

            try
            {
                var result = await postTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameSession exception {0}", e);
                return _gameSessionResponse;
            }
        }
        
        [SerializeField] private Response<GamePropertiesResponse> _gamePropertiesResponse;
        [ContextMenu("GetGameProperties")]
        public async UniTask<Response<GamePropertiesResponse>> GetGameProperties()
        {
            _gamePropertiesResponse = new Response<GamePropertiesResponse>(); 
            Debug.LogFormat("_networkClient.GetAsync<GamePropertiesResponse>");
            var getTask = _networkClient.GetAsync<GamePropertiesResponse>("v1/game/properties", null);

            try
            {
                var result = await getTask;
                _gamePropertiesResponse = result;
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameProperties exception {0}", e);
                return _gamePropertiesResponse;
            }
        }

        [ContextMenu("PostGameProperties")]
        public async UniTask<Response<GamePropertiesResponse>> PostGameProperties(GamePropertiesResponse value)
        {
            _gamePropertiesResponse = new Response<GamePropertiesResponse>();
            Debug.LogFormat("_networkClient.PostGameProperties<GamePropertiesResponse> {0}", value);
            
            var postTask = _networkClient.PostAsync<GamePropertiesResponse, GamePropertiesResponse>("v1/game/properties", value);

            try
            {
                var result = await postTask;
                _gamePropertiesResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameSession exception {0}", e);
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
            Debug.LogFormat("_networkClient.GetAsync<UserXpResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<UserSummaryResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<UserSlotsResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<UserInventoryResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<UserItemsResponse>");
            var getTask = _networkClient.GetAsync<UserItemsResponse>("v1/user/item/detail/all", null);

            var result = await getTask;
            _userItemsResponse = result;
            
            return result;
        }

        [SerializeField] private Response<UserBalanceResponse> _userBalanceResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<Response<UserBalanceResponse>> GetUserBalance()
        {
            _userBalanceResponse = new Response<UserBalanceResponse>();
            Debug.LogFormat("_networkClient.GetAsync<UserBalanceResponse>");
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
            Debug.LogFormat("_networkClient.GetAsync<UserLocationsResponse>");
            var getTask = _networkClient.GetAsync<UserLocationsResponse>("v1/user/locations", null);

            var result = await getTask;
            _userLocationsResponse = result;

            return result;
        }
    }
}
