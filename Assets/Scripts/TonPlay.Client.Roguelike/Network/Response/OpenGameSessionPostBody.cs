namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class OpenGameSessionPostBody
	{
		public bool pve;
		public string locationId;
	}
}