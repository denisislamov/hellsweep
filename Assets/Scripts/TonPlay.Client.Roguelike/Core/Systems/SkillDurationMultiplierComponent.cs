using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public struct SkillDurationMultiplierComponent
	{
		public DictionaryExt<SkillDurationMultiplierType, float> Map;

		public float Value
		{
			get
			{
				var value = 1f;
				
				foreach (var mapValue in Map.Values)
				{
					value *= mapValue;
				}

				return value;
			}
		}
	}
}