namespace TonPlay.Roguelike.Client.Core.Pooling.Interfaces
{
	public interface IViewPoolObject
	{
		void Release();
	}

	public interface IViewPoolObject<out T> : IViewPoolObject
	{
		T Object { get; }
	}
}