using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Core.Match.Interfaces
{
	public interface IMatch
	{
		UniTask Launch();

		UniTask Finish();
	}
}