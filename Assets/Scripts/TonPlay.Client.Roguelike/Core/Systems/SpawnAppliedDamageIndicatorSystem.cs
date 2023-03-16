using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SpawnAppliedDamageIndicatorSystem : IEcsRunSystem, IEcsInitSystem
	{
		private const float FADE_OUT_TIME = 0.75f;
		private const float MOVE_SPEED = 1f;
		private readonly Vector2 SPAWN_OFFSET = new Vector2(0, 0.25f);

		private ISharedData _sharedData;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_poolIdentity = new DamageTextViewPoolIdentity(_sharedData.DamageTextViewPrefab);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _sharedData.DamageTextViewPrefab, 256);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<SpawnAppliedDamageIndicatorEvent>().Inc<PositionComponent>().End();

			var spawnPool = world.GetPool<SpawnAppliedDamageIndicatorEvent>();
			var positionPool = world.GetPool<PositionComponent>();
			var indicatorPool = world.GetPool<AppliedDamageIndicator>();
			var destroyOnTimerPool = world.GetPool<DestroyOnTimerComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var speedPool = world.GetPool<SpeedComponent>();
			var poolObjectPool = world.GetPool<ViewPoolObjectComponent>();
			var transformPool = world.GetPool<TransformComponent>();

			foreach (var entityId in filter)
			{
				ref var spawnEvent = ref spawnPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (_pool.TryGet<DamageTextView>(_poolIdentity, out var poolObject))
				{
					position.Position += SPAWN_OFFSET + Random.insideUnitCircle;

					var view = poolObject.Object;
					view.Position = position.Position;
					view.Color = Color.white;
					view.SetText(Mathf.RoundToInt(spawnEvent.Damage));

					ref var indicator = ref indicatorPool.Add(entityId);
					indicator.View = view;
					indicator.FadeOutColor = new Color32(255, 255, 255, 0);
					indicator.FadeOutTime = FADE_OUT_TIME;

					ref var destroyOnTimer = ref destroyOnTimerPool.Add(entityId);
					destroyOnTimer.TimeLeft = indicator.FadeOutTime;

					ref var movement = ref movementPool.Add(entityId);
					movement.Direction = Vector2.up;

					ref var speed = ref speedPool.Add(entityId);
					speed.Speed = MOVE_SPEED;

					ref var poolObjectComponent = ref poolObjectPool.Add(entityId);
					poolObjectComponent.ViewPoolObject = poolObject;

					ref var transformComponent = ref transformPool.Add(entityId);
					transformComponent.Transform = view.transform;
				}

				spawnPool.Del(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();;
		}
	}
}