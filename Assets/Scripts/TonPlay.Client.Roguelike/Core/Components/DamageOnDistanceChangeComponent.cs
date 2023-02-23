using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct DamageOnDistanceChangeComponent
	{
		public IDamageProvider DamageProvider;

		public Vector2 LastDamagePosition;
	}
}