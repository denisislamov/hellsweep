using System;
using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class MatchProfileGainData
	{
		public int Gold { get; set; }
		public float ProfileExperience { get; set; }
		public long BlueprintsArms { get; set; }
		public long BlueprintsBody { get; set; }
		public long BlueprintsBelt { get; set; }
		public long BlueprintsFeet { get; set; }
		public long BlueprintsNeck { get; set; }
		public long BlueprintsWeapon { get; set; }
		public List<InventoryItemData> Items { get; set; }
		
		public void SetBlueprintsValue(SlotName blueprintsSlot, int value)
		{
			switch (blueprintsSlot)
			{
				case SlotName.ARMS:
					BlueprintsArms = value;
					break;
				case SlotName.BELT:
					BlueprintsBelt = value;
					break;
				case SlotName.BODY:
					BlueprintsBody = value;
					break;
				case SlotName.FEET:
					BlueprintsFeet = value;
					break;
				case SlotName.NECK:
					BlueprintsNeck = value;
					break;
				case SlotName.WEAPON:
					BlueprintsWeapon = value;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(blueprintsSlot), blueprintsSlot, null);
			}
		}
		
		public long GetBlueprintsValue(SlotName blueprintsSlot)
		{
			switch (blueprintsSlot)
			{
				case SlotName.ARMS:
					return BlueprintsArms;
				case SlotName.BELT:
					return BlueprintsBelt;
				case SlotName.BODY:
					return BlueprintsBody;
				case SlotName.FEET:
					return BlueprintsFeet;
				case SlotName.NECK:
					return BlueprintsNeck;
				case SlotName.WEAPON:
					return BlueprintsWeapon;
				default:
					throw new ArgumentOutOfRangeException(nameof(blueprintsSlot), blueprintsSlot, null);
			}
		}
	}
}