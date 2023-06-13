using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyCameraShakeSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();

			var playerFilter = world.Filter<PlayerComponent>()
									.Inc<ApplyDamageComponent>()
									.Exc<DeadComponent>()
									.Exc<DestroyComponent>()
									.End();

			var cameraFilter = world.Filter<CameraComponent>().End();
			var shakePool = world.GetPool<CameraShakeComponent>();

			if (playerFilter.GetEntitiesCount() <= 0)
			{
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return;
			}
			
			foreach (var cameraEntityId in cameraFilter)
			{
				ref var shake = ref shakePool.AddOrGet(cameraEntityId);
				shake.TimeLeft = RoguelikeConstants.Core.Camera.CAMERA_SHAKE_TIME;
				shake.ShakeMagnitude = Mathf.Max(shake.ShakeMagnitude, RoguelikeConstants.Core.Camera.CAMERA_SHAKE_RADIUS);
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}