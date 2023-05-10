namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class UserInventoryResponse
	{
		public long blueprintsArms;
		public long blueprintsBody;
		public long blueprintsBelt;
		public long blueprintsFeet;
		public long blueprintsNeck;
		public long blueprintsWeapon;
		public int keysCommon;
		public int keysUncommon;
		public int keysRare;
		public int keysLegendary;
		public int heroSkins;
	}
}