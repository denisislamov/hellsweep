using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	[CreateAssetMenu(fileName = nameof(LocationConfigProvider), menuName = AssetMenuConstants.LOCATION_CONFIGS + nameof(LocationConfigProvider))]
	public class LocationConfigProvider : ScriptableObject, ILocationConfigProvider
	{
		[SerializeField]
		private LocationConfig _config;
		
		public ILocationConfig Get() => _config;
	}
}