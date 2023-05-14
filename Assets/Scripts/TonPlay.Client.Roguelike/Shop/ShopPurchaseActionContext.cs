using System.Threading;

namespace TonPlay.Client.Roguelike.Shop
{
	public class ShopPurchaseActionContext : IShopPurchaseActionContext
	{
		public PaymentReason PaymentReason { get; }
		
		public object Value { get; }
		public CancellationToken CancellationToken { get; }

		public ShopPurchaseActionContext(PaymentReason paymentReason, object value, CancellationToken cancellationToken)
		{
			PaymentReason = paymentReason;
			Value = value;
			CancellationToken = cancellationToken;
		}
	}
}