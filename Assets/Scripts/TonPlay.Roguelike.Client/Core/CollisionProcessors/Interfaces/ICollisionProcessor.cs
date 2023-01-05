using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces
{
	public interface ICollisionProcessor
	{
		void Process(ref CollisionEventComponent collisionEventComponent);
	}
}