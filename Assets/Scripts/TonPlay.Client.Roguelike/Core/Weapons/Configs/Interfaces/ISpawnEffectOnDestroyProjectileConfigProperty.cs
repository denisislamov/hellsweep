using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface ISpawnEffectOnDestroyProjectileConfigProperty : IProjectileConfigProperty
	{
		IEffectView EffectView { get; }

		IViewPoolIdentity EffectViewPoolIdentity { get; }
		
		float DestroyTimer { get; }
	}
}