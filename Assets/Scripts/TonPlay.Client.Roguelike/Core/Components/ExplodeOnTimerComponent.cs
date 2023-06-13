using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ExplodeOnTimerComponent
	{
		public IDamageProvider DamageProvider;
		public ICollisionAreaConfig CollisionConfig;
		public int CollisionLayerMask;
		public float TimeLeft;
	}
}