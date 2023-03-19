using TonPlay.Client.Roguelike.Core.Match.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Match
{
	public class MatchResult : IMatchResult
	{
		public MatchResultType MatchResultType { get; }
		public long Coins { get; }
		public float ProfileExperience { get; }
		
		public MatchResult(MatchResultType matchResultType, long coins, float profileExperience)
		{
			MatchResultType = matchResultType;
			Coins = coins;
			ProfileExperience = profileExperience;
		}
	}
}