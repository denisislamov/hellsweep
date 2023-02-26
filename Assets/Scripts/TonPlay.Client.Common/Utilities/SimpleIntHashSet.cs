using System;
using System.Runtime.CompilerServices;

namespace TonPlay.Client.Common.Utilities
{
	public class SimpleIntHashSet
	{
		private const short SIZE = 256;

		private readonly int[] _buckets;
		private readonly int _size;

		private Slot[] _slots;
		private int _lastSlotIndex = -1;

		public int Count => _lastSlotIndex + 1;

		public SimpleIntHashSet(int size = 0)
		{
			_size = size > 0 ? size : SIZE;
			_slots = new Slot[_size];
			_buckets = new int[_size];

			for (int i = 0; i < _size; i++)
			{
				_buckets[i] = -1;
			}
		}
		
		public bool Add(int value)
		{
			var hash = value < 0 ? value * -1 : value;
			var bucketIndex = hash % (_size - 1);
			var bucket = _buckets[bucketIndex];
			var slotIndex = bucket;
			
			while (slotIndex != -1)
			{
				var slot = _slots[slotIndex];
				if (slot.value == value) return false;
				if (slot.next == -1) break;
				slotIndex = slot.next;
			}
			
			var newSlot = new Slot() { value = value, next = bucket };
			var newSlotIndex = ++_lastSlotIndex;
			
			if (newSlotIndex >= _slots.Length)
				Array.Resize(ref _slots, _slots.Length + _size);
			
			_buckets[bucketIndex] = newSlotIndex;
			_slots[newSlotIndex] = newSlot;

			return true;
		}

		public bool Contains(int value)
		{
			var hash = value < 0 ? value * -1 : value;
			var bucketIndex = hash % (_size - 1);
			var bucket = _buckets[bucketIndex];
			var slotIndex = bucket;
			
			while (slotIndex != -1)
			{
				var slot = _slots[slotIndex];
				if (slot.value == value) return true;
				if (slot.next == -1) break;
				slotIndex = slot.next;
			}
			
			return false;
		}
		
		public bool Remove(int value)
		{
			var hash = value < 0 ? value * -1 : value;
			var bucketIndex = hash % (_size - 1);
			var bucket = _buckets[bucketIndex];
			var slotIndex = bucket;

			var previousSlotIndex = -1;
			while (slotIndex != -1)
			{
				var slot = _slots[slotIndex];
				
				if (slot.value == value)
				{
					if (previousSlotIndex == -1)
					{
						_buckets[bucketIndex] = -1;
					}
					else
					{
						_slots[previousSlotIndex].next = -1;
					}
					
					return true;
				}
				
				if (slot.next == -1) break;
				
				previousSlotIndex = slotIndex;
				slotIndex = slot.next;
			}

			return false;
		}

		public void Clear()
		{
			Array.Clear(_slots, 0, _slots.Length);

			for (int i = 0; i < _buckets.Length; i++)
			{
				_buckets[i] = -1;
			}

			_lastSlotIndex = -1;
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public Enumerator GetEnumerator() {
			return new Enumerator (this);
		}
		
		private struct Slot
		{
			public int value;
			public int next;
		}
		
		public struct Enumerator : IDisposable {
			private SimpleIntHashSet set;
			private int index;
			private int current;
 
			internal Enumerator(SimpleIntHashSet set) {
				this.set = set;
				index = 0;
				current = default(int);
			}
 
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public void Dispose() {
				Reset();
			}
 
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public bool MoveNext() {
				while (index < set._lastSlotIndex + 1) {
					if (set._slots[index].value >= 0) {
						current = set._slots[index].value;
						index++;
						return true;
					}
					index++;
				}
				index = set._lastSlotIndex + 1;
				current = default(int);
				return false;
			}
 
			public int Current {
				[MethodImpl (MethodImplOptions.AggressiveInlining)]
				get {
					return current;
				}
			}
 
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public void Reset() {
				index = 0;
				current = default(int);
			}
		}
	}
}