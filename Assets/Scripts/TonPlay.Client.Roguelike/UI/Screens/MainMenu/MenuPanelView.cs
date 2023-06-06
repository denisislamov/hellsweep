using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

public class MenuPanelView : View, IMenuPanelView
{
    [SerializeField] 
    private ButtonView _myBagButtonView;
		
    [SerializeField]
    private ButtonView _achievementsButtonView;
		
    [SerializeField] 
    private ButtonView _settingsButtonView;

    public IButtonView MyBagButtonView => _myBagButtonView;
    public IButtonView AchievementsButtonView => _achievementsButtonView;
    public IButtonView SettingsButtonView => _settingsButtonView;
    
    public void Toggle()
    {
	    gameObject.SetActive(!gameObject.activeSelf);
    }
}
