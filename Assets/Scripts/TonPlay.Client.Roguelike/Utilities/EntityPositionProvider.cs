using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Utilities
{
	public class EntityPositionProvider : IPositionProvider
	{
		private readonly EcsPool<PositionComponent> _positionPool;
		private readonly int _entityId;

		public EntityPositionProvider(EcsPool<PositionComponent> positionPool, int entityId)
		{
			_positionPool = positionPool;
			_entityId = entityId;
		}

		public Vector2 Position => _positionPool.Get(_entityId).Position;
	}
}