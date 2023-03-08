using TonPlay.Client.Roguelike.Core.Match.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Match
{
	public class MatchProvider : IMatchProvider, IMatchProviderSetup
	{
		private IMatch _current;

		public IMatch Current => _current;
		
		public void Setup(IMatch match)
		{
			_current = match;
		}
	}
}