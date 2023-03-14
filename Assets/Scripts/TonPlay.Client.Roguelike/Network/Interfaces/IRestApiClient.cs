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
		UniTask<GameSessionResponse> GetGameSession();
		UniTask<GameSessionResponse> PutGameSession(GameSessionPutBody value);
		UniTask<GameSessionResponse> PostGameSession(bool pve);
		UniTask<UserXpResponse> GetUserXp();
		UniTask<UserSummaryResponse> GetUserSummary();
		UniTask<UserItemsResponse> GetUserItems();
		UniTask<UserBalanceResponse> GetUserBalance();
	}
}