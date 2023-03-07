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

        // # User - User Controller
        [Space(10)]
        [Header("User")]
       
        [SerializeField] private UserXpModel _userXpModel;
        [ContextMenu("GetUserXp")]
        public async UniTask<UserXpModel> GetUserXp()
        {
            _userXpModel = new UserXpModel();
            var getTask = _networkClient.GetAsync<UserXpModel>("v1/user/xp", _headers, _userXpModel);

            var result = await getTask;
            _userXpModel = result;

            return result;
        }

        [SerializeField] private UserSummaryModel _userSummaryModel;
        [ContextMenu("GetUserSummary")]
        public async UniTask<UserSummaryModel> GetUserSummary()
        {
            _userSummaryModel = new UserSummaryModel();
            var getTask = _networkClient.GetAsync<UserSummaryModel>("v1/user/summary", _headers, _userSummaryModel);

            var result = await getTask;
            _userSummaryModel = result;
            
            return result;
        }

        [SerializeField] private UserItemsModel _userItemsModel;
        [ContextMenu("GetUserItems")]
        public async UniTask<UserItemsModel> GetUserItems()
        {
            _userItemsModel = new UserItemsModel();
            var getTask = _networkClient.GetAsync<UserItemsModel>("v1/user/items", _headers, _userItemsModel);

            var result = await getTask;
            _userItemsModel = result;
            
            return result;
        }

        [SerializeField] private UserBalanceModel _userBalanceModel;
        [ContextMenu("GetUserBalance")]
        public async UniTask<UserBalanceModel> GetUserBalance()
        {
            _userBalanceModel = new UserBalanceModel();
            var getTask = _networkClient.GetAsync<UserBalanceModel>("v1/user/balance", _headers, _userBalanceModel);

            var result = await getTask;
            _userBalanceModel = result;

            return result;
        }

        [System.Serializable]
        public class UserItemsModel
        {
            public List<UserItemModel> items;
        }
    }
}
