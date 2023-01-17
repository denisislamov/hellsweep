using System;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Movement
{
	[Serializable]
	public class MovementConfig : IMovementConfig
	{
		[SerializeField]
		private float _startSpeed;
		
		[SerializeField]
		private float _acceleration;

		public float StartSpeed => _startSpeed;
		public float Acceleration => _acceleration;
	}
}