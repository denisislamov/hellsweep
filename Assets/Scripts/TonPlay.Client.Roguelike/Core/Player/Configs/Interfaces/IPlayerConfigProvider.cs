using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfigProvider
	{
		IPlayerConfig Get(string id = default);
	}
}