using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardData : IRewardData
	{
		public string Id { get; }
		public int Count { get; }
		
		public RewardData(string id, int count)
		{
			Id = id;
			Count = count;
		}
	}
}