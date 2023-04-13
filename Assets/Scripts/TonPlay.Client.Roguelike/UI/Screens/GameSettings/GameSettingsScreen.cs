using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.GameSettings
{
    public class GameSettingsScreen : Screen<GameSettingsScreenContext>
    {
        [SerializeField] private GameSettingsView _view;
        
        [Inject]
        private void Construct(GameSettingsPresenter.Factory factory)
        {
            var presenter = factory.Create(_view, Context);
            Presenters.Add(presenter);
        }
        
        public class Factory : ScreenFactory<IGameSettingsScreenContext, GameSettingsScreen>
        {
            public Factory(DiContainer container, Screen screenPrefab)
                : base(container, screenPrefab)
            {
            }
        }
    }
}