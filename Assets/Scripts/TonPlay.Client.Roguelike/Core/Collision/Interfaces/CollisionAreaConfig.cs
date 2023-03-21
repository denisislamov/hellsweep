using System;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	internal abstract class CollisionAreaConfig : ScriptableObject, ICollisionAreaConfig
	{
		[SerializeField]
		protected Vector2 _position;
		
		[SerializeField]
		protected bool _doNotInitiateCollisionOverlap;
		
		public Vector2 Position => _position;
		
		public bool DoNotInitiateCollisionOverlap => _doNotInitiateCollisionOverlap;

		public abstract CollisionAreaConfig Clone();
	}
}