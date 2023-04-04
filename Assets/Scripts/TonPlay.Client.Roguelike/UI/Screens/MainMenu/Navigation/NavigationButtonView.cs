using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationButtonView : View, INavigationButtonView
	{
		[SerializeField]
		private ButtonView _buttonView;
		
		[SerializeField]
		private GameObject[] _activeStateObjects;
		
		[SerializeField]
		private GameObject[] _inactiveStateObjects;

		public IButtonView ButtonView => _buttonView;
		
		public void SetActiveState(bool state)
		{
			for (var i = 0; i < _activeStateObjects.Length; i++)
			{
				_activeStateObjects[i].SetActive(state);
			}
			
			for (var i = 0; i < _inactiveStateObjects.Length; i++)
			{
				_inactiveStateObjects[i].SetActive(!state);
			}
		}
	}
}