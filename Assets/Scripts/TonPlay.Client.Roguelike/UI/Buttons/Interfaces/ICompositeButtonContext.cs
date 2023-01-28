using System.Collections.Generic;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface ICompositeButtonContext : IScreenContext
	{
		IReadOnlyList<IButtonContext> ButtonContexts { get; }

		ICompositeButtonContext Add(IButtonContext context);
	}
}