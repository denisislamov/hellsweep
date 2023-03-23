using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class LocationMoveSystem : IEcsRunSystem
	{
		private readonly ILocationConfig _locationConfig;
		private readonly KdTreeStorage _kdTreeStorage;
		private readonly HashSet<int> _movedBlockEntityIds;
		
		private List<int> _resultIndeces = new List<int>();

		public LocationMoveSystem(KdTreeStorage kdTreeStorage, ILocationConfigStorage locationConfigStorage)
		{
			_kdTreeStorage = kdTreeStorage;
			_locationConfig = locationConfigStorage.Current.Value;
			_movedBlockEntityIds = new HashSet<int>();
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

				if ((diff.x > 0 || diff.x < 0) && (diff.y > 0 || diff.y < 0))
				{
					MoveBlocksRow(new Vector2Int(0, -diff.y), _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
					UpdateMovedBlocksPositions(world, positionPool, nearestBlockEntityId);
					UpdateBlocksPositions(world, positionPool);

					MoveBlocksColumn(new Vector2Int(-diff.x, 0), _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
					UpdateMovedBlocksPositions(world, positionPool, nearestBlockEntityId);
					UpdateBlocksPositions(world, positionPool);
				}
				else if (diff.x > 0 || diff.x < 0)
				{
					MoveBlocksColumn(diff, _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
					UpdateMovedBlocksPositions(world, positionPool, nearestBlockEntityId);
					UpdateBlocksPositions(world, positionPool);
				}
				else if (diff.y > 0 || diff.y < -0)
				{
					MoveBlocksRow(diff, _locationConfig.BlockSize, ref location, positionPool, _movedBlockEntityIds);
					UpdateMovedBlocksPositions(world, positionPool, nearestBlockEntityId);
					UpdateBlocksPositions(world, positionPool);
				}

				location.LastNearestBlockToPlayerEntityId = nearestBlockEntityId;
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		private void UpdateBlocksPositions(EcsWorld world, EcsPool<PositionComponent> positionPool)
		{
			var filter = world.Filter<LocationBlockComponent>().Inc<PositionComponent>().End();
			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				var index = _kdTreeStorage.KdTreeEntityIdToPositionIndexMap[entityId];
				_kdTreeStorage.KdTree.Points[index] = position.Position;
			}
			
			_kdTreeStorage.KdTree.Rebuild();
		}

		private void UpdateMovedBlocksPositions(EcsWorld world, EcsPool<PositionComponent> positionPool, int nearestBlockEntityId)
		{
			if (_movedBlockEntityIds.Count <= 0)
			{
				return;
			}
			
			var filter = world
					.Filter<StickToLocationBlockComponent>()
					.Inc<PositionComponent>()
					.Exc<DeadComponent>()
					.Exc<InactiveComponent>()
					.End();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);

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

			_movedBlockEntityIds.Clear();
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
			// var nearestIndex = 0;
			//
			// for (var i = 0; i < _kdTreeStorage.KdTree.Points.Length; i++)
			// {
			// 	var currentPos = _kdTreeStorage.KdTree.Points[i];
			// 	var nearestPos = _kdTreeStorage.KdTree.Points[nearestIndex];
			//
			// 	var dirToCurrent = currentPos - position;
			// 	var dirToNearest = nearestPos - position;
			//
			// 	var sqrToCurrent = dirToCurrent.x * dirToCurrent.x + dirToCurrent.y * dirToCurrent.y;
			// 	var sqrToNearest = dirToNearest.x * dirToNearest.x + dirToNearest.y * dirToNearest.y;
			//
			// 	if (sqrToCurrent < sqrToNearest)
			// 	{
			// 		nearestIndex = i;
			// 	}
			// }
			
			_resultIndeces.Clear();
			_kdTreeStorage.KdQuery.KNearest(_kdTreeStorage.KdTree, position, 1, _resultIndeces);
			
			var nearest = _resultIndeces[0];
			var nearestEntityId = _kdTreeStorage.KdTreePositionIndexToEntityIdMap[nearest];
			return nearestEntityId;
		}
	}
}