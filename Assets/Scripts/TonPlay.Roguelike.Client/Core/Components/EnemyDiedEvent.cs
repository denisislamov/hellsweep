using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct EnemyDiedEvent
	{
		public Vector3 Position;
		public IEnemyConfig EnemyConfig;
	}
}