namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationConfigProvider
	{
		ILocationConfig Get(string id);

		ILocationConfig[] Configs { get; }
	}
}