using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Profile.Interfaces
{
	public interface IProfileLoadingService
	{
		public UniTask Load();
	}
}