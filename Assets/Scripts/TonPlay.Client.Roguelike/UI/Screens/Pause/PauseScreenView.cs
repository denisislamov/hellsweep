using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	public class PauseScreenView : View, IPauseScreenView
	{
		[SerializeField] 
		private ButtonView _quitButtonView;
		
		[SerializeField] 
		private ButtonView _continueButtonView;
		
		[SerializeField] 
		private PauseSkillItemView[] _attackSkillItemViews;
		
		[SerializeField] 
		private PauseSkillItemView[] _defenceSkillItemViews;

		public IButtonView QuitButtonView => _quitButtonView;
		
		public IButtonView ContinueButtonView => _continueButtonView;
		
		public IPauseSkillItemView[] AttackSkillItemViews => _attackSkillItemViews;
		
		public IPauseSkillItemView[] DefenceSkillItemViews => _defenceSkillItemViews;
	}
}