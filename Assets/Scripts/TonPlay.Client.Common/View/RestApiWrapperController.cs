using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Common.Network
{
    public class RestApiWrapperController : MonoBehaviour
    {
        [SerializeField] private RestApiWrapper _restApiWrapper;
        [SerializeField] private Text _debugLogView;

        [Space(5)]
        [SerializeField] private InputField _slotIdInputField;
        [SerializeField] private InputField _itemIdInputField;


        [Space(5)]
        [SerializeField] private InputField _lootedItemsInputField;
        [SerializeField] private InputField _surviveMillsInputField;

        [Space(5)]
        [SerializeField] private Toggle _gameSessionToggle;

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

        public void GetSkillAll()
        {
            _restApiWrapper.GetSkillAll();
        }

        public void GetBoostAll()
        {
            _restApiWrapper.GetBoostAll();
        }

        public void GetInfoLevelAll()
        {
            _restApiWrapper.GetInfoLevelAll();
        }

        public void PutItem() 
        {
            ItemPutBody value = new ItemPutBody() 
            {
                slotId = _slotIdInputField.text,
                itemId = _itemIdInputField.text
            };

            _restApiWrapper.PutItem(value);
        }

        public void GetAllItems() 
        {
            _restApiWrapper.GetAllItems();
        }

        public void DeleteItem() 
        {
            _restApiWrapper.DeleteItem(_slotIdInputField.text);
        }

        public void GetGameSession()
        {
            _restApiWrapper.GetGameSession();
        }

        public void PutGameSession()
        {
            GameSessionPutBody value = new GameSessionPutBody() 
            {
                lootedItems = new List<string>(_lootedItemsInputField.text.Split(',')),
                surviveMills = int.Parse(_surviveMillsInputField.text)
            };
            _restApiWrapper.PutGameSession(value);
        }

        public void PostGameSession()
        {
            _restApiWrapper.PostGameSession(_gameSessionToggle.isOn);
        }
        public void GetUserXp()
        {
            _restApiWrapper.GetUserXp();
        }

        public void GetUserSummary()
        {
            _restApiWrapper.GetUserSummary();
        }

        public void GetUserSlots()
        {
            _restApiWrapper.GetUserSlots();
        }

        public void GetUserItems()
        {
            _restApiWrapper.GetUserItems();
        }

        public void GetUserBalance()
        {
            _restApiWrapper.GetUserBalance();
        }
    }
}
