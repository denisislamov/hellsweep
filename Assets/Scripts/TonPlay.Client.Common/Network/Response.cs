namespace TonPlay.Client.Common.Network
{
	[System.Serializable]
	public class Response<T>
	{
		public bool successful;
		public T response;
		public ErrorResponse error;
	}
}