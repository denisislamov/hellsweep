using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Interfaces
{
	public interface IConfigsLoadingService
	{
		public UniTask Load();
	}
}