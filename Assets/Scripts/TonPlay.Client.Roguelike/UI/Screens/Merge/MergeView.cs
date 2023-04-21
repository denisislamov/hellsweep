using System.Collections.Generic;
using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    public class MergeView : View, IMergeView
    {
        [SerializeField] 
        private InventorySlotView[] _slots;
    
        [SerializeField]
        private ButtonView _mergeButtonView;

        [SerializeField]
        private TMP_Text _descriptionHeaderText;
        
        [SerializeField]
        private TMP_Text _descriptionInfoText;
		
        [SerializeField]
        private TMP_Text _descriptionValuesText;
    
        public IInventorySlotView[] Slots => _slots;
        public IButtonView MergeButtonView => _mergeButtonView;

        public void SetDescriptionHeaderText(string text)
        {
            _descriptionHeaderText.text = text;
        }

        public void SetDescriptionInfoText(string text)
        {
            _descriptionInfoText.text = text;
        }

        public void SetDescriptionValuesText(string text)
        {
            _descriptionValuesText.text = text;
        }
    }
}