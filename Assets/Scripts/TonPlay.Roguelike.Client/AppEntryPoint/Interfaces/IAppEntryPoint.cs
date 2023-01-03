using Cysharp.Threading.Tasks;

namespace TonPlay.Roguelike.Client.AppEntryPoint.Interfaces
{
	public interface IAppEntryPoint
	{
		UniTask ProcessEntrance();

		UniTask ProcessReboot();
	}
}