using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces
{
	public interface IPauseScreenView : IView
	{
		IButtonView QuitButtonView { get; }
		
		IButtonView ContinueButtonView { get; }
		
		IPauseSkillItemView[] AttackSkillItemViews { get; }
		
		IPauseSkillItemView[] DefenceSkillItemViews { get; }
	}
}