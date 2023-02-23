using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	public interface IOverlapParams
	{
		EcsPool<LayerComponent> LayerPool { get; }

		EcsPool<CollisionComponent> CollisionPool { get; }

		SimpleIntHashSet FilteredEntities { get; }

		EcsWorld.Mask CreateDefaultFilterMask();

		IOverlapParams SetFilter(EcsFilter filter);

		IOverlapParams Build();
	}
}