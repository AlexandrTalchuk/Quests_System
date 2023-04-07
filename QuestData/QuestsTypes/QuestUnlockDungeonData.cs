using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestUnlockDungeonData : QuestData
    {
        public override string Localize()
        {
            var dungeonTitle = Count > 1 ? Strings.DungeonsTitle : Strings.DungeonTitle;
            return string.Format(Strings.UnlockDungeonTitle, Count, dungeonTitle);
        }
    }
}