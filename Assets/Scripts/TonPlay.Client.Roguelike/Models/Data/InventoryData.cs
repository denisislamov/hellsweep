using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class InventoryData : IData
	{
		public List<InventoryItemData> Items { get; set; }
		public Dictionary<SlotName, SlotData> Slots { get; set; }
		public List<SlotData> MergeSlots { get; set; }
		
		public long BlueprintsArms { get; set; }
		
		public long BlueprintsBody { get; set; }
		
		public long BlueprintsBelt { get; set; }
		
		public long BlueprintsFeet { get; set; }
		
		public long BlueprintsNeck { get; set; }
		
		public long BlueprintsWeapon { get; set; }
		
		public int CommonKeys { get; set; }
		
		public int UncommonKeys { get; set; }
		
		public int RareKeys { get; set; }
		
		public int LegendaryKeys { get; set; }

		public void SetBlueprintsValue(SlotName slotName, long value)
		{
			switch (slotName)
			{
				case SlotName.ARMS:
					BlueprintsArms = value;
					return;
				case SlotName.BODY:
					BlueprintsBody = value;
					return;
				case SlotName.BELT:
					BlueprintsBelt = value;
					return;
				case SlotName.FEET:
					BlueprintsFeet = value;
					return;
				case SlotName.NECK:
					BlueprintsNeck = value;
					return;
				case SlotName.WEAPON:
					BlueprintsWeapon = value;
					return;
			}
		}
		
		public long GetBlueprintsValue(SlotName slotName)
		{
			switch (slotName)
			{
				case SlotName.ARMS:
					return BlueprintsArms;
				case SlotName.BODY:
					return BlueprintsBody;
				case SlotName.BELT:
					return BlueprintsBelt;
				case SlotName.FEET:
					return BlueprintsFeet;
				case SlotName.NECK:
					return BlueprintsNeck;
				case SlotName.WEAPON:
					return BlueprintsWeapon;
			}

			return -1;
		}
		
		public long GetKeysValue(RarityName rarity)
		{
			switch (rarity)
			{
				case RarityName.COMMON:
					return CommonKeys;
				case RarityName.UNCOMMON:
					return UncommonKeys;
				case RarityName.RARE:
					return RareKeys;
				case RarityName.LEGENDARY:
					return LegendaryKeys;
			}

			return -1;
		}
	}
}