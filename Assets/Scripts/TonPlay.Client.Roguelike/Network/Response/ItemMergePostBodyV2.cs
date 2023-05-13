using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemMergePostBodyV2
    {
        public List<string>  userItemDetailIds= new List<string>();
    }
}