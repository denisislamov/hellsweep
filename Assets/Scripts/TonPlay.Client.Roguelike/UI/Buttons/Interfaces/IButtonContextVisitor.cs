namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IButtonContextVisitor
	{
		void Visit(IClickableButtonContext context);

		void Visit(IReactiveTextButtonContext context);

		void Visit(ITextButtonContext context);
	}
}