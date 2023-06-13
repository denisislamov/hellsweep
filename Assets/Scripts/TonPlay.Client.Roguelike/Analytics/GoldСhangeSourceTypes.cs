namespace TonPlay.Client.Roguelike.Analytics
{
    public enum GoldСhangeSourceTypes
    {
        ChapterChest, // ( когда игрок получает из сундука локаций)
        VictoryOrDefeatLocation, // когда игрок получает за прохождения локации или когда игрок продержался какое то время на локации и все равно получает монеты)
        LootBoxes,
        DailyQuests,
        LevelUp,
        Tournament,
        TonPacks
    }
}