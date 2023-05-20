using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class EnemiesKdTreeSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int CHECK_DISTANCE_TO_PLAYER = 14;
		private const int SQR_CHECK_DISTANCE_TO_PLAYER = CHECK_DISTANCE_TO_PLAYER*CHECK_DISTANCE_TO_PLAYER;

		private readonly KdTreeStorage _kdTreeStorage;

		public EnemiesKdTreeSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var positionPool = world.GetPool<PositionComponent>();
			var kdTreeElementPool = world.GetPool<KdTreeElementComponent>();

			var sharedData = systems.GetShared<ISharedData>();

			IncludeToKdTreeValidEnemies(world, positionPool, sharedData, kdTreeElementPool);
			ExcludeFromKdTreeInvalidEnemies(world, positionPool, sharedData, kdTreeElementPool);

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private static void ExcludeFromKdTreeInvalidEnemies(EcsWorld world, EcsPool<PositionComponent> positionPool, ISharedData sharedData, EcsPool<KdTreeElementComponent> kdTreeElementPool)
		{

			var enemyFilter = world.Filter<EnemyComponent>()
								   .Inc<PositionComponent>()
								   .Inc<KdTreeElementComponent>()
								   .End();

			foreach (var entityIdx in enemyFilter)
			{
				var enemyPosition = positionPool.Get(entityIdx);
				var dirToPlayer = sharedData.PlayerPositionProvider.Position - enemyPosition.Position;
				var sqrDistanceToPlayer = dirToPlayer.x*dirToPlayer.x + dirToPlayer.y*dirToPlayer.y;

				if (sqrDistanceToPlayer > SQR_CHECK_DISTANCE_TO_PLAYER)
				{
					ref var kdTreeElement = ref kdTreeElementPool.Get(entityIdx);
					kdTreeElement.Storage.RemoveEntity(entityIdx);

					kdTreeElementPool.Del(entityIdx);
				}
			}
		}
		private void IncludeToKdTreeValidEnemies(EcsWorld world, EcsPool<PositionComponent> positionPool, ISharedData sharedData, EcsPool<KdTreeElementComponent> kdTreeElementPool)
		{
			var enemyFilter = world.Filter<EnemyComponent>()
								   .Inc<PositionComponent>()
								   .Exc<KdTreeElementComponent>()
								   .End();

			foreach (var entityIdx in enemyFilter)
			{
				var enemyPosition = positionPool.Get(entityIdx);
				var dirToPlayer = sharedData.PlayerPositionProvider.Position - enemyPosition.Position;
				var sqrDistanceToPlayer = dirToPlayer.x*dirToPlayer.x + dirToPlayer.y*dirToPlayer.y;

				if (sqrDistanceToPlayer <= SQR_CHECK_DISTANCE_TO_PLAYER)
				{
					ref var kdTreeElement = ref kdTreeElementPool.Add(entityIdx);
					kdTreeElement.Storage = _kdTreeStorage;
					kdTreeElement.TreeIndex = _kdTreeStorage.AddEntity(entityIdx, enemyPosition.Position);
				}
			}
		}
	}
}