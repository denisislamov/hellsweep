using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views
{
	public class SkillChoiceView : View, ISkillChoiceView
	{
		[SerializeField]
		private SkillChoiceCollectionView _collectionView;

		[SerializeField]
		private LevelProgressBarView _levelProgressBarView;

		[SerializeField]
		private SkillChoiceCurrentSkillView[] _defenceCurrentSkillViews;

		[SerializeField]
		private SkillChoiceCurrentSkillView[] _utilityCurrentSkillViews;

		public ISkillChoiceCollectionView CollectionView => _collectionView;

		public ILevelProgressBarView LevelProgressBarView => _levelProgressBarView;

		public ISkillChoiceCurrentSkillView[] DefenceCurrentSkillViews => _defenceCurrentSkillViews;

		public ISkillChoiceCurrentSkillView[] UtilityCurrentSkillViews => _utilityCurrentSkillViews;
	}
}