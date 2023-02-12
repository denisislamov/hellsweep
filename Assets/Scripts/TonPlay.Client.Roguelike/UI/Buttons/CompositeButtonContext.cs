using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class CompositeButtonContext : ScreenContext, ICompositeButtonContext
	{
		private readonly List<IButtonContext> _contexts = new List<IButtonContext>();
		
		public IReadOnlyList<IButtonContext> ButtonContexts => _contexts;

		public ICompositeButtonContext Add(IButtonContext context)
		{
			_contexts.Add(context);

			return this;
		}
	}
}