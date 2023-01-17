using UnityEngine;

namespace TonPlay.Client.Roguelike.Interfaces
{
	public interface IPositionProvider
	{
		Vector2 Position { get; }
	}
}