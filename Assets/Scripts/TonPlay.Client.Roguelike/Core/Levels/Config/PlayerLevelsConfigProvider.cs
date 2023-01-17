using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Levels.Config
{
	[CreateAssetMenu(fileName = nameof(PlayerLevelsConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(PlayerLevelsConfigProvider))]
	public class PlayerLevelsConfigProvider : ScriptableObject, IPlayersLevelsConfigProvider
	{
		[SerializeField]
		private PlayerLevelConfig[] _levelConfigs;

		public IPlayerLevelConfig Get(int level)
		{
			return level < _levelConfigs.Length 
				? _levelConfigs[level] 
				: null;
		}
	}
}