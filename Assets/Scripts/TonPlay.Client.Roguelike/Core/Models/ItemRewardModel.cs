using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class ItemRewardModel : IItemRewardModel
	{
		private ItemRewardData _data = new ItemRewardData();

		private readonly ReactiveProperty<ushort> _count = new ReactiveProperty<ushort>();
		private readonly ReactiveProperty<string> _id = new ReactiveProperty<string>();

		public IReadOnlyReactiveProperty<ushort> Count => _count;
		public IReadOnlyReactiveProperty<string> Id => _id;

		public void Update(ItemRewardData data)
		{
			if (data.Count != _count.Value)
			{
				_count.SetValueAndForceNotify(data.Count);
			}

			if (data.Id != _id.Value)
			{
				_id.SetValueAndForceNotify(data.Id);
			}
		}
		
		public ItemRewardData ToData()
		{
			_data.Count = _count.Value;
			_data.Id = _id.Value;
			return _data;
		}
	}
}