namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface ICollectionView<in TItemView> : IView where TItemView : ICollectionItemView
	{
		void AddContent(TItemView item);

		void RebuildLayout();
	}
}