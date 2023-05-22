using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyOnTimerSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld[] _worlds;
		
		public void Init(EcsSystems systems)
		{
			_worlds = new EcsWorld[]
			{
				systems.GetWorld(),
				systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
			};
		}
		
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			for (var index = 0; index < _worlds.Length; index++)
			{
				var world = _worlds[index];
				var filter = world.Filter<DestroyOnTimerComponent>().Exc<InactiveComponent>().Exc<DestroyComponent>().End();
				var pool = world.GetPool<DestroyOnTimerComponent>();
				var destroyPool = world.GetPool<DestroyComponent>();

				foreach (var entityId in filter)
				{
					ref var component = ref pool.Get(entityId);
					component.TimeLeft -= Time.deltaTime;

					if (component.TimeLeft <= 0)
					{
						pool.Del(entityId);
						destroyPool.AddOrGet(entityId);
					}
				}	
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}