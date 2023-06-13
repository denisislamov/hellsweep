using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	[Serializable]
	public class LocationConfig : ILocationConfig
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private bool _infiniteX;
		
		[SerializeField]
		private bool _infiniteY;

		[SerializeField]
		private string _title;

		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private SceneName _sceneName;

		[SerializeField]
		private Vector2 _blockSize;
		
		[SerializeField]
		private bool _alreadyUnlocked;

		[SerializeField]
		private List<LocationBlockList> _blocksMatrix;

		public int index = -1;
		
		[SerializeField]
		private LocationBlockMatrix _locationBlockMatrix;
		
		[SerializeField]
		private GameObject _blockerPrefab;

		public string Id => _id;
		public int ChapterIdx => index;
		public bool InfiniteX => _infiniteX;
		public bool InfiniteY => _infiniteY;
		public string Title => _title;
		public Sprite Icon => _icon;
		public Vector2 BlockSize => _blockSize;

		public IReadOnlyList<IReadOnlyList<LocationBlockView>> BlocksPrefabsMatrix =>
			_locationBlockMatrix is null 
				? _blocksMatrix.Select(_ => _.Prefabs).ToList() 
				: _locationBlockMatrix.Matrix;
		
		public SceneName SceneName => _sceneName;
		public bool AlreadyUnlocked => _alreadyUnlocked;
		public GameObject BlockerPrefab => _blockerPrefab;

		public void AcceptUpdater(ILocationConfigUpdaterVisitor locationConfigUpdaterVisitor)
		{
			locationConfigUpdaterVisitor.Visit(this);
		}
		
		public void SetId(string id)
		{
			_id = id;
		}
		
		public void SetInfiniteX(bool infinite)
		{
			_infiniteX = infinite;
		}
		
		public void SetInfiniteY(bool infinite)
		{
			_infiniteY = infinite;
		}

		[Serializable]
		private class LocationBlockList
		{
			[SerializeField]
			private List<LocationBlockView> _prefabs;

			public IReadOnlyList<LocationBlockView> Prefabs => _prefabs;
		}
	}
}