using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class LocationSpawnSystem : IEcsInitSystem
	{
		private readonly Transform _blocksRoot;
		private readonly ILocationConfig _locationConfig;
		private readonly KdTreeStorage _kdTreeStorage;

		public LocationSpawnSystem(
			Transform blocksRoot,
			KdTreeStorage kdTreeStorage,
			ILocationConfigStorage locationConfigStorage)
		{
			_blocksRoot = blocksRoot;
			_kdTreeStorage = kdTreeStorage;
			_locationConfig = locationConfigStorage.Current.Value;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();
			var positionPool = world.GetPool<PositionComponent>();

			var size = _locationConfig.BlockSize;
			var matrix = _locationConfig.BlocksPrefabsMatrix;
			var matrixSize = matrix.Sum(_ => _.Count);

			var positions = new Vector2[matrixSize];

			var locationEntity = world.NewEntity();

			ref var location = ref locationEntity.Add<LocationComponent>();
			location.BlockEntityIds = new int[matrix.Count][];
			location.InfinityX = _locationConfig.InfiniteX;
			location.InfinityY = _locationConfig.InfiniteY;
			
			var index = 0;

			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(matrixSize);
			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(matrixSize);

			if (!_locationConfig.InfiniteX)
			{
				var col = -1;
				for (var i = 0; i < 2; i++)
				{
					for (var row = 0; row < matrix.Count; row++)
					{
						var x = col - matrix[0].Count/2;
						var y = row - matrix.Count/2;
						var prefab = _locationConfig.BlockerPrefab;
						var position = new Vector2(size.x * x, size.y * y);

						var view = Object.Instantiate(prefab, _blocksRoot);
						view.transform.position = position;
					}

					col = matrix[0].Count;
				}
			}

			if (!_locationConfig.InfiniteY)
			{
				var row = -1;
				for (var i = 0; i < 2; i++)
				{
					for (var col = 0; col < matrix[0].Count; col++)
					{
						var x = col - matrix[0].Count/2;
						var y = row - matrix.Count/2;
						var prefab = _locationConfig.BlockerPrefab;
						var position = new Vector2(size.x*x, size.y*y);

						var view = Object.Instantiate(prefab, _blocksRoot);
						view.transform.position = position;
					}

					row = matrix.Count;
				}
			}
			
			for (var row = 0; row < matrix.Count; row++)
			{
				location.BlockEntityIds[row] = new int[matrix[row].Count];

				for (var col = 0; col < matrix[row].Count; col++)
				{
					var blockEntity = world.NewEntity();
					var x = col - matrix[row].Count/2;
					var y = row - matrix.Count/2;
					var prefab = matrix[row][col];
					var position = new Vector2(size.x*x, size.y*y);

					var view = Object.Instantiate(prefab, _blocksRoot);

					ref var boxSize = ref blockEntity.Add<BoxSizeComponent>();
					boxSize.Size = _locationConfig.BlockSize;

					ref var locationBlock = ref blockEntity.Add<LocationBlockComponent>();
					locationBlock.Id = new Vector2Int(x, y);

					ref var positionComponent = ref blockEntity.Add<PositionComponent>();
					positionComponent.Position = position;

					ref var transformComponent = ref blockEntity.Add<TransformComponent>();
					transformComponent.Transform = view.transform;
					
					view.SetText($"[{row}:{col}]");

					location.BlockEntityIds[row][col] = blockEntity.Id;

					_kdTreeStorage.KdTreePositionIndexToEntityIdMap[index] = blockEntity.Id;
					_kdTreeStorage.KdTreeEntityIdToPositionIndexMap[blockEntity.Id] = index;

					positions[index] = position;

					index++;
				}
			}

			var locationSize = new Vector2(
				_locationConfig.InfiniteX 
					? float.PositiveInfinity 
					: location.BlockEntityIds[0].Length * _locationConfig.BlockSize.x,
				_locationConfig.InfiniteY 
					? float.PositiveInfinity 
					: location.BlockEntityIds.Length * _locationConfig.BlockSize.y);
			
			sharedData.SetLocationSize(locationSize);

			_kdTreeStorage.KdTree.Build(positions);

			var playerPosition = systems.GetShared<ISharedData>().PlayerPositionProvider.Position;
			var nearestToPlayer = FindNearestBlockEntityId(playerPosition);

			location.LastNearestBlockToPlayerEntityId = nearestToPlayer;
		}

		private int FindNearestBlockEntityId(Vector2 position)
		{
			var resultIndeces = new List<int>();
			_kdTreeStorage.KdQuery.KNearest(_kdTreeStorage.KdTree, position, 1, resultIndeces);
			var nearest = resultIndeces[0];
			var nearestEntityId = _kdTreeStorage.KdTreePositionIndexToEntityIdMap[nearest];
			return nearestEntityId;
		}
	}
}