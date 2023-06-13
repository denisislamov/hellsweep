namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct LocationComponent
	{
		public int[][] BlockEntityIds;
		public int LastNearestBlockToPlayerEntityId;
		public bool InfinityX;
		public bool InfinityY;
	}
}