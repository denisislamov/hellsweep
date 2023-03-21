using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Skills
{
	public static class RemoteSkillConverter
	{
		private static readonly Dictionary<string, SkillName> _map = new Dictionary<string, SkillName>()
		{
			["bf330b35-944e-4795-8ad1-2893954f75ce"] = SkillName.Revolver,
			["fdf5484d-eb77-415d-84e7-d3ecbd57be57"] = SkillName.Lightchaser,
			["1677bd2b-5644-45ac-9dc1-348264325112"] = SkillName.Bat,
			["b57c70c7-4ea5-48c7-a921-6747c755e742"] = SkillName.Crossbow,
			["1940b088-1997-45b7-b981-cc90ac066757"] = SkillName.Katana,
			["91ecf9d9-0b27-488e-be04-42b40b9e4ada"] = SkillName.Kunai,
			["2207c252-2b63-4f8d-9aaf-76ef088ba186"] = SkillName.Guardian,
			["d85cc2d7-6911-44a8-8146-ff5201759a7c"] = SkillName.SoccerBall,
			["e211ef47-874d-4b7c-9f06-046cdad69324"] = SkillName.DrillShot,
			["6484da9d-a912-4004-ad88-695b2e1e9715"] = SkillName.TypeADrone,
			["9775d8b8-ea70-4a6a-9c1e-22c7e71bebf0"] = SkillName.TypeBDrone,
			["9fb122cd-cae9-4f2f-8abd-fdde8e3be618"] = SkillName.LightningEmitter,
			["29aca413-cc96-4ad6-b610-844a6997ddf6"] = SkillName.Boomerang,
			["29b43ac7-e745-4980-8c41-5d3fd765925c"] = SkillName.Brick,
			["e7bcee96-0352-436d-b72d-2bdf87884d02"] = SkillName.Molotov,
			["5e1817ec-afc1-4a69-946b-34b5a10c78c1"] = SkillName.RPG,
			["73e27eee-a041-4500-b83e-9c90d996bdfb"] = SkillName.ForcefieldDevice,
			["cf9d628f-3736-4e36-9fe9-4ae51b2cbf5e"] = SkillName.Durian,
			["c448fc0d-5b18-41d3-b04f-39bd440abac1"] = SkillName.LaserLauncher,
			["41d7a577-50a6-45c0-a1b9-45d67994f0df"] = SkillName.AmmoThruster,
			["b0e55b3a-c0be-4283-9cc0-24dddfe5b6b0"] = SkillName.EnergyDrink,
			["199aff59-4ebf-4a91-a92b-18b84a5104ca"] = SkillName.HEFuel,
			["afb15740-5f0b-4497-9753-ca069e0033c6"] = SkillName.FitnessGuide,
			["7b3c75e8-aab4-491a-a660-611516c49567"] = SkillName.KogaNinjaScroll,
			["56df8ea5-58a9-4770-a07f-5724a1ca8c5e"] = SkillName.RoninOyoroi,
			["c5bc9286-1477-46dd-a14a-22448fbfeac0"] = SkillName.HIPowerMagnet,
			["df7846c3-c932-47ff-9684-46f27c977a1b"] = SkillName.SportShoes,
			["435e4fe5-be5d-4b22-a25e-aa328fa27d80"] = SkillName.GoldGain,
			["27ddf4ef-5942-4dec-90ce-ceb5f9330a39"] = SkillName.EnergyCube,
			["bf810773-57b2-4b2f-abb2-26dafa871edc"] = SkillName.ExoBracer,
			["00094bb9-4935-4448-afb1-9109ad9535e1"] = SkillName.HiPowerBullet,
			["3614242b-92c8-4225-a421-a7596242091d"] = SkillName.GoldPouch,
			["e43342a4-c44d-4db5-b8a1-f432683f63c9"] = SkillName.StickOfHam,
		};

		public static SkillName ConvertUdidToSkillName(string udid) => _map[udid];
	}
}