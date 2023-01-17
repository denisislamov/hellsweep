using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(HealthCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(HealthCollectableConfig))]
	public class HealthCollectableConfig : CollectableConfig
	{
		public override CollectableType Type => CollectableType.Health;
	}
}