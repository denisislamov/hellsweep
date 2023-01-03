using Cysharp.Threading.Tasks;

namespace TonPlay.Roguelike.Client.Bootstrap.Interfaces
{
	public interface IBootstrapService
	{
		UniTask Bootstrap();
	}
}