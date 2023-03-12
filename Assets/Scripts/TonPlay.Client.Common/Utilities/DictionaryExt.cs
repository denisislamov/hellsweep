using System.Collections.Generic;

namespace TonPlay.Client.Common.Utilities
{
	public class DictionaryExt<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public TValue this[TKey key]
		{
			get
			{
				if (!ContainsKey(key))
				{
					Add(key, default(TValue));
				}
				
				return base[key];
			}
			set
			{
				if (!ContainsKey(key))
				{
					Add(key, value);
				}
				else
				{
					base[key] = value;
				}
			}
		}
	}
}