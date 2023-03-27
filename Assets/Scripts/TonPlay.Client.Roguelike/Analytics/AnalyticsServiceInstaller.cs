using TonPlay.Client.Roguelike.Analytics;
using TonPlay.Client.Roguelike.Interfaces;
using Zenject;

public class AnalyticsServiceInstaller :  MonoInstaller<AnalyticsServiceInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IAnalyticsServiceWrapper>().To<AnalyticsServiceWrapper>().AsSingle().NonLazy();
    }
}
