using System;
using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Skills;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface ISkillsModel
	{
		IReadOnlyDictionary<SkillName, int> SkillLevels { get; }
		
		IReadOnlyReactiveProperty<int> Level { get; }

		IObservable<Unit> Updated { get; }

		void Update(SkillsData data);
		
		SkillsData ToData();
	}
}