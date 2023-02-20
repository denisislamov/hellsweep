using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(GoldCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(GoldCollectableConfig))]
	public class GoldCollectableConfig : CollectableConfig
	{
		public override CollectableType Type => CollectableType.Gold;
	}
}