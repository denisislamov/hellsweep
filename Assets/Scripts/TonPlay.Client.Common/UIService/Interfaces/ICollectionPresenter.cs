using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface ICollectionPresenter<TItemView, TContext> : IPresenter<ICollectionView<TItemView>, TContext>
		where TItemView : ICollectionItemView
		where TContext : IScreenContext
	{
	}
}