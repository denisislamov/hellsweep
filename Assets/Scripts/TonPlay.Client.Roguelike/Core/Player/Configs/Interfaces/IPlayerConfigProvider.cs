namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfigProvider
	{
		IPlayerConfig Get(string id = default);
	}
}