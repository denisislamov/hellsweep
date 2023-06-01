using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class ProfileBarView : View, IProfileBarView
	{
		[SerializeField]
		private TextMeshProUGUI _levelText;

		[SerializeField]
		private TextMeshProUGUI _energyText;

		[SerializeField]
		private TextMeshProUGUI _goldText;

		[SerializeField]
		private FillImageProgressBarView _experienceProgressBarView;
		
		[SerializeField] 
		private ButtonView _gameSettingsButtonView;

		[SerializeField] 
		private ButtonView _menuPanelButtonView;
		
		public IProgressBarView ExperienceProgressBarView => _experienceProgressBarView;
		public IButtonView GameSettingsButtonView => _gameSettingsButtonView;
		public IButtonView MenuPanelButtonView => _menuPanelButtonView;

		[SerializeField]
		private TextMeshProUGUI _usernameText;
		
		[SerializeField] 
		private MenuPanelView _menuPanelView;
		public IMenuPanelView MenuPanelView => _menuPanelView;
		
		public void SetLevelText(string text)
		{
			_levelText.SetText(text);
		}

		public void SetEnergyText(string text)
		{
			_energyText.SetText(text);
		}

		public void SetGoldText(string text)
		{
			_goldText.SetText(text);
		}

		public void SetUsername(string text)
		{
			_usernameText.SetText(text);
		}
	}
}