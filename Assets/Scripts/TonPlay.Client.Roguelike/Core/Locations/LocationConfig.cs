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
		private bool _infinite;

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

		public string Id => _id;
		public int Index => index;
		public bool Infinite => _infinite;
		public string Title => _title;
		public Sprite Icon => _icon;
		public Vector2 BlockSize => _blockSize;

		public IReadOnlyList<IReadOnlyList<LocationBlockView>> BlocksPrefabsMatrix =>
			_blocksMatrix.Select(_ => _.Prefabs).ToList();
		public SceneName SceneName => _sceneName;
		public bool AlreadyUnlocked => _alreadyUnlocked;
		
		public void AcceptUpdater(ILocationConfigUpdaterVisitor locationConfigUpdaterVisitor)
		{
			locationConfigUpdaterVisitor.Visit(this);
		}
		
		public void SetId(string id)
		{
			_id = id;
		}
		
		public void SetInfinite(bool infinite)
		{
			_infinite = infinite;
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