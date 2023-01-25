namespace TonPlay.Client.Roguelike.Core.Drops.Interfaces
{
	public interface IItemDrop<out T>
	{
		T Drop();
	}
}