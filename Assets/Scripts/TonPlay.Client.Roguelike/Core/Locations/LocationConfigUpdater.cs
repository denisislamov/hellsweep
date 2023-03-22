using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	public class LocationConfigUpdater : ILocationConfigUpdater, ILocationConfigUpdaterVisitor
	{
		private readonly ILocationConfigProvider _locationConfigProvider;
		
		private LocationAllResponse.Location _remoteConfig;

		public LocationConfigUpdater(ILocationConfigProvider locationConfigProvider)
		{
			_locationConfigProvider = locationConfigProvider;
		}
		
		public void UpdateByIndex(int index, LocationAllResponse.Location remoteConfig)
		{
			var locationConfig = _locationConfigProvider.Get(index);

			if (locationConfig is null)
			{
				Debug.LogWarning($"Location config with index {index} doesn't exist.");
				return;
			}

			_remoteConfig = remoteConfig;
			
			locationConfig.AcceptUpdater(this);
		}
		
		public void Visit(LocationConfig locationConfig)
		{
			locationConfig.SetId(_remoteConfig.id);
			locationConfig.SetInfinite(_remoteConfig.infinite);
		}
	}
}