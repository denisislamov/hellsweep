using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	public interface ILocationBlockMatrix
	{
		IReadOnlyList<IReadOnlyList<LocationBlockView>> Matrix { get; }
	}
}