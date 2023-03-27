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
        [SerializeField] private UnitAllResponse _unitAllResponse;
        [ContextMenu("GetUnitAll")]
        public async UniTask<UnitAllResponse> GetUnitAll()
        {
            _unitAllResponse = new UnitAllResponse();
            Debug.LogFormat("_networkClient.GetAsync<UnitAllResponse>");
            var getTask = _networkClient.GetAsync<UnitAllResponse>("v1/unit/all", _unitAllResponse);

            var result = await getTask;
            _unitAllResponse = result;

            return result;
        }
        
        // # Skill - Skill Controller
        [Space(10)]
        [Header("--------- Skill ---------")]
        [SerializeField] private SkillAllResponse _skillAllResponse;
        [ContextMenu("GetSkillAll")]
        public async UniTask<SkillAllResponse> GetSkillAll()
        {
            _skillAllResponse = new SkillAllResponse();
            Debug.LogFormat("_networkClient.GetAsync<SkillAllResponse>");
            var getTask = _networkClient.GetAsync<SkillAllResponse>("v1/skill/all", _skillAllResponse);

            var result = await getTask;
            _skillAllResponse = result;

            return result;
        }
        
        // # Boost - Boost controller
        [Space(10)]
        [Header("--------- Boost ---------")]
        [SerializeField] private BoostAllResponse _boostAllResponse;
        [ContextMenu("GetBoostAll")]
        public async UniTask<BoostAllResponse> GetBoostAll()
        {
            _boostAllResponse = new BoostAllResponse();
            Debug.LogFormat("_networkClient.GetAsync<BoostAllResponse>");
            var getTask = _networkClient.GetAsync<BoostAllResponse>("v1/boost/all", _boostAllResponse);

            var result = await getTask;
            _boostAllResponse = result;

            return result;
        }
        
        // # Location - Location controller
        [Space(10)]
        [Header("--------- Location ---------")]
        [SerializeField] private LocationAllResponse _locationAllResponse;
        [ContextMenu("GetLocationAll")]
        public async UniTask<LocationAllResponse> GetLocationAll()
        {
            _locationAllResponse = new LocationAllResponse();
            Debug.LogFormat("_networkClient.GetAsync<BoostAllResponse>");
            var getTask = _networkClient.GetAsync<LocationAllResponse>("v1/location/all", _locationAllResponse);

            var result = await getTask;
            _locationAllResponse = result;

            return result;
        }

        // # Info - Info controller
        [Space(10)]
        [Header("--------- Info ---------")]
        [SerializeField] private InfoLevelAllResponse _infoLevelAllResponse;
        [ContextMenu("GetInfoLevelAll")]
        public async UniTask<InfoLevelAllResponse> GetInfoLevelAll()
        {
            _infoLevelAllResponse = new InfoLevelAllResponse();
            Debug.LogFormat("_networkClient.GetAsync<InfoLevelAllResponse>");
            var getTask = _networkClient.GetAsync<InfoLevelAllResponse>("v1/info/level/all", _infoLevelAllResponse);

            var result = await getTask;
            _infoLevelAllResponse = result;

            return result;
        }
        
        // # Item - Item controller
        [Space(10)]
        [Header("--------- Item ---------")]
        [SerializeField] private ItemPutBody _itemPutBody;
        [SerializeField] private ItemPutResponse _itemPutResponse;
        [ContextMenu("PutItem")]
        public async UniTask<ItemPutResponse> PutItem(ItemPutBody value)
        {
            _itemPutResponse = new ItemPutResponse();

            Debug.LogFormat("_networkClient.PutAsync<ItemPutResponse> {0}", value);
            var putTask = _networkClient.PutAsync<ItemPutResponse, ItemPutBody>("v1/item", value);

            var result = await putTask;
            _itemPutResponse = result;

            return result;
        }

        [SerializeField] private ItemsGetResponse _itemsGetResponse;
        [ContextMenu("GetAllItems")]
        public async UniTask<ItemsGetResponse> GetAllItems()
        {
            _itemsGetResponse = new ItemsGetResponse();

            Debug.LogFormat("_networkClient.GetAsync<ItemsGetResponse> {0}", _itemsGetResponse);
            var putTask = _networkClient.GetAsync<ItemsGetResponse>("v1/item/all", _itemsGetResponse);

            var result = await putTask;
            _itemsGetResponse = result;

            return result;
        }

        [ContextMenu("DeleteItem")]
        public async UniTask<string> DeleteItem(string slotId)
        {
            Debug.LogFormat("_networkClient.DeleteAsync<string> {0}", slotId);
            var deleteTask = _networkClient.DeleteAsync<string>("v1/item/" + slotId, "");
            var result = await deleteTask;
            
            return result;
        }

        // # Game - Game controller
        [Space(10)]
        [Header("--------- Game ---------")]
        [SerializeField] private GameSessionResponse _gameSessionResponse;
        [ContextMenu("GetGameSession")]
        public async UniTask<GameSessionResponse> GetGameSession()
        {
            _gameSessionResponse = new GameSessionResponse();
            Debug.LogFormat("_networkClient.GetAsync<GameSessionResponse>");
            var getTask = _networkClient.GetAsync<GameSessionResponse>("v1/game/session", _gameSessionResponse);

            try
            {
                var result = await getTask;
                _gameSessionResponse = result;

                return result;
            }
            catch (Exception e)
            {
                Debug.LogFormat("GetGameSession exception {0}", e);
                _gameSessionResponse = null;
                return null;
            }
        }

        [ContextMenu("PostGameSession - GameSessionPutBody")]
        public async UniTask<GameSessionResponse> PostGameSessionClose(CloseGameSessionPostBody value)
        {
            _gameSessionResponse = new GameSessionResponse();
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
                _gameSessionResponse = null;
                return null;
            }
        }

        [ContextMenu("PostGameSession")]
        public async UniTask<GameSessionResponse> PostGameSession(OpenGameSessionPostBody value)
        {
            _gameSessionResponse = new GameSessionResponse();
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
                _gameSessionResponse = null;
                return null;
            }
        }

        // # User - User Controller
        [Space(10)]
        [Header("--------- User ---------")]
       
        [SerializeField] private UserXpResponse _userXpResponse;
        [ContextMenu("GetUserXp")]
        public async UniTask<UserXpResponse> GetUserXp()
        {
            _userXpResponse = new UserXpResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserXpResponse>");
            var getTask = _networkClient.GetAsync<UserXpResponse>("v1/user/xp", _userXpResponse);

            var result = await getTask;
            _userXpResponse = result;

            return result;
        }

        [SerializeField] private UserSummaryResponse _userSummaryResponse;
        [ContextMenu("GetUserSummary")]
        public async UniTask<UserSummaryResponse> GetUserSummary()
        {
            _userSummaryResponse = new UserSummaryResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserSummaryResponse>");
            var getTask = _networkClient.GetAsync<UserSummaryResponse>("v1/user/summary", _userSummaryResponse);

            var result = await getTask;
            _userSummaryResponse = result;
            
            return result;
        }

        [SerializeField] private UserSlotsResponse _userSlotsResponse;
        [ContextMenu("GetUserSlots")]
        public async UniTask<UserSlotsResponse> GetUserSlots()
        {
            _userSlotsResponse = new UserSlotsResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserSlotsResponse>");
            var getTask = _networkClient.GetAsync<UserSlotsResponse>("/v1/user/slots", _userSlotsResponse);

            var result = await getTask;
            _userSlotsResponse = result;
            
            return result;
        }

        [SerializeField] private UserItemsResponse _userItemsResponse;
        [ContextMenu("GetUserItems")]
        public async UniTask<UserItemsResponse> GetUserItems()
        {
            _userItemsResponse = new UserItemsResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserItemsResponse>");
            var getTask = _networkClient.GetAsync<UserItemsResponse>("v1/user/items", _userItemsResponse);

            var result = await getTask;
            _userItemsResponse = result;
            
            return result;
        }

        [SerializeField] private UserBalanceResponse _userBalanceResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<UserBalanceResponse> GetUserBalance()
        {
            _userBalanceResponse = new UserBalanceResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserBalanceResponse>");
            var getTask = _networkClient.GetAsync<UserBalanceResponse>("v1/user/balance", _userBalanceResponse);

            var result = await getTask;
            _userBalanceResponse = result;

            return result;
        }
        
        [SerializeField] private UserLocationsResponse _userLocationsResponse;
        [ContextMenu("GetUserBalance")]
        public async UniTask<UserLocationsResponse> GetUserLocations()
        {
            _userLocationsResponse = new UserLocationsResponse();
            Debug.LogFormat("_networkClient.GetAsync<UserLocationsResponse>");
            var getTask = _networkClient.GetAsync<UserLocationsResponse>("v1/user/locations", _userLocationsResponse);

            var result = await getTask;
            _userLocationsResponse = result;

            return result;
        }

    }
}
