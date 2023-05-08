using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class ShopPackRewardsModel : IShopPackRewardsModel
	{
		private readonly ShopPackRewardsData _cached = new ShopPackRewardsData();
		
		private uint _energy;
		private ulong _coins;
		private uint _blueprints;
		private uint _keysCommon;
		private uint _keysUncommon;
		private uint _keysRare;
		private uint _keysLegendary;
		private uint _heroSkins;
		
		public uint Energy => _energy;
		public ulong Coins => _coins;
		public uint Blueprints => _blueprints;
		public uint KeysCommon => _keysCommon;
		public uint KeysUncommon => _keysUncommon;
		public uint KeysRare => _keysRare;
		public uint KeysLegendary => _keysLegendary;
		public uint HeroSkins => _heroSkins;
		
		public void Update(ShopPackRewardsData data)
		{
			_energy = data.Energy;
			_coins = data.Coins;
			_blueprints = data.Blueprints;
			_keysCommon = data.KeysCommon;
			_keysUncommon = data.KeysUncommon;
			_keysRare = data.KeysRare;
			_keysLegendary = data.KeysLegendary;
			_heroSkins = data.HeroSkins;
		}
		
		public ShopPackRewardsData ToData()
		{
			_cached.Energy = _energy;
			_cached.Coins = _energy;
			_cached.Blueprints = _blueprints;
			_cached.KeysCommon = _keysCommon;
			_cached.KeysUncommon = _keysUncommon;
			_cached.KeysRare = _keysRare;
			_cached.KeysLegendary = _keysLegendary;
			_cached.HeroSkins = _heroSkins;
			return _cached;
		}
	}
}