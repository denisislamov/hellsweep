using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearAttackEventsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<AttackEvent>().End();
			var pool = world.GetPool<AttackEvent>();

			foreach (var entityId in filter)
			{
				pool.Del(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}