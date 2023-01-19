using System.Collections.Generic;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationConfig
	{
		Vector2 BlockSize { get; }
		
		IReadOnlyList<IReadOnlyList<LocationBlockView>> BlocksPrefabsMatrix { get; }
	}
}