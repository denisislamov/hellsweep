namespace TonPlay.Client.Roguelike.Core.Match.Interfaces
{
	public interface IMatchResult
	{
		MatchResultType MatchResultType { get; }
		
		long Coins { get; }
		
		float ProfileExperience { get; }
	}
}