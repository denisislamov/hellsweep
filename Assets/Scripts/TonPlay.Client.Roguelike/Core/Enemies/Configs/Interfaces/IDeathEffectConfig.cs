using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IDeathEffectConfig
	{
		IViewPoolIdentity Identity { get; }
		
		DeathEffectView Prefab { get; }
		
		float DestroyTimer { get; }
		
		int PoolSize { get; }
	}
}