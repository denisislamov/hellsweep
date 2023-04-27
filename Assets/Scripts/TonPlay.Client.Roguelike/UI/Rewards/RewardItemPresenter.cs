using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemPresenter : Presenter<IRewardItemView, IRewardItemContext>
	{
		public RewardItemPresenter(
			IRewardItemView view, 
			IRewardItemContext context) 
			: base(view, context)
		{
			InitView();
		}
		
		private void InitView()
		{
			var showCountPanel = Context.Count > 1;
			
			View.SetIcon(Context.Icon);
			View.SetBackgroundGradientMaterial(Context.GradientMaterial);
			View.SetCountActiveState(showCountPanel);
			View.SetCountText($"x{Context.Count}");
		}

		public class Factory : PlaceholderFactory<IRewardItemView, IRewardItemContext, RewardItemPresenter>
		{
		}
	}
}