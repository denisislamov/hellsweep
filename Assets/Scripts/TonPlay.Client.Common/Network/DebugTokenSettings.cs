using TonPlay.Roguelike.Client.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace TonPlay.Client.Common.Network
{
    [CreateAssetMenu(fileName = nameof(DebugTokenSettings), menuName = AssetMenuConstants.NETWORK_SERVICE_INSTALLERS + nameof(DebugTokenSettings))]
    public class DebugTokenSettings : ScriptableObject
    {
        [SerializeField] private string _serverUrl = "http://api.hellsweep.xyz/v1/";
        [SerializeField] private List<TokenWithNameModel> _tokensWithName;
        
        public string GetFullDebugAppUrl(string name)
        {
            var result = _serverUrl + "?token=" + GetTokenByName(name);
            Debug.LogFormat("GetFullDebugAppUrl : {0}", result);
            return result;
        }

        private string GetTokenByName(string name) 
        {
            foreach (var tokenWithName in _tokensWithName) 
            {
                if (tokenWithName.Name == name) 
                {
                    return tokenWithName.Token;
                }
            }

            return string.Empty;
        }

        [System.Serializable]
        private class TokenWithNameModel
        {
            public string Name;
            public string Token;
        }
    }
}