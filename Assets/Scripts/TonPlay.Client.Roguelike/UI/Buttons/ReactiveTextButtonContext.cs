using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ReactiveTextButtonContext : ButtonContext, IReactiveTextButtonContext
	{
		public IReadOnlyReactiveProperty<string> Text { get; }

		public ReactiveTextButtonContext(IReadOnlyReactiveProperty<string> text)
		{
			Text = text;
		}

		public override void Accept(IButtonContextVisitor buttonContextVisitor)
		{
			buttonContextVisitor.Visit(this);
		}
	}
}