using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface ICollectionView<in TItemView> : IView
	{
		void AddContent(TItemView item);

		void RebuildLayout();
	}
}