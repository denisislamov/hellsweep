using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views
{
	public class SkillChoiceView : View, ISkillChoiceView
	{
		[SerializeField]
		private SkillChoiceCollectionView _collectionView;
		
		public ISkillChoiceCollectionView CollectionView => _collectionView;
	}
}