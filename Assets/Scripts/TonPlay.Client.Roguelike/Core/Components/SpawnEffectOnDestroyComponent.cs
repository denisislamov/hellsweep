using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SpawnEffectOnDestroyComponent
	{
		public IViewPoolIdentity EffectIdentity;
		public float DestroyTimer;
	}
}