using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Locations.Sands;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Locations.Sands;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Locations.Sands
{
	public class RavenSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int RAVEN_COUNT = 20;
		private const float DISTANCE_TO_PLAYER_TO_FLY_AWAY = 2.5f;
		private const float TIMER_TO_DESTROY = 5.1f;
		private const float INFINITE_LOCATION_GENERATION_RADIUS = 100f;

		private readonly RavenView _ravenView;
		private readonly IViewPoolIdentity _viewPoolIdentity;
		private static readonly int s_Flying = Animator.StringToHash("Flying");

		public RavenSystem(RavenView ravenView)
		{
			_ravenView = ravenView;
			_viewPoolIdentity = new GameObjectViewPoolIdentity(ravenView.gameObject);
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			sharedData.CompositeViewPool.Add(_viewPoolIdentity, _ravenView, RAVEN_COUNT);

			for (var i = 0; i < RAVEN_COUNT; i++)
			{
				if (!sharedData.CompositeViewPool.TryGet<RavenView>(_viewPoolIdentity, out var viewPoolObject))
				{
					return;
				}
				
				var view = viewPoolObject.Object;
				var entity = world.NewEntity();

				var position = GeneratePosition(sharedData.LocationSize, sharedData.PlayerPositionProvider.Position);
				var dir = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
				
				view.SelfTransform.position = position;
				view.SelfTransform.localScale = new Vector3(dir, 1f, 1f);

				entity.Add<RavenComponent>();
				entity.AddTransformComponent(view.SelfTransform);
				entity.AddAnimatorComponent(view.Animator);
				entity.AddPositionComponent(position);
				entity.AddPoolObjectComponent(viewPoolObject);
			}
		}
		
		private Vector2 GeneratePosition(Vector2 locationSize, Vector2 playerPosition)
		{
			if (locationSize.x > 1000 || locationSize.y > 1000)
			{
				return playerPosition + Random.insideUnitCircle * INFINITE_LOCATION_GENERATION_RADIUS;
			}

			return new Vector2(
				Random.Range(-locationSize.x * 0.5f, locationSize.x * 0.5f),
				Random.Range(-locationSize.y * 0.5f, locationSize.y * 0.5f));
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var filter = world.Filter<RavenComponent>()
							  .Inc<PositionComponent>()
							  .Inc<TransformComponent>()
							  .Inc<AnimatorComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var ravenPool = world.GetPool<RavenComponent>();
			var destroyPool = world.GetPool<DestroyOnTimerComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();
			var transformPool = world.GetPool<TransformComponent>();
			
			foreach (var entityIdx in filter)
			{
				ref var animator = ref animatorPool.Get(entityIdx);
				ref var position = ref positionPool.Get(entityIdx);
				ref var raven = ref ravenPool.Get(entityIdx);

				var distance = Vector2.Distance(position.Position, sharedData.PlayerPositionProvider.Position);

				if (distance < DISTANCE_TO_PLAYER_TO_FLY_AWAY && !raven.Flying)
				{
					ref var transform = ref transformPool.Get(entityIdx);
					
					raven.Flying = true;
					animator.Animator.SetBool(s_Flying, true);
					
					ref var destroyOnTimerComponent = ref destroyPool.AddOrGet(entityIdx);
					destroyOnTimerComponent.TimeLeft = TIMER_TO_DESTROY;

					var dir = position.Position.x - sharedData.PlayerPositionProvider.Position.x > 0 ? -1f : 1f;
					
					transform.Transform.localScale = new Vector3(dir, 1f, 1f);
				} 
			}
		}
	}
}