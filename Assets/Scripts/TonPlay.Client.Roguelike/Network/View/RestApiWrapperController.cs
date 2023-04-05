using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Network.Response;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.Network.View
{
    public class RestApiWrapperController : MonoBehaviour
    {
        [SerializeField] private RestApiWrapper _restApiWrapper;
        [SerializeField] private Text _debugLogView;

        [Space(5)]
        [SerializeField] private InputField _slotIdInputField;
        [SerializeField] private InputField _itemIdInputField;
        
        [SerializeField] private InputField _surviveMillsInputField;
        [SerializeField] private InputField _coinsInputField;
        [Space(5)]
        [SerializeField] private Toggle _gameSessionToggle;
        [SerializeField] private InputField _gameSessionLocationId;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private bool _dirtyFlag;
        private string _logString;

        private void Update() 
        {
            if (_dirtyFlag) 
            {
                _dirtyFlag = true;
                 if (_debugLogView == null) 
                {
                    return;
                }
                _debugLogView.text = _logString;
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            _logString = logString.Replace(",", ",\n").Replace("{", "{\n").Replace("}", "\n}");
            _dirtyFlag = true;
        }
        
        public void GetUnitAll()
        {
            _restApiWrapper.GetUnitAll().Forget();
        }
        
        public void GetSkillAll()
        {
            _restApiWrapper.GetSkillAll().Forget();
        }

        public void GetBoostAll()
        {
            _restApiWrapper.GetBoostAll().Forget();
        }

        public void GetInfoLevelAll()
        {
            _restApiWrapper.GetInfoLevelAll().Forget();
        }

        public void GetLocationAll()
        {
            _restApiWrapper.GetLocationAll().Forget();
        }
        
        public void PutItem() 
        {
            ItemPutBody value = new ItemPutBody() 
            {
                slotId = _slotIdInputField.text,
                itemDetailId = _itemIdInputField.text
            };

            _restApiWrapper.PutItem(value).Forget();
        }

        public void GetAllItems() 
        {
            _restApiWrapper.GetAllItems().Forget();
        }

        public void DeleteItem() 
        {
            _restApiWrapper.DeleteItem(_slotIdInputField.text).Forget();
        }

        public void GetGameSession()
        {
            _restApiWrapper.GetGameSession().Forget();
        }

        public void PostGameSessionClose()
        {
            var value = new CloseGameSessionPostBody() 
            {
                surviveMills = int.Parse(_surviveMillsInputField.text),
                coins = int.Parse(_coinsInputField.text)
            };
            _restApiWrapper.PostGameSessionClose(value).Forget();
        }

        public void PostGameSession()
        {
            _restApiWrapper.PostGameSession(new OpenGameSessionPostBody()
            {
                pve = _gameSessionToggle.isOn,
                locationId = _gameSessionLocationId.text
            }).Forget();
        }
        public void GetUserXp()
        {
            _restApiWrapper.GetUserXp().Forget();
        }

        public void GetUserSummary()
        {
            _restApiWrapper.GetUserSummary().Forget();
        }

        public void GetUserSlots()
        {
            _restApiWrapper.GetUserSlots().Forget();
        }

        public void GetUserItems()
        {
            _restApiWrapper.GetUserItems().Forget();
        }

        public void GetUserBalance()
        {
            _restApiWrapper.GetUserBalance().Forget();
        }
    }
}
