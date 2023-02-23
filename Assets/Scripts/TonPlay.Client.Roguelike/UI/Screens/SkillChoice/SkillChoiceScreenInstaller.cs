using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	[CreateAssetMenu(fileName = nameof(SkillChoiceScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(SkillChoiceScreenInstaller))]
	public class SkillChoiceScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private SkillChoiceItemView _itemPrefab;

		[SerializeField]
		private Canvas _pooledItemsContainerPrefab;

		[SerializeField]
		private SkillConfigProvider _skillConfigProvider;

		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<ISkillChoiceScreenContext, SkillChoiceScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<SkillChoiceScreenContext, SkillChoiceScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<ISkillChoiceScreenContext, SkillChoiceScreen>>()
						.To<SkillChoiceScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<SkillChoiceScreenContext, SkillChoiceScreen>>()
						.To<SkillChoiceScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<ISkillChoiceView, ISkillChoiceScreenContext, SkillChoicePresenter, SkillChoicePresenter.Factory>();

			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<SkillChoiceItemView, CollectionItemPool<ISkillChoiceItemView, SkillChoiceItemView>,
					ICollectionItemPool<ISkillChoiceItemView>>()
			   .FromComponentInNewPrefab(_itemPrefab)
			   .UnderTransform(pooledItemsContainer.transform);

			subContainer
			   .BindFactory<ISkillChoiceCollectionView, ISkillChoiceCollectionContext, SkillChoiceCollectionPresenter, SkillChoiceCollectionPresenter.Factory>()
			   .FromNew();

			subContainer
			   .BindFactory<ISkillChoiceItemView, ISkillChoiceItemContext, SkillChoiceItemPresenter, SkillChoiceItemPresenter.Factory>()
			   .FromNew();

			subContainer.Bind<ISkillConfigProvider>().FromInstance(_skillConfigProvider).AsSingle();

			subContainer.BindFactory<ILevelProgressBarView, ILevelProgressBarContext, LevelProgressBarPresenter, LevelProgressBarPresenter.Factory>().FromNew();
			subContainer.BindFactory<IProgressBarView, IProgressBarContext, ProgressBarPresenter, ProgressBarPresenter.Factory>().FromNew();
		}
	}
}