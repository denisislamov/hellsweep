using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	[CreateAssetMenu(fileName = nameof(ButtonsInstaller), menuName = AssetMenuConstants.UI_INSTALLERS + nameof(ButtonsInstaller))]
	public class ButtonsInstaller : Common.UIService.ButtonsInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IButtonPresenterFactory>().To<ButtonPresenterFactory>().AsSingle();

			Container.BindFactory<IButtonView, ButtonPresenterFactoryContextVisitor, ButtonPresenterFactoryContextVisitor.Factory>();

			Container.BindFactory<IButtonView, ICompositeButtonContext, CompositeButtonPresenter, CompositeButtonPresenter.Factory>();

			Container.BindFactory<IButtonView, ITextButtonContext, TextButtonPresenter, TextButtonPresenter.Factory>();
			Container.BindFactory<IButtonView, IClickableButtonContext, ClickableButtonPresenter, ClickableButtonPresenter.Factory>();
			Container.BindFactory<IButtonView, IReactiveTextButtonContext, ReactiveTextButtonPresenter, ReactiveTextButtonPresenter.Factory>();
		}
	}
}