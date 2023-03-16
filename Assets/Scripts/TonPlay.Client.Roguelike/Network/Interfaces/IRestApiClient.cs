using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Network.Interfaces
{
	public interface IRestApiClient
	{
		UniTask<SkillAllResponse> GetSkillAll();
		UniTask<BoostAllResponse> GetBoostAll();
		UniTask<InfoLevelAllResponse> GetInfoLevelAll();
		UniTask<ItemPutResponse> PutItem(ItemPutBody value);
		UniTask<ItemsGetResponse> GetAllItems();
		UniTask<string> DeleteItem(string slotId);
		
		/// <summary>
		/// Get current active user session
		/// </summary>
		/// <returns></returns>
		UniTask<GameSessionResponse> GetGameSession();
		
		/// <summary>
		/// Close game session
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		UniTask<GameSessionResponse> PutGameSession(GameSessionPutBody value);
		
		/// <summary>
		/// Create game session
		/// </summary>
		/// <param name="pve">If True - PvE mode else - PvP mode</param>
		/// <returns></returns>
		UniTask<GameSessionResponse> PostGameSession(bool pve);
		
		UniTask<UserXpResponse> GetUserXp();
		UniTask<UserSummaryResponse> GetUserSummary();
		UniTask<UserItemsResponse> GetUserItems();
		UniTask<UserBalanceResponse> GetUserBalance();
	}
}