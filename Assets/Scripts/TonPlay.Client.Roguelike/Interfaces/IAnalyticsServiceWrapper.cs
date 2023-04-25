using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Interfaces
{
    public interface IAnalyticsServiceWrapper
    {
        UniTask Init();

        public void OnSingleMatchFinishSession(int coins);
    }
}
