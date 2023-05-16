using TonPlay.Roguelike.Client.Utilities;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TonPlay.Client.Common.Network
{
    [CreateAssetMenu(fileName = nameof(DebugTokenSettings), menuName = AssetMenuConstants.NETWORK_SERVICE_INSTALLERS + nameof(DebugTokenSettings))]
    public class DebugTokenSettings : ScriptableObject
    {
        [SerializeField] private string _serverUrl = "http://api.hellsweep.xyz/v1/";
        [SerializeField] private List<TokenWithNameModel> _tokensWithName;
        [SerializeField] private List<Query> _queries;
        
        public string GetFullDebugAppUrl(string name)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_serverUrl);
            stringBuilder.Append("?token=" + GetTokenByName(name));
            
            if (_queries != null)
            {
                foreach (var query in _queries)
                {
                    stringBuilder.Append($"&{query.Key}={query.Value}");
                }   
            }
            
            var result = stringBuilder.ToString();
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
        
        [System.Serializable]
        private class Query
        {
            public string Key;
            public string Value;
        }
    }
}