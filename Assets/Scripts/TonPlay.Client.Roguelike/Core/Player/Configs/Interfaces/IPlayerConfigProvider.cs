namespace TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfigProvider
	{
		IPlayerConfig Get(string id = default);
		ISkinConfig GetSkin(string id = default);
	}
}