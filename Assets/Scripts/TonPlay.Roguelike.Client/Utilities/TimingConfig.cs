using System;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Utilities
{
	[Serializable]
	public class TimingConfig
	{
		[Range(0, 60), SerializeField]
		private int _minutes;
		
		[Range(0, 60), SerializeField]
		private int _seconds;

		public TimeSpan GetTimeSpan()
		{
			return TimeSpan.FromMinutes(_minutes) + TimeSpan.FromSeconds(_seconds);
		}
	}
}