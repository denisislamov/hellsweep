using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyPoolObjectSystem : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<DestroyComponent>().Inc<ViewPoolObjectComponent>().Exc<GameNonPoolObject>().End();
				var pool = world.GetPool<ViewPoolObjectComponent>();

				foreach (var entityId in filter)
				{
					ref var component = ref pool.Get(entityId);
					component.ViewPoolObject.Release();
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}