using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
    public interface IMenuPanelView : IView
    {
        IButtonView MyBagButtonView { get; }
        IButtonView AchievementsButtonView { get; }
        IButtonView SettingsButtonView { get; }
        
        void Toggle();
    }
}
