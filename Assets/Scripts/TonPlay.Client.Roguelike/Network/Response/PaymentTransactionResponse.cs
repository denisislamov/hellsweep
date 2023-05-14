using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Shop;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class PaymentTransactionResponse
	{
		public string id;
		public PaymentReason reason;
		public string status;
		public string packRateId;
		public PaymentTransactionAttributes attributes;
		
		[System.Serializable]
		public class PaymentTransactionAttributes
		{
			public string id;
			public string itemId;
			public string itemDetailId;
			public long amount;
			public RarityName rarity;
			public SlotName blueprintsSlotPurpose;
		}

		[System.Serializable]
		public class TonPayInResponse
		{
			public string tonkeeper;
			public string tonhub;
		}
	}
}