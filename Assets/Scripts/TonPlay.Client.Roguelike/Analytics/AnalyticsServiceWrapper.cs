using System;
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
        private string _consentIdentifier;
        private bool _isOptInConsentRequired;
        private bool _isInitSuccess;
        public async UniTask Init()
        {
            //Debug.Log("Init");
            //try
            //{
            _isInitSuccess = true;
            try
            {
                await UnityServices.InitializeAsync();
                var consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
                if (consentIdentifiers.Count > 0)
                {
                    _consentIdentifier = consentIdentifiers[0];
                    _isOptInConsentRequired = _consentIdentifier == "pipl";
                }

                
            }
            catch (ConsentCheckException e)
            {
                _isInitSuccess = false;
                Debug.LogFormat("Something went wrong when checking the GeoIP, " +
                                "check the e.Reason and handle appropriately\n {0}", e.Message);
                // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately
            }
            
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
            if (!_isInitSuccess)
            {
                return;
            }

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
            if (!_isInitSuccess)
            {
                return;
            }
            
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
            if (!_isInitSuccess)
            {
                return;
            }
            
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
        public void OnStartChapter(string idLocation, long balanceCoinsV2)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "id_location", idLocation },
                { "balance_coins_v2", balanceCoinsV2 }
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
        public void OnCompleteChapter(string idLocation, long balanceCoinsV2)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "id_location", idLocation },
                { "balance_coins_v2", balanceCoinsV2 }
            };

            AnalyticsService.Instance.CustomData("complete_chapter", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('complete_chapter', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// Игрок проиграл на локации и вылетает поп ап DEFEAT + время которое продержался
        /// </summary>
        /// <param name="idLocation">id location</param>
        /// <param name="balanceCoins">balance coins</param>
        /// <param name="time">time</param>
        /// <param name="levelSkills">level skills</param>
        public void OnDefeatChapter(string idLocation, long balanceCoinsV2,
                                    int time, int levelSkills)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "id_location", idLocation },
                { "balance_coins_v2", balanceCoinsV2 },
                { "time", time },
                { "level_skills", levelSkills }
            };
            
            AnalyticsService.Instance.CustomData("defeat_chapter", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('defeat_chapter', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        /// <summary>
        /// Игроку вылетает поп ап левел апа
        /// </summary>
        /// <param name="idLevel"></param>
        /// <param name="idLocation"></param>
        /// <param name="balanceCoinsV2"></param>
        public void OnLevelUpProfile(int idLevel, string idLocation, long balanceCoinsV2)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "id_level", idLevel },
                { "balance_coins_v2", balanceCoinsV2 },
                { "id_location", idLocation }
            };
            
            AnalyticsService.Instance.CustomData("level_up_profile", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('level_up_profile', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnLevelUpItems(string idLevelItems, string rarityItem,
                                   string nameItem, long balanceCoinsV2,
                                   string idLocation, int counts)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                {"id_level_items", idLevelItems },
                {"rarity_item", rarityItem },
                {"name_item", nameItem },
                {"balance_coins_v2", balanceCoinsV2 },
                {"id_location", idLocation },
                {"counts", counts }
            };
            
            AnalyticsService.Instance.CustomData("level_up_items", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('level_up_items', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnMergeItems(string idLevelItems, string rarityItem,
            string nameItem, long balanceCoinsV2,
            string idLocation, int counts)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                {"id_level_items", idLevelItems },
                {"rarity_item", rarityItem },
                {"name_item", nameItem },
                {"balance_coins_v2", balanceCoinsV2 },
                {"id_location", idLocation },
                {"counts", counts }
            };
            
            AnalyticsService.Instance.CustomData("merge_items", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('merge_items', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnOpenLootbox(string idRarityLootbox, string idItems,
            long balanceCoinsV2, int levelPlayer,
            string idLocation, int counts)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                {"id_rarity_lootbox", idRarityLootbox },
                {"id_items", idItems },
                {"balance_coins_v2", balanceCoinsV2 },
                {"id_location", idLocation },
                {"level_player", levelPlayer },
                {"counts", counts }
            };
            
            AnalyticsService.Instance.CustomData("open_lootbox", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('open_lootbox', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnTonPurchase(string idPurchaseItems,
            long balanceCoinsV2, int levelPlayer,
            string idLocation, int counts)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                {"id_purchase_items", idPurchaseItems },
                {"level_player", levelPlayer },
                {"balance_coins_v2", balanceCoinsV2 },
                {"id_location", idLocation },
                {"level_player", levelPlayer },
                {"counts", counts }
            };
            
            AnalyticsService.Instance.CustomData("ton_purchase", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('ton_purchase', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnCompleteQuests(string idQuests,
            long balanceCoinsV2, string rewardPoints,
            string idLocation, int counts)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                {"id_quests", idQuests },
                {"reward_points", rewardPoints },
                {"balance_coins_v2", balanceCoinsV2 },
                {"id_location", idLocation },
                {"counts", counts }
            };
            
            AnalyticsService.Instance.CustomData("complete_quests", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('complete_quests', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnReceiveCoins(GoldСhangeSourceTypes source, long quantityCoins)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "source", source.ToString() },
                { "quantity_coins", quantityCoins }
            };

            AnalyticsService.Instance.CustomData("receive_coins", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('receive_coins', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
        
        public void OnSpentCoins(GoldСhangeSourceTypes source, long quantityCoins)
        {
            if (!_isInitSuccess)
            {
                return;
            }
            
            var parameters = new Dictionary<string, object>()
            {
                { "source", source.ToString() },
                { "quantity_coins", quantityCoins }
            };

            AnalyticsService.Instance.CustomData("spent_coins", parameters);
            Debug.LogFormat("AnalyticsService.Instance.CustomData('spent_coins', parameters); {0}",
                string.Join("\n", parameters));
            AnalyticsService.Instance.Flush();
        }
    }
}
