using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(ProfileExperienceCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(ProfileExperienceCollectableConfig))]
	public class ProfileExperienceCollectableConfig : CollectableConfig
	{
		public override CollectableType Type => CollectableType.ProfileExperience;
	}
}