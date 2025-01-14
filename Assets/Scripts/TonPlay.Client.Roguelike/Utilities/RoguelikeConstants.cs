using UnityEngine;

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
			public const int KD_TREE_ENEMY_INIT_COUNT = 128;
			public const float REQUIRED_POSITION_CHANGED_DIFF = 0.00001f;
			public const int AMOUNT_OF_START_LEVELS_TO_SPAWN_EQUAL_EXP = 1;
			public const float REMOTE_CONFIG_RANGE_MULTIPLIER = 2f;
			public const float MAX_MATCH_TIME_MINUTES = 15;
			public const string EFFECTS_WORLD_NAME = "EFFECTS";

			public static class Skills
			{
				public static class FeatureIds
				{
					public const string DAMAGE = "DAMAGE";
					public const string QUANTITY = "QUANTITY";
					public const string COOLDOWN = "COOLDOWN";
					public const string RANGE = "RANGE";
					public const string FLIGHT_SPEED = "FLIGHT_SPEED";
					public const string SPIN_SPEED = "SPIN_SPEED";
					public const string DURATION = "DURATION";
				}
			}

			public class Collision
			{
				public const float OVERLAP_MIN_RADIUS = 2.5f;
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
				public const string BLUEPRINTS_ARMS = "blueprints_arms";
				public const string BLUEPRINTS_BODY = "blueprints_body";
				public const string BLUEPRINTS_BELT = "blueprints_belt";
				public const string BLUEPRINTS_FEET = "blueprints_feet";
				public const string BLUEPRINTS_NECK = "blueprints_neck";
				public const string BLUEPRINTS_WEAPON = "blueprints_weapons";
			}

			public class UI
			{
				public static readonly Vector2 HEALTH_OFFSET = new Vector2(0, -1.15f);
			}
		}
	}
}