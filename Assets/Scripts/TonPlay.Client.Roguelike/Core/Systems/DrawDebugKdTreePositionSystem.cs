using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DrawDebugKdTreePositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<DrawDebugKdTreePositionComponent>()
							  .Inc<KdTreeElementComponent>()
							  .End();
			var elementPool = world.GetPool<KdTreeElementComponent>();

			foreach (var entityId in filter)
			{
				var element = elementPool.Get(entityId);
				var index = element.Storage.KdTreeEntityIdToPositionIndexMap[entityId];
				var position = element.Storage.KdTree.Points[index];

				DrawCircle(0.5f, position, Color.black);
			}
		}

		private void DrawCircle(float radius, Vector2 position, Color color)
		{
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
			var previousDir = Vector2.right;
			var angle = 360/12;
			for (var i = 0; i < 12; i++)
			{
				var dir = previousDir.Rotate(angle);
				
				Debug.DrawLine(position + previousDir * radius, position + dir * radius, color, Time.deltaTime);
				
				previousDir = dir;
			}
#endif
		}
	}
}