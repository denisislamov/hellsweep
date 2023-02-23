using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces
{
	public interface ISkillChoiceView : IView
	{
		ISkillChoiceCollectionView CollectionView { get; }

		ILevelProgressBarView LevelProgressBarView { get; }

		ISkillChoiceCurrentSkillView[] DefenceCurrentSkillViews { get; }

		ISkillChoiceCurrentSkillView[] UtilityCurrentSkillViews { get; }
	}
}