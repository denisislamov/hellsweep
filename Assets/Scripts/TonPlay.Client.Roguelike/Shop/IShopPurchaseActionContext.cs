using System.Threading;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopPurchaseActionContext
	{
		PaymentReason PaymentReason { get; }
		
		object Value { get; }
		
		CancellationToken CancellationToken { get; }
	}
}