using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardsInstaller : Installer
	{
		private readonly RewardItemView _prefab;
		private readonly IRewardPresentationProvider _provider;
		private readonly GameObject _pooledItemsContainerPrefab;

		public RewardsInstaller(RewardItemView prefab, IRewardPresentationProvider provider, GameObject pooledItemsContainerPrefab)
		{
			_prefab = prefab;
			_provider = provider;
			_pooledItemsContainerPrefab = pooledItemsContainerPrefab;
		}

		public override void InstallBindings()
		{
			Bind(Container);
		}
		
		private void Bind(DiContainer container)
		{
			var screenHolder = container.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			container
			   .BindMemoryPoolCustomInterface<RewardItemView, CollectionItemPool<IRewardItemView, RewardItemView>,
					ICollectionItemPool<IRewardItemView>>()
			   .FromComponentInNewPrefab(_prefab)
			   .UnderTransform(pooledItemsContainer.transform);

			container
			   .BindFactory<IRewardItemView, IRewardItemContext, RewardItemPresenter, RewardItemPresenter.Factory>()
			   .FromNew();

			container
			   .BindFactory<IRewardItemCollectionView, IRewardItemCollectionContext, RewardItemCollectionPresenter, RewardItemCollectionPresenter.Factory>()
			   .FromNew();

			container.Bind<IRewardPresentationProvider>().FromInstance(_provider).AsSingle();
		}
	}
}