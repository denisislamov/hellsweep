using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces
{
	public interface ISkillChoiceView : IView
	{
		ISkillChoiceCollectionView CollectionView { get; }
	}
}