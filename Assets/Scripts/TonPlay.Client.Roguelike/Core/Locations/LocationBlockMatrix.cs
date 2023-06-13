using System.Collections.Generic;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	public abstract class LocationBlockMatrix : ScriptableObject, ILocationBlockMatrix
	{
		public abstract IReadOnlyList<IReadOnlyList<LocationBlockView>> Matrix { get; }
	}
}