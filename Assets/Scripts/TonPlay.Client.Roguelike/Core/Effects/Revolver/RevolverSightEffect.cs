using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Effects.Revolver
{
	public class RevolverSightEffect : EffectView
	{
		[SerializeField]
		private Transform _leftBorder;
		
		[SerializeField]
		private Transform _rightBorder;
		
		public void SetLeftBorderDirection(Vector2 direction)
		{
			_leftBorder.right = direction;
		}
		
		public void SetRightBorderDirection(Vector2 direction)
		{
			_rightBorder.right = direction;
		}
	}
}