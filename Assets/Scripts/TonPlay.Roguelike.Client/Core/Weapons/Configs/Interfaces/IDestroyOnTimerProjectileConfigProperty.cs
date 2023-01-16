using TonPlay.Roguelike.Client.Core.Collision.Config;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IDestroyOnTimerProjectileConfigProperty : IProjectileConfigProperty
	{
		float Timer { get; }
	}
}