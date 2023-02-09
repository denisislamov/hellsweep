using UnityEngine;

namespace TonPlay.Client.Roguelike.Signals
{
	public class AppliedDamageSignal
	{
		public float Damage { get; }
		
		public Vector2 Position { get; }
		
		public AppliedDamageSignal(float damage, Vector2 position)
		{
			Damage = damage;
			Position = position;
		}
	}
}