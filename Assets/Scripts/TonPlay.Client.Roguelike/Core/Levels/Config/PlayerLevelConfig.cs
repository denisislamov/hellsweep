using System;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Levels.Config
{
	[Serializable]
	public class PlayerLevelConfig : IPlayerLevelConfig
	{
		[SerializeField]
		private int _experienceToNextLevel;

		public int ExperienceToNextLevel => _experienceToNextLevel;
	}
}