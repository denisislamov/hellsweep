using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Interfaces;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Analytics
{
    public class AnalyticsServiceWrapper : IAnalyticsServiceWrapper
    {
        public async UniTask Init()
        {
            //Debug.Log("Init");
            //try
            //{
            await UnityServices.InitializeAsync();
            //Debug.Log("Done");
                // var consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            //}
            // catch (ConsentCheckException e)
            // {
            //     // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            // }
        }
        
        public void OnSingleMatchFinishSession(int coins)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "coins", coins },
            };
            
            AnalyticsService.Instance.CustomData("single_match_finish_session", parameters);
            AnalyticsService.Instance.Flush();
        }
    }
}
