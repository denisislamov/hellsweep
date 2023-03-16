using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Match
{
	public class MatchLauncher : IMatchLauncher
	{
		private readonly IMatchProviderSetup _matchProviderSetup;
		// private readonly OfflineSingleMatch.Factory _offlineSingleMatchFactory;
		private readonly SingleMatch.Factory _singleMatchFactory;
		
		public MatchLauncher(
			IMatchProviderSetup matchProviderSetup,
			SingleMatch.Factory singleMatchFactory)
			//OfflineSingleMatch.Factory offlineSingleMatchFactory)
		{
			_matchProviderSetup = matchProviderSetup;
			// _offlineSingleMatchFactory = offlineSingleMatchFactory;
			_singleMatchFactory = singleMatchFactory;
		}

		public async UniTask Launch(MatchType matchType, ILocationConfig locationConfig)
		{
			switch (matchType)
			{
				case MatchType.OfflineSingle:
				{
					var match = _singleMatchFactory.Create(locationConfig);
					
					_matchProviderSetup.Setup(match);

					await match.Launch();
					return;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}