using System.Collections.Generic;

namespace TonPlay.Client.Common.Utilities
{
	public static class EnumerableExtensions
	{
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
		{
			var hashset = new HashSet<T>();
			foreach (var val in enumerable)
			{
				hashset.Add(val);
			}
			return hashset;
		}
		
		public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable)
		{
			var hashset = new Stack<T>();
			foreach (var val in enumerable)
			{
				hashset.Push(val);
			}
			return hashset;
		}
	}
}