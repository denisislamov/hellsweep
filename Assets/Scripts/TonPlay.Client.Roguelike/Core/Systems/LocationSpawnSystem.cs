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
	public class LocationSpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly Transform _blocksRoot;
		private readonly ILocationConfig _locationConfig;
		private readonly KdTreeStorage _kdTreeStorage;
		private readonly HashSet<int> _movedBlockEntityIds;

		public LocationSpawnSystem(Transform blocksRoot, ILocationConfigStorage locationConfigStorage)
		{
			_blocksRoot = blocksRoot;
			_kdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("Default"));
			_locationConfig = locationConfigStorage.Current;
			_movedBlockEntityIds = new HashSet<int>();
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var positionPool = world.GetPool<PositionComponent>();

			var size = _locationConfig.BlockSize;
			var matrix = _locationConfig.BlocksPrefabsMatrix;
			var matrixSize = matrix.Sum(_ => _.Count);

			var positions = new Vector2[matrixSize];

			var locationEntity = world.NewEntity();

			ref var location = ref locationEntity.Add<LocationComponent>();
			location.BlockEntityIds = new int[matrix.Count][];

			var index = 0;

			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(matrixSize);
			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(matrixSize);

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

					location.BlockEntityIds[row][col] = blockEntity.Id;

					_kdTreeStorage.KdTreePositionIndexToEntityIdMap[index] = blockEntity.Id;
					_kdTreeStorage.KdTreeEntityIdToPositionIndexMap[blockEntity.Id] = index;

					positions[index] = position;

					index++;
				}
			}

			_kdTreeStorage.KdTree.Build(positions);

			var playerPosition = systems.GetShared<ISharedData>().PlayerPositionProvider.Position;
			var nearestToPlayer = FindNearestBlockEntityId(playerPosition);

			location.LastNearestBlockToPlayerEntityId = nearestToPlayer;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<LocationComponent>().End();
			var blockPool = world.GetPool<LocationBlockComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var locationPool = world.GetPool<LocationComponent>();
			var playerPosition = systems.GetShared<ISharedData>().PlayerPositionProvider.Position;

			var nearestBlockEntityId = FindNearestBlockEntityId(playerPosition);

			foreach (var entityId in filter)
			{
				ref var location = ref locationPool.Get(entityId);

				if (location.LastNearestBlockToPlayerEntityId == nearestBlockEntityId)
				{
					continue;
				}

				ref var lastNearest = ref blockPool.Get(location.LastNearestBlockToPlayerEntityId);
				ref var currentNearest = ref blockPool.Get(nearestBlockEntityId);

				var diff = currentNearest.Id - lastNearest.Id;

				if (diff.sqrMagnitude > 1)
				{
					var currentNearestId = currentNearest.Id - currentNearest.Id;
					var lastNearestId = lastNearest.Id - currentNearest.Id;

					diff = lastNearestId - currentNearestId;

					var vector2 = new Vector2(diff.x, diff.y);
					vector2.Normalize();

					diff = new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
				}

				if (diff.x > 0 || diff.x < 0)
				{
					MoveBlocksColumn(diff, _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
				}
				else if (diff.y > 0 || diff.y < -0)
				{
					MoveBlocksRow(diff, _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
				}

				location.LastNearestBlockToPlayerEntityId = nearestBlockEntityId;
			}

			if (_movedBlockEntityIds.Count > 0)
			{
				filter = world
						.Filter<StickToLocationBlockComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.Exc<InactiveComponent>()
						.End();

				foreach (var entityId in filter)
				{
					ref var position = ref positionPool.Get(entityId);

					nearestBlockEntityId = FindNearestBlockEntityId(position.Position);

					if (!_movedBlockEntityIds.Contains(nearestBlockEntityId))
					{
						continue;
					}

					ref var nearestBlockPosition = ref positionPool.Get(nearestBlockEntityId);

					var index = _kdTreeStorage.KdTreeEntityIdToPositionIndexMap[nearestBlockEntityId];

					var oldPosition = _kdTreeStorage.KdTree.Points[index];
					var currentPosition = nearestBlockPosition.Position;
					var diff = new Vector2(currentPosition.x - oldPosition.x, currentPosition.y - oldPosition.y);

					position.Position += diff;
				}
			}

			_movedBlockEntityIds.Clear();

			filter = world.Filter<LocationBlockComponent>().Inc<PositionComponent>().End();
			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				var index = _kdTreeStorage.KdTreeEntityIdToPositionIndexMap[entityId];
				_kdTreeStorage.KdTree.Points[index] = position.Position;
			}

			_kdTreeStorage.KdTree.Rebuild();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void MoveBlocksRow(
			Vector2Int diff,
			Vector2 blockSize,
			ref LocationComponent location,
			EcsPool<PositionComponent> positionPool,
			HashSet<int> movedBlockEntityIds)
		{
			var rows = location.BlockEntityIds.GetLength(0);
			var cols = location.BlockEntityIds[0].GetLength(0);

			var targetRow = diff.y < 0 ? rows - 1 : 0;
			var newNeighborRow = diff.y < 0 ? 0 : rows - 1;

			for (var col = 0; col < cols; col++)
			{
				var targetBlockEntityId = location.BlockEntityIds[targetRow][col];
				var newNeighborBlockEntityId = location.BlockEntityIds[newNeighborRow][col];

				ref var targetPosition = ref positionPool.Get(targetBlockEntityId);
				ref var newNeighborPosition = ref positionPool.Get(newNeighborBlockEntityId);

				targetPosition.Position = newNeighborPosition.Position + new Vector2(blockSize.x*diff.x, blockSize.y*diff.y);

				movedBlockEntityIds.Add(targetBlockEntityId);

				if (diff.y < 0)
				{
					var tmp = location.BlockEntityIds[rows - 1][col];
					for (var i = rows - 1; i > 0; i--)
					{
						location.BlockEntityIds[i][col] = location.BlockEntityIds[i - 1][col];
					}
					location.BlockEntityIds[0][col] = tmp;
				}
				else
				{
					var tmp = location.BlockEntityIds[0][col];
					for (var i = 0; i < rows - 1; i++)
					{
						location.BlockEntityIds[i][col] = location.BlockEntityIds[i + 1][col];
					}
					location.BlockEntityIds[cols - 1][col] = tmp;
				}
			}
		}

		private static void MoveBlocksColumn(
			Vector2Int diff,
			Vector2 blockSize,
			ref LocationComponent location,
			EcsPool<PositionComponent> positionPool,
			HashSet<int> movedBlockEntityIds)
		{
			var rows = location.BlockEntityIds.GetLength(0);

			for (var row = 0; row < rows; row++)
			{
				var cols = location.BlockEntityIds[row].GetLength(0);

				var targetColumn = diff.x < -0.1f ? cols - 1 : 0;
				var newNeighborColumn = diff.x < -0.1f ? 0 : cols - 1;

				var targetBlockEntityId = location.BlockEntityIds[row][targetColumn];
				var newNeighborBlockEntityId = location.BlockEntityIds[row][newNeighborColumn];

				ref var targetPosition = ref positionPool.Get(targetBlockEntityId);
				ref var newNeighborPosition = ref positionPool.Get(newNeighborBlockEntityId);

				targetPosition.Position = newNeighborPosition.Position + new Vector2(blockSize.x*diff.x, blockSize.y*diff.y);

				movedBlockEntityIds.Add(targetBlockEntityId);

				if (diff.x < 0)
				{
					var tmp = location.BlockEntityIds[row][cols - 1];
					for (var i = cols - 1; i > 0; i--)
					{
						location.BlockEntityIds[row][i] = location.BlockEntityIds[row][i - 1];
					}
					location.BlockEntityIds[row][0] = tmp;
				}
				else
				{
					var tmp = location.BlockEntityIds[row][0];
					for (var i = 0; i < cols - 1; i++)
					{
						location.BlockEntityIds[row][i] = location.BlockEntityIds[row][i + 1];
					}
					location.BlockEntityIds[row][cols - 1] = tmp;
				}
			}
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