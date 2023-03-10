using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	public interface ICompositeCollisionArea : ICollisionArea
	{
		IReadOnlyList<ICollisionArea> CollisionAreas { get; }
	}
}