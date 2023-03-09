using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SpawnExperienceCollectablesToGainFirstLevelSystem : IEcsInitSystem
	{
		private readonly ICollectableEntityFactory _collectablesEntityFactory;
		
		public SpawnExperienceCollectablesToGainFirstLevelSystem(ICollectableEntityFactory collectablesEntityFactory)
		{
			_collectablesEntityFactory = collectablesEntityFactory;
		}
		
		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			var initialExperienceCollectable = sharedData.CollectablesConfigProvider.InitialExperienceCollectableForFirstLevel;
			var requiredExperienceAmount = sharedData.PlayersLevelsConfigProvider.Get(0);

			var spawnCollectablesCount = Mathf.FloorToInt(requiredExperienceAmount.ExperienceToNextLevel / initialExperienceCollectable.Value);
			for (var i = 0; i < spawnCollectablesCount; i++)
			{
				var position = GeneratePosition(sharedData);
				
				_collectablesEntityFactory.Create(world, initialExperienceCollectable, position);
			}
		}
		
		private static Vector2 GeneratePosition(ISharedData sharedData)
		{
			return sharedData.PlayerPositionProvider.Position + Random.onUnitSphere.ToVector2XY() * Random.Range(3f, 6f);
		}
	}
}