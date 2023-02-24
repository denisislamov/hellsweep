using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct EnemyDiedEvent
	{
		public Vector3 Position;
		public IEnemyConfig EnemyConfig;
	}
}