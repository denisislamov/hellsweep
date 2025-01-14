using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Core.Match.Interfaces
{
	public interface IMatch
	{
		UniTask<bool> Launch();

		UniTask<GameSessionResponse> FinishSession(IMatchResult matchResult);
		
		UniTask Finish();
	}
}