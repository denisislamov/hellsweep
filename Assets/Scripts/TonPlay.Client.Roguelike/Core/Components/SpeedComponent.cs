using System;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SpeedComponent
	{
		public DictionaryExt<MovementSpeedMultiplierType, float> Map;

		public float InitialSpeed;
		
		public float Speed
		{
			get
			{
				var value = 1f;

				if (Map is null)
				{
					Map = new DictionaryExt<MovementSpeedMultiplierType, float>()
					{
						[MovementSpeedMultiplierType.Default] = 1f
					};
				}
				
				foreach (var mapValue in Map.Values)
				{
					value *= mapValue;
				}

				return value * InitialSpeed;
			}
		}
	}
}