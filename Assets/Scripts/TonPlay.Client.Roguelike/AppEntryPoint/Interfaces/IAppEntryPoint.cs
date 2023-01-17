using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.AppEntryPoint.Interfaces
{
	public interface IAppEntryPoint
	{
		UniTask ProcessEntrance();

		UniTask ProcessReboot();
	}
}