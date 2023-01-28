using System;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IClickableButtonContext : IButtonContext
	{
		Action OnClick { get; }
	}
}