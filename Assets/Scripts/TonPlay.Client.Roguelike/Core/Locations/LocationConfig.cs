using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	[Serializable]
	public class LocationConfig : ILocationConfig
	{
		[SerializeField]
		private Vector2 _blockSize;

		[SerializeField]
		private List<LocationBlockList> _blocksMatrix;

		public Vector2 BlockSize => _blockSize;
		
		public IReadOnlyList<IReadOnlyList<LocationBlockView>> BlocksPrefabsMatrix => 
			_blocksMatrix.Select(_ => _.Prefabs).ToList();

		[Serializable]
		private class LocationBlockList
		{
			[SerializeField]
			private List<LocationBlockView> _prefabs;

			public IReadOnlyList<LocationBlockView> Prefabs => _prefabs;
		}
	}
}