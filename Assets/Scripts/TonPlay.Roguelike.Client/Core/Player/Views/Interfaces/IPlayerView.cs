using TonPlay.Roguelike.Client.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Player.Views.Interfaces
{
	public interface IPlayerView : IPositionProvider, IHasWeaponSpawnRoot
	{
		Rigidbody2D Rigidbody2D { get; }
	}
}