using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Match.Interfaces
{
	public interface IMatchLauncher
	{
		UniTask Launch(MatchType matchType, ILocationConfig locationConfig);
	}
}