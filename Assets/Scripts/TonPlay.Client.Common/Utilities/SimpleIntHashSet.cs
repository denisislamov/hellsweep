using System;

namespace TonPlay.Client.Common.Utilities
{
	public class SimpleIntHashSet
	{
		private const short SIZE = 256;

		private readonly int[] _buckets;
		
		private Slot[] _slots;
		private int _lastSlotIndex = -1;

		public int Count => _lastSlotIndex + 1;

		public SimpleIntHashSet()
		{
			_slots = new Slot[SIZE];
			_buckets = new int[SIZE];

			for (int i = 0; i < SIZE; i++)
			{
				_buckets[i] = -1;
			}
		}
		
		public bool Add(int value)
		{
			var hash = value < 0 ? value * -1 : value;
			var bucketIndex = hash % (SIZE - 1);
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
				Array.Resize(ref _slots, _slots.Length + SIZE);
			
			_buckets[bucketIndex] = newSlotIndex;
			_slots[newSlotIndex] = newSlot;

			return true;
		}

		public bool Contains(int value)
		{
			var hash = value < 0 ? value * -1 : value;
			var bucketIndex = hash % (SIZE - 1);
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

		public void Clear()
		{
			Array.Clear(_slots, 0, _slots.Length);

			for (int i = 0; i < _buckets.Length; i++)
			{
				_buckets[i] = -1;
			}

			_lastSlotIndex = -1;
		}

		private struct Slot
		{
			public int value;
			public int next;
		}
	}
}