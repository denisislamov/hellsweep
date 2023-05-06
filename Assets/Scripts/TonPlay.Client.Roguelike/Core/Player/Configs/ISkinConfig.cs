using TonPlay.Client.Roguelike.Core.Player.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Player.Configs
{
	public interface ISkinConfig
	{
		RectTransform GetInventorySpriteForWeaponItemId(string itemId);
		
		PlayerView GetPlayerViewForWeaponItemId(string itemId);
	}
}