using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService.Utilities
{
	public interface ICollectionItemPool<TItem> : IMemoryPool<TItem>
		where TItem : ICollectionItemView
	{
	}
}