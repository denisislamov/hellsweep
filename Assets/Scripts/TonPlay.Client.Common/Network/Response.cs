namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class Response<T>
	{
		public bool successful;
		public T response;
	}
}