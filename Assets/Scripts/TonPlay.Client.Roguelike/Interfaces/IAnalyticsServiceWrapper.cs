using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Analytics;

namespace TonPlay.Client.Roguelike.Interfaces
{
    public interface IAnalyticsServiceWrapper
    {
        UniTask Init();

        public void OnSingleMatchFinishSession(int coins);

        public void OnFirstAppLaunch();

        public void OnAppLaunch(string playerIdPlatform, string date, string version, string os, string deviceModel,
                                string country, string sizeScreen, string internet);

        public void OnStartChapter(string idLocation, long balanceCoinsV2);

        public void OnCompleteChapter(string idLocation, long balanceCoinsV2);

        public void OnDefeatChapter(string idLocation, long balanceCoinsV2,
                                    int time, int levelSkills);
        
        public void OnLevelUpProfile(int idLevel, string idLocation, long balanceCoinsV2);

        public void OnLevelUpItems(string idLevelItems, string rarityItem,
            string nameItem, long balanceCoinsV2,
            string idLocation, int counts);

        public void OnMergeItems(string idLevelItems, string rarityItem, string nameItem, 
                                 long balanceCoinsV2, string idLocation, int counts);
        // TODO - ask
        public void OnOpenLootbox(string idRarityLootbox, string idItems, long balanceCoinsV2, 
                                  int levelPlayer, string idLocation, int counts);
        public void OnTonPurchase(string idPurchaseItems, long balanceCoinsV2, int levelPlayer,
                                  string idLocation, int counts);
        // TODO - ask
        public void OnCompleteQuests(string idQuests, long balanceCoinsV2, string rewardPoints,
                                     string idLocation, int counts);
        
        
        public void OnReceiveCoins(GoldСhangeSourceTypes source, long quantityCoins);
        public void OnSpentCoins(GoldСhangeSourceTypes source, long quantityCoins);
        
    }
}
