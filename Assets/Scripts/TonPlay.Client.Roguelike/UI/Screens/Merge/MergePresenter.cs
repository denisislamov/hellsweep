using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    public class MergePresenter : Presenter<IMergeView, IMergeScreenContext>
    {
        private readonly IUIService _uiService;
        private readonly IButtonPresenterFactory _buttonPresenterFactory;
        
        public MergePresenter(IMergeView view, IMergeScreenContext context,
            IUIService uiService,
            IButtonPresenterFactory buttonPresenterFactory) : base(view, context)
        {
            _uiService = uiService;
            _buttonPresenterFactory = buttonPresenterFactory;

            AddMergeButtonPresenter();
        }

        public override void Show()
        {
            base.Show();
            
            View.Show();
        }

        public override void Hide()
        {
            base.Hide();
            
            View.Hide();
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }

        private void InitView()
        {
        }
        
        private void AddMergeButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.MergeButtonView,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(Hide)));

            Presenters.Add(presenter);
        }
        
        internal class Factory : PlaceholderFactory<IMergeView, IMergeScreenContext, MergePresenter>
        {

        }
    }
}