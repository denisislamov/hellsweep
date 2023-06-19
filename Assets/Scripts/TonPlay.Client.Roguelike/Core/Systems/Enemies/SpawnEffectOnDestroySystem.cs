using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class SpawnEffectOnDestroySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			var mainWorld = systems.GetWorld();
			var effectsWorld = systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME);

			var filter = mainWorld.Filter<SpawnEffectOnDestroyComponent>()
								  .Inc<DestroyComponent>()
								  .Inc<PositionComponent>()
								  .End();

			var effectPool = mainWorld.GetPool<SpawnEffectOnDestroyComponent>();
			var positionPool = mainWorld.GetPool<PositionComponent>();
			
			foreach (var entityIdx in filter)
			{
				ref var effect = ref effectPool.Get(entityIdx);
				var position = positionPool.Get(entityIdx);
				
				if (!sharedData.CompositeViewPool.TryGet<EffectView>(effect.EffectIdentity, out var viewPoolObject))
				{
					return;
				}

				var view = viewPoolObject.Object;
				var entity = effectsWorld.NewEntity();

				view.transform.position = position.Position;
				
				entity.AddPositionComponent(position.Position);
				entity.AddDestroyOnTimerComponent(effect.DestroyTimer);
				entity.AddPoolObjectComponent(viewPoolObject);
				
				if (view.PlayableDirector != null)
				{
					view.PlayableDirector.OptimizedPlay();
					entity.AddPlayableDirectorComponent(view.PlayableDirector);
				}
			}
		}
	}
}