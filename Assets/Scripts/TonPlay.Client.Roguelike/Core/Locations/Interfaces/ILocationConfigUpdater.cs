using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationConfigUpdater
	{
		void UpdateByIndex(int index, LocationAllResponse.Location remoteConfig);
	}
}