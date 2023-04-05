using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Network.Interfaces
{
	public interface IRestApiClient
	{
		void Init();
		
		UniTask<Response<SkillAllResponse>> GetSkillAll();
		UniTask<Response<BoostAllResponse>> GetBoostAll();
		UniTask<Response<LocationAllResponse>> GetLocationAll();
		UniTask<Response<InfoLevelAllResponse>> GetInfoLevelAll();
		UniTask<Response<ItemPutResponse>> PutItem(ItemPutBody value);
		UniTask<Response<ItemsGetResponse>> GetAllItems();
		UniTask<Response<string>> DeleteItem(string slotId);
		
		/// <summary>
		/// Get current active user session
		/// </summary>
		/// <returns></returns>
		UniTask<Response<GameSessionResponse>> GetGameSession();

		/// <summary>
		/// Close game session
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		UniTask<Response<GameSessionResponse>> PostGameSessionClose(CloseGameSessionPostBody value);
		
		/// <summary>
		/// Create game session
		/// </summary>
		/// <param name="pve">If True - PvE mode else - PvP mode</param>
		/// <returns></returns>
		UniTask<Response<GameSessionResponse>> PostGameSession(OpenGameSessionPostBody pve);
		
		UniTask<Response<UserXpResponse>> GetUserXp();
		UniTask<Response<UserSummaryResponse>> GetUserSummary();
		UniTask<Response<UserItemsResponse>> GetUserItems();
		UniTask<Response<UserSlotsResponse>> GetUserSlots();
		UniTask<Response<UserBalanceResponse>> GetUserBalance();
		UniTask<Response<UserLocationsResponse>> GetUserLocations();
	}
}