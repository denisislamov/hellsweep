using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Interfaces
{
    public interface IAnalyticsServiceWrapper
    {
        UniTask Init();

        public void OnSingleMatchFinishSession(int coins);

        public void OnFirstAppLaunch();

        public void OnAppLaunch(string playerIdPlatform, string date, string version, string os, string deviceModel,
                                string country, string sizeScreen, string internet);

        public void OnStartChapter(string idLocation, string balanceCoins);

        public void OnCompleteChapter(string idLocation, string balanceCoins);
    }
}
