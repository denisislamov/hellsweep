using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core
{
	public class SharedData : ISharedData
	{
		public IPlayerSpawnConfigProvider PlayerSpawnConfigProvider { get; }

		public IEnemySpawnConfigProvider EnemySpawnConfigProvider { get; }
		
		public IGameModel GameModel { get; }

		public IPositionProvider PlayerPositionProvider { get; private set; }

		public IReadOnlyDictionary<Collider2D, EcsEntity> ColliderToEntityMap => _colliderToEntityMap;

		private readonly Dictionary<Collider2D, EcsEntity> _colliderToEntityMap;

		public SharedData(
			IPlayerSpawnConfigProvider playerSpawnConfigProvider,
			IEnemySpawnConfigProvider enemySpawnConfigProvider,
			IGameModel gameModel)
		{
			PlayerSpawnConfigProvider = playerSpawnConfigProvider;
			EnemySpawnConfigProvider = enemySpawnConfigProvider;
			GameModel = gameModel;

			_colliderToEntityMap = new Dictionary<Collider2D, EcsEntity>();
		}

		public void SetPlayerPositionProvider(IPositionProvider positionProvider)
		{
			PlayerPositionProvider = positionProvider;
		}
		
		public void AddColliderWithEntityToMap(Collider2D collider, EcsEntity entity)
		{
			if (_colliderToEntityMap.ContainsKey(collider)) return;
			
			_colliderToEntityMap.Add(collider, entity);
		}
		
		public void RemoveColliderFromEntityMap(Collider2D collider)
		{
			if (!_colliderToEntityMap.ContainsKey(collider)) return;
			
			_colliderToEntityMap.Remove(collider);
		}
	}
}