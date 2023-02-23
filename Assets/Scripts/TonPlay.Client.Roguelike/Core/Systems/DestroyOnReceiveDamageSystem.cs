using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyOnReceiveDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyOnReceiveDamageComponent>()
							  .Inc<ApplyDamageComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var pool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				pool.AddOrGet(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}