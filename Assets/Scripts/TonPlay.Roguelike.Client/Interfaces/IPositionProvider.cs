using UnityEngine;

namespace TonPlay.Roguelike.Client.Interfaces
{
	public interface IPositionProvider
	{
		Vector2 Position { get; }
	}
}