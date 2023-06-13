using System.Collections.Generic;
using TonPlay.Client.Common.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationConfig
	{
		string Id { get; }
		
		int ChapterIdx { get; }
		
		bool InfiniteX { get; }
		
		bool InfiniteY { get; }

		string Title { get; }

		Sprite Icon { get; }

		Vector2 BlockSize { get; }

		IReadOnlyList<IReadOnlyList<LocationBlockView>> BlocksPrefabsMatrix { get; }

		SceneName SceneName { get; }
		
		bool AlreadyUnlocked { get; }
		
		GameObject BlockerPrefab { get; }

		void AcceptUpdater(ILocationConfigUpdaterVisitor locationConfigUpdaterVisitor);
	}
}