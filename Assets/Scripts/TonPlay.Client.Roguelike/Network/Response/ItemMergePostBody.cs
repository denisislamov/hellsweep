using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemMergePostBody
    {
        public List<string> itemDetailIds;
    }
}