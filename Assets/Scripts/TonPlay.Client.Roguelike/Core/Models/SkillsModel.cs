using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
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

			if (data.Level != _level.Value)
			{
				_level.SetValueAndForceNotify(data.Level);
			}

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