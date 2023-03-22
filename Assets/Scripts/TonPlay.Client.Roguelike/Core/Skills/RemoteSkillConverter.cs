using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Skills
{
	public static class RemoteSkillConverter
	{
		private static readonly Dictionary<string, SkillName> _map = new Dictionary<string, SkillName>()
		{
			["86f9c448-69ea-4d82-93b7-4de1875062ea"] = SkillName.Revolver,
			["60d949de-56ff-45c7-a535-5dbc15a0f363"] = SkillName.Lightchaser,
			["472ad864-c55a-44a9-9e65-dc7c7be38d23"] = SkillName.Bat,
			["a24629b1-dd12-4319-ad50-7f680f1a494d"] = SkillName.Crossbow,
			["06696f24-84bf-40c7-ae12-20323fef31b4"] = SkillName.Katana,
			["be01aec2-c945-4e27-87fd-914a510ea728"] = SkillName.Kunai,
			["8b66ebed-1817-4589-ab92-8d720edfe7c0"] = SkillName.Guardian,
			["6fa310cd-ea28-4765-a02a-55fbe84d484a"] = SkillName.SoccerBall,
			["b52f7c53-90f5-4078-9a28-a41db06f7064"] = SkillName.DrillShot,
			["2f3377aa-066f-423a-8224-47d21dc50192"] = SkillName.TypeADrone,
			["02bec516-20b9-4c54-a372-834f00f9a9c0"] = SkillName.LightningEmitter,
			["8760b007-2f71-4231-8f1e-09546cb261d2"] = SkillName.Boomerang,
			["a9b05242-cb45-4b5c-81dc-52271a05e0be"] = SkillName.Brick,
			["f1dd5ac8-ab6e-47dd-a878-33a89242a63e"] = SkillName.Molotov,
			["e759c42d-8914-4a5a-89b9-2705d456de8b"] = SkillName.RPG,
			["f44d9b7d-8dbe-4e5e-a352-375028bb57c8"] = SkillName.ForcefieldDevice,
			["1e8d38ec-072a-4e0b-9c41-fae6850177fa"] = SkillName.Durian,
			["d520955c-9b25-49c7-89d4-5901c1adc998"] = SkillName.LaserLauncher,
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

		public static SkillName ConvertUdidToSkillName(string udid)
		{
			if (_map.ContainsKey(udid))
			{
				return _map[udid];
			}
			
			return SkillName.Unknown;
		}
	}
}