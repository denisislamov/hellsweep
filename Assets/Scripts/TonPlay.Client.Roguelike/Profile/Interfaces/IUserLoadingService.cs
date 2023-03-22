using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Roguelike.Profile.Interfaces
{
	public interface IUserLoadingService
	{
		public UniTask Load();
	}
}