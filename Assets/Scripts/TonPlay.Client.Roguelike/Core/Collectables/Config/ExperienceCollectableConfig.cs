using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(ExperienceCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(ExperienceCollectableConfig))]
	public class ExperienceCollectableConfig : CollectableConfig
	{
		public override CollectableType Type => CollectableType.Experience;
	}
	[CreateAssetMenu(fileName = nameof(ProfileExperienceCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(ProfileExperienceCollectableConfig))]
	public class ProfileExperienceCollectableConfig : CollectableConfig
	{
		public override CollectableType Type => CollectableType.Experience;
	}
}