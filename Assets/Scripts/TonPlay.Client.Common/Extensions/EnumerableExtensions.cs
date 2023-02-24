using System.Collections.Generic;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Common.Extensions
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
		
		public static SimpleIntHashSet ToCachedSimpleIntHashSet(this int[] array, SimpleIntHashSet hashSet)
		{
			for (var index = 0; index < array.Length; index++)
			{
				var val = array[index];
				hashSet.Add(val);
			}
			return hashSet;
		}
		
		public static Stack<int> ToCachedStack(this SimpleIntHashSet hashSet, Stack<int> stack)
		{
			foreach (var val in hashSet)
			{
				stack.Push(val);
			}
			return stack;
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