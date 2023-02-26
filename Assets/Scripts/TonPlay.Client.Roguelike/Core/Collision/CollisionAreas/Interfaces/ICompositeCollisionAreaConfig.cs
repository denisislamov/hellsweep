using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces
{
	public interface ICompositeCollisionAreaConfig : ICollisionAreaConfig
	{
		IReadOnlyList<ICollisionAreaConfig> CollisionAreaConfigs { get; }
	}
}