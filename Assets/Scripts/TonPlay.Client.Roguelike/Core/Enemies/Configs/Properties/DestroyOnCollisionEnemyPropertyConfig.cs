using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(DestroyOnCollisionEnemyPropertyConfig), 
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(DestroyOnCollisionEnemyPropertyConfig))]
	public class DestroyOnCollisionEnemyPropertyConfig : EnemyPropertyConfig, IDestroyOnCollisionEnemyPropertyConfig
	{
	}
}