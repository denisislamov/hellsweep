using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Pooling.Interfaces
{
	public interface ILevelPoolObjectCreatorList 
	{
		IReadOnlyList<IPoolObjectCreator> All { get; }
	}
}