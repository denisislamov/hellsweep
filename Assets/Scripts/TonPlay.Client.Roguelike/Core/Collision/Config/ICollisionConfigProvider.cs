namespace TonPlay.Roguelike.Client.Core.Collision.Config
{
	public interface ICollisionConfigProvider
	{
		ICollisionConfig Get(int layer);
	}
}