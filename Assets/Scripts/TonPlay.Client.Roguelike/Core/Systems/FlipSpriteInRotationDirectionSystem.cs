using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class FlipSpriteInRotationDirectionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<FlipSpriteInRotationDirectionComponent>()
						.Inc<SpriteRenderersComponent>()
						.Inc<RotationComponent>()
						.Exc<DeadComponent>()
						.Exc<DestroyComponent>()
						.End();

			var spriteRendererPool = world.GetPool<SpriteRenderersComponent>();
			var rotationPool = world.GetPool<RotationComponent>();

			foreach (var entityIdx in filter)
			{
				ref var spriteRenderer = ref spriteRendererPool.Get(entityIdx);
				ref var rotation = ref rotationPool.Get(entityIdx);
				
				for (var i = 0; i < spriteRenderer.SpriteRenderers.Length; i++)
				{
					var renderer = spriteRenderer.SpriteRenderers[i];
					
					if (rotation.Direction.x > 0)
					{
						renderer.flipX = false;
					} 
					else if (rotation.Direction.x < 0)
					{
						renderer.flipX = true;
					}
				}
			}
		}
	}
}