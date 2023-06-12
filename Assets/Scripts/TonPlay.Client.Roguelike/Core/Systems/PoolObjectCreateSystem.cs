using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PoolObjectCreateSystem : IEcsInitSystem
	{
		public void Init(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			
			foreach (var poolObjectCreator in sharedData.LevelPoolObjectCreatorList.All)
			{
				poolObjectCreator.Create(sharedData.CompositeViewPool);
			}
		}
	}
}