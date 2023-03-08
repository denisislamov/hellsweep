using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Common.Network
{
    public class RestApiWrapper : MonoBehaviour
    {
        [SerializeField] private bool _useDebugToken;
        [SerializeField] private string _username;

        [Space(5)]
        [SerializeField] private DebugTokenSettings _debugTokenSettings;
        private string _token;
        private NetworkClient _networkClient;
        [SerializeField]
        private NetworkSettings _networkSettings;
        private Dictionary<string, string> _headers;

        private void Start()
        {
            if (_useDebugToken)
            {
                _token = UriParser.Parse(_debugTokenSettings.GetFullDebugAppUrl(_username))["token"];
            }
            else 
            {
                _token = UriParser.Parse(Application.absoluteURL)["token"];
            }

            Debug.LogFormat("_token {0}", _token);
            
            _networkClient = new NetworkClient(_networkSettings.BaseAddress, new System.TimeSpan(0, 0, 10));
            _headers = new Dictionary<string, string>()
            {
                {"Content-Type", "application/json"},
                {"Authorization", "Bearer " + _token},
            };
        }

        // # Skill - Skill Controller
        [Space(10)]
        [Header("--------- Skill ---------")]
        [SerializeField] private SkillAllResponse _skillAllResponse;
        [ContextMenu("GetSkillAll")]
        public async UniTask<SkillAllResponse> GetSkillAll()
        {
            _skillAllResponse = new SkillAllResponse();
            var getTask = _networkClient.GetAsync<SkillAllResponse>("v1/skill/all", _headers, _skillAllResponse);

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
            var getTask = _networkClient.GetAsync<BoostAllResponse>("v1/boost/all", _headers, _boostAllResponse);

            var result = await getTask;
            _boostAllResponse = result;

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
            var getTask = _networkClient.GetAsync<InfoLevelAllResponse>("v1/info/level/all", _headers, _infoLevelAllResponse);

            var result = await getTask;
            _infoLevelAllResponse = result;

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
            var getTask = _networkClient.GetAsync<GameSessionResponse>("v1/game/session", _headers, _gameSessionResponse);

            var result = await getTask;
            _gameSessionResponse = result;

            return result;
        }

        // # User - User Controller
        [Space(10)]
        [Header("--------- User ---------")]
       
        [SerializeField] private UserXpResponse _userXpModel;
        [ContextMenu("GetUserXp")]
        public async UniTask<UserXpResponse> GetUserXp()
        {
            _userXpModel = new UserXpResponse();
            var getTask = _networkClient.GetAsync<UserXpResponse>("v1/user/xp", _headers, _userXpModel);

            var result = await getTask;
            _userXpModel = result;

            return result;
        }

        [SerializeField] private UserSummaryResponse _userSummaryModel;
        [ContextMenu("GetUserSummary")]
        public async UniTask<UserSummaryResponse> GetUserSummary()
        {
            _userSummaryModel = new UserSummaryResponse();
            var getTask = _networkClient.GetAsync<UserSummaryResponse>("v1/user/summary", _headers, _userSummaryModel);

            var result = await getTask;
            _userSummaryModel = result;
            
            return result;
        }

        [SerializeField] private UserItemsResponse _userItemsModel;
        [ContextMenu("GetUserItems")]
        public async UniTask<UserItemsResponse> GetUserItems()
        {
            _userItemsModel = new UserItemsResponse();
            var getTask = _networkClient.GetAsync<UserItemsResponse>("v1/user/items", _headers, _userItemsModel);

            var result = await getTask;
            _userItemsModel = result;
            
            return result;
        }

        [SerializeField] private UserBalanceResponse _userBalanceModel;
        [ContextMenu("GetUserBalance")]
        public async UniTask<UserBalanceResponse> GetUserBalance()
        {
            _userBalanceModel = new UserBalanceResponse();
            var getTask = _networkClient.GetAsync<UserBalanceResponse>("v1/user/balance", _headers, _userBalanceModel);

            var result = await getTask;
            _userBalanceModel = result;

            return result;
        }
    }
}
