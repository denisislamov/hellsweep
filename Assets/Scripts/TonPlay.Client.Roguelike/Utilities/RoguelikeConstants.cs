namespace TonPlay.Client.Roguelike.Utilities
{
	public static class RoguelikeConstants
	{
		public static class Meta
		{
			public const int MATCH_ENERGY_PRICE_BASE = 5;
			public const int INCREASE_ENERGY_PER_GAINED_LEVEL = 5;
		}
		
		public static class Core
		{
			public const int PLAYER_PROJECTILES_MAX_COUNT = 64;
			public const int ARENA_MAX_COUNT = 1;
			public const float REQUIRED_POSITION_CHANGED_DIFF = 0.00001f;

			public class Collision
			{
				public const int OVERLAP_MIN_RADIUS = 10;
			}
			
			public class Camera
			{
				public const float CAMERA_SHAKE_TIME = 0.25f;
				public const float CAMERA_SHAKE_RADIUS = 0.15f;
			}
			
			public class Rewards
			{
				public const string COINS_ID = "coins";
				public const string PROFILE_EXPERIENCE_ID = "profile_experience";
			}
		}
	}
}