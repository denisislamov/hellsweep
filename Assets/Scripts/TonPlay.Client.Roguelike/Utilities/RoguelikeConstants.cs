namespace TonPlay.Client.Roguelike.Utilities
{
	public static class RoguelikeConstants
	{
		public static class Core
		{
			public const int PLAYER_PROJECTILES_MAX_COUNT = 64;
			public const float REQUIRED_POSITION_CHANGED_DIFF = 0.00001f;
			
			public class Collision
			{
				public const int OVERLAP_MIN_RADIUS = 5;
			}
		}
	}
}