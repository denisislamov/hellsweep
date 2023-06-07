using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Samples.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Samples
{
    public class SimplePopupPresenter : Presenter<ISimplePopupView, ISimplePopupScreenContext>
    {
        private readonly IButtonPresenterFactory _buttonPresenterFactory;
        private readonly IUIService _uiService;
        
        public SimplePopupPresenter(
            ISimplePopupView view, 
            ISimplePopupScreenContext context,
            IButtonPresenterFactory buttonPresenterFactory,
            IUIService uiService) : base(view, context)
        {
            _buttonPresenterFactory = buttonPresenterFactory;
            _uiService = uiService;
            
            AddButtonsPresenter();
        }
        
        private void AddButtonsPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.CloseButton,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(CloseButtonClickHandler)));

            Presenters.Add(presenter);
        }
        
        private void CloseButtonClickHandler()
        {
            _uiService.Close(Context.Screen);
        }

        internal class Factory : PlaceholderFactory<ISimplePopupView, ISimplePopupScreenContext, SimplePopupPresenter>
        {
            
        }
    }
}