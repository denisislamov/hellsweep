using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IItemRewardModel : IModel<ItemRewardData>
	{
		IReadOnlyReactiveProperty<ushort> Count { get; }
		IReadOnlyReactiveProperty<string> Id { get; }
	}
}