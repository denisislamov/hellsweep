using System;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ClickableButtonContext : ButtonContext, IClickableButtonContext
	{
		public Action OnClick { get; }

		public ClickableButtonContext(Action onClick)
		{
			OnClick = onClick;
		}

		public override void Accept(IButtonContextVisitor buttonContextVisitor)
		{
			buttonContextVisitor.Visit(this);
		}
	}
}