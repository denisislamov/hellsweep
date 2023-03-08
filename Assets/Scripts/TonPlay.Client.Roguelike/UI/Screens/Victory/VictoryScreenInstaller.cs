using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Screens.Victory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Victory
{
	[CreateAssetMenu(fileName = nameof(VictoryScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(VictoryScreenInstaller))]
	public class VictoryScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private RewardItemView _rewardItemPrefab;
		
		[SerializeField]
		private RewardPresentationProvider _rewardPresentationProvider;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;

		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IVictoryScreenContext, VictoryScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<VictoryScreenContext, VictoryScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<VictoryScreenContext, VictoryScreen>>()
						.To<VictoryScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<IVictoryScreenContext, VictoryScreen>>()
						.To<VictoryScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IVictoryView, IVictoryScreenContext, VictoryPresenter, VictoryPresenter.Factory>();

			var rewardsInstaller = new RewardsInstaller(_rewardItemPrefab, _rewardPresentationProvider, _pooledItemsContainerPrefab);
			subContainer.Inject(rewardsInstaller);
			
			rewardsInstaller.InstallBindings();
		}
	}
}