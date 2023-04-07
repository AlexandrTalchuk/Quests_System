using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestFightTimeData : QuestBattleData
    {
        public override string Localize()
        {
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            return string.Format(Strings.FightTheBattleTitle, Count, inOneBattle);
        }
    }
}