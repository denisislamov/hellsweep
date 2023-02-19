using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	public interface IOverlapPools
	{
		EcsPool<InactiveComponent> InactivePool { get; }
		EcsPool<DeadComponent> DeadPool { get; }
		EcsPool<UsedComponent> UsedPool { get; }
		EcsPool<LayerComponent> LayerPool { get; }
		EcsPool<DestroyComponent> DestroyPool { get; }
	}
}