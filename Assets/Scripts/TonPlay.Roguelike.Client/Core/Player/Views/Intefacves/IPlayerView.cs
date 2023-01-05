using TonPlay.Roguelike.Client.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Player.Views.Intefacves
{
	public interface IPlayerView : IPositionProvider
	{
		Rigidbody2D Rigidbody2D { get; }
	}
}