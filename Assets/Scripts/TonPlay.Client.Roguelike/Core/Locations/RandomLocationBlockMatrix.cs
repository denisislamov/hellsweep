using System.Collections.Generic;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	[CreateAssetMenu(fileName = nameof(RandomLocationBlockMatrix), menuName = AssetMenuConstants.LOCATION_CONFIGS + nameof(RandomLocationBlockMatrix))]
	public class RandomLocationBlockMatrix : LocationBlockMatrix
	{
		[SerializeField] private LocationBlockView[] _blocks;
		[SerializeField] private Vector2 _size;
		public override IReadOnlyList<IReadOnlyList<LocationBlockView>> Matrix => GenerateMatrix();
		
		private IReadOnlyList<IReadOnlyList<LocationBlockView>> GenerateMatrix()
		{
			var result = new List<List<LocationBlockView>>();
			
			for (var row = 0; row < _size.y; row++)
			{
				result.Add(new List<LocationBlockView>());
				
				for (var col = 0; col < _size.x; col++)
				{
					var blockIndex = Random.Range(0, _blocks.Length);
					var block = _blocks[blockIndex];
					result[row].Add(block);
				}
			}

			return result;
		}
	}
}