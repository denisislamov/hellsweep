namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IModel<T> where T : IData
	{
		void Update(T data);

		T ToData();
	}
}