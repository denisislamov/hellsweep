using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationBlockView
	{
		void SetPosition(Vector2 position);

		void SetText(string text);
	}
}