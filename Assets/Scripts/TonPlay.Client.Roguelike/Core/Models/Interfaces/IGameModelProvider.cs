using TonPlay.Client.Roguelike.Core.Models.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Models.Interfaces
{
	public interface IGameModelProvider
	{
		IGameModel Get();
	}
}