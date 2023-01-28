namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IButtonPresenterFactory
	{
		IButtonPresenter Create(IButtonView view, ICompositeButtonContext context);
	}
}