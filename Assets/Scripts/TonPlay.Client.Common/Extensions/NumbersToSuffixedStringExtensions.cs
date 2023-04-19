using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TonPlay.Client.Common.Extensions
{
	public static class NumbersToSuffixedStringExtensions
	{
		private static Dictionary<long, string> _suffixes = new Dictionary<long, string>()
		{
			[1_000] = "k",
			[1_000_000] = "M",
			[1_000_000_000] = "B",
			[1_000_000_000_000] = "T",
		};
		
		public static string ConvertToSuffixedFormat(this long value, long convertFromValue, int floatingDigits)
		{
			if (value < 0) return "-" + ConvertToSuffixedFormat(-value, convertFromValue, floatingDigits);
			if (value < convertFromValue) return value.ToString();

			var kvp = _suffixes.First(_ => value / _.Key >= 1 && value / _.Key < 1000);
			var divideBy = kvp.Key;
			var suffix = kvp.Value;
			var powed = (int) Mathf.Pow(10, floatingDigits);

			var truncated = value / (divideBy / powed);

			var hasDecimal = (truncated / (double) powed) != (truncated / powed);
			return hasDecimal ? (truncated / (double) powed) + suffix : (truncated / powed) + suffix;
		}
		
		public static string ConvertToSuffixedFormat(this int value, long convertFromValue, int floatingDigits)
		{
			if (value < 0) return "-" + ConvertToSuffixedFormat(-value, convertFromValue, floatingDigits);
			if (value < convertFromValue) return value.ToString();

			var kvp = _suffixes.First(_ => value / _.Key >= 1 && value / _.Key < 1000);
			var divideBy = kvp.Key;
			var suffix = kvp.Value;
			var powed = (int) Mathf.Pow(10, floatingDigits);

			var truncated = value / (divideBy / powed);

			var hasDecimal = (truncated / (double) powed) != (truncated / powed);
			return hasDecimal ? (truncated / (double) powed) + suffix : (truncated / powed) + suffix;
		}
		
		public static string ConvertToSuffixedFormat(this ushort value, long convertFromValue, int floatingDigits)
		{
			if (value < convertFromValue) return value.ToString();

			var kvp = _suffixes.First(_ => value / _.Key >= 1 && value / _.Key < 1000);
			var divideBy = kvp.Key;
			var suffix = kvp.Value;
			var powed = (int) Mathf.Pow(10, floatingDigits);

			var truncated = value / (divideBy / powed);

			var hasDecimal = (truncated / (double) powed) != (truncated / powed);
			return hasDecimal ? (truncated / (double) powed) + suffix : (truncated / powed) + suffix;
		}
	}
}