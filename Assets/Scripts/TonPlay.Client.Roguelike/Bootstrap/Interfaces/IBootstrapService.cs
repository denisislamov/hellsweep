using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Bootstrap.Interfaces
{
	public interface IBootstrapService
	{
		UniTask Bootstrap();
	}
}