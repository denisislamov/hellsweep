using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class SpawnEnemyDeathEffectSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			var mainWorld = systems.GetWorld();
			var effectsWorld = systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME);

			var filter = mainWorld.Filter<EnemyComponent>()
								  .Inc<DeadComponent>()
								  .End();

			var enemyPool = mainWorld.GetPool<EnemyComponent>();
			var positionPool = mainWorld.GetPool<PositionComponent>();
			
			foreach (var entityIdx in filter)
			{
				ref var enemy = ref enemyPool.Get(entityIdx);
				var position = positionPool.Get(entityIdx);
				
				var enemyConfig = sharedData.EnemyConfigProvider.Get(enemy.ConfigId);
				
				if (enemyConfig.DeathEffectConfig == null || 
					!sharedData.CompositeViewPool.TryGet<EffectView>(enemyConfig.DeathEffectConfig.Identity, out var viewPoolObject))
				{
					return;
				}

				var view = viewPoolObject.Object;
				var entity = effectsWorld.NewEntity();

				view.transform.position = position.Position;

				entity.AddEffectComponent();
				entity.AddPositionComponent(position.Position);
				entity.AddDestroyOnTimerComponent(enemyConfig.DeathEffectConfig.DestroyTimer);
				entity.AddPoolObjectComponent(viewPoolObject);
				
				if (view.PlayableDirector != null)
				{
					view.PlayableDirector.Stop();
					view.PlayableDirector.Play();
					entity.AddPlayableDirectorComponent(view.PlayableDirector);
				}
			}
		}
	}
}