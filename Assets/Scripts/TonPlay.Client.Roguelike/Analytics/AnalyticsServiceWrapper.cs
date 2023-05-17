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
                { "coins", coins }
            };
            
            AnalyticsService.Instance.CustomData("single_match_finish_session", parameters);
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// First time application launch
        /// </summary>
        public void OnFirstAppLaunch()
        {
            var parameters = new Dictionary<string, object>();
            AnalyticsService.Instance.CustomData("first_app_launch", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('first_app_launch', parameters); {0}", 
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// Application launch
        /// </summary>
        /// <param name="playerIdPlatform">player id</param>
        /// <param name="date">date and time in format dd/mm/yy and hh/mm/ss</param>
        /// <param name="version">application version</param>
        /// <param name="os">iOS/Android operating system</param>
        /// <param name="deviceModel">device model Galaxy S21 / iphone 12</param>
        /// <param name="country">country code GS/UA/US</param>
        /// <param name="sizeScreen">device aspect ratio 18:9/16:9, etc.</param>
        /// <param name="internet">Internet source mobile / Wi-Fi</param>
        public void OnAppLaunch(string playerIdPlatform, 
                                string date,
                                string version,
                                string os,
                                string deviceModel,
                                string country,
                                string sizeScreen, 
                                string internet)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "player_id_platform", playerIdPlatform },
                { "date", date },
                { "version", version },
                { "os", os },
                { "device_model", deviceModel },
                { "country", country },
                { "size_screen", sizeScreen },
                { "internet", internet },
            };
            
            AnalyticsService.Instance.CustomData("app_launch", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('app_launch', parameters); {0}", 
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// Start game at location
        /// </summary>
        /// <param name="idLocation">id location</param>
        /// <param name="balanceCoins">coins balance</param>
        public void OnStartChapter(string idLocation, string balanceCoins)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "id_location", idLocation },
                { "balance_coins", balanceCoins }
            };
            
            AnalyticsService.Instance.CustomData("start_chapter", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('start _chapter', parameters); {0}", 
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// Location completed
        /// </summary>
        /// <param name="idLocation">id location</param>
        /// <param name="balanceCoins">coins balance</param>
        public void OnCompleteChapter(string idLocation, string  balanceCoins)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "id_location", idLocation },
                { "balance_coins", balanceCoins }
            };
            
            AnalyticsService.Instance.CustomData("complete_chapter", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('complete_chapter', parameters); {0}", 
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
    }
}
