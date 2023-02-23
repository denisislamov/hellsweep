using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(SkillConfigProvider), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(SkillConfigProvider))]
	public class SkillConfigProvider : ScriptableObject, ISkillConfigProvider
	{
		[SerializeField]
		private SkillConfig[] _skillConfigs;

		[SerializeField]
		private SkillPresentationConfig _presentationConfig;

		private Dictionary<SkillName, SkillConfig> _map;
		private Dictionary<SkillName, SkillConfig> Map => _map ??= _skillConfigs.ToDictionary(_ => _.SkillName, _ => _);

		public IEnumerable<ISkillConfig> All => _skillConfigs;
		public ISkillPresentationConfig PresentationConfig => _presentationConfig;

		public ISkillConfig Get(SkillName skillName)
		{
			return Map[skillName];
		}
	}
}