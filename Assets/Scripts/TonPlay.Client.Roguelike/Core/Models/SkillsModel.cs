using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class SkillsModel : ISkillsModel
	{
		private readonly SkillsData _data = new SkillsData();
		private readonly Subject<Unit> _updated = new Subject<Unit>();

		private ReactiveProperty<int> _level = new ReactiveProperty<int>();
		private Dictionary<SkillName, int> _skillLevels = new Dictionary<SkillName, int>();

		public IReadOnlyDictionary<SkillName, int> SkillLevels => _skillLevels;
		public IReadOnlyReactiveProperty<int> Level => _level;
		public IObservable<Unit> Updated => _updated;
		
		public void Update(SkillsData data)
		{
			_skillLevels = data.SkillLevels;
			
			_updated.OnNext(Unit.Default);
		}
		
		public SkillsData ToData()
		{
			_data.SkillLevels = _skillLevels.ToDictionary(_ => _.Key, _ => _.Value);
			_data.Level = _level.Value;
			return _data;
		}
	}
}