using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ReactiveLockButtonContext : ButtonContext, IReactiveLockButtonContext
	{
		public IReadOnlyReactiveProperty<bool> Locked { get; }

		public ReactiveLockButtonContext(IReadOnlyReactiveProperty<bool> locked)
		{
			Locked = locked;
		}

		public override void Accept(IButtonContextVisitor buttonContextVisitor)
		{
			buttonContextVisitor.Visit(this);
		}
	}
}