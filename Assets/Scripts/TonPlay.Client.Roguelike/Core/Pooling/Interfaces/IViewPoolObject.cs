namespace TonPlay.Client.Roguelike.Core.Pooling.Interfaces
{
	public interface IViewPoolObject
	{
		void Release();
	}

	public interface IViewPoolObject<out T> : IViewPoolObject
	{
		T Object { get; }
		
		bool Active { get; }
		
		int EntityId { get; set; }

		void SetActive(bool state);
	}
}