namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IRicochetOffTheArenaProjectileConfigProperty : IProjectileConfigProperty
	{
		int CollisionLayerMask { get; }
	}
}