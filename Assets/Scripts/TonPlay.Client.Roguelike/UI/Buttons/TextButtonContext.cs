using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class TextButtonContext : ButtonContext, ITextButtonContext
	{
		public string Text { get; }

		public TextButtonContext(string text)
		{
			Text = text;
		}

		public override void Accept(IButtonContextVisitor buttonContextVisitor)
		{
			buttonContextVisitor.Visit(this);
		}
	}
}