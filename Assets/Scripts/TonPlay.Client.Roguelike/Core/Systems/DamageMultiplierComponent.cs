using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public struct DamageMultiplierComponent
	{
		public DictionaryExt<DamageMultiplierType, float> Map;

		public float Value
		{
			get
			{
				var value = 0f;
				
				foreach (var mapValue in Map.Values)
				{
					value += mapValue;
				}

				return value;
			}
		}
	}
}