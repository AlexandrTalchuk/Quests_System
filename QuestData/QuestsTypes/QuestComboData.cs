using JsonFx.Json;
using UnityEngine;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestComboData : QuestBattleData
    {
        [JsonMember, JsonName("ComboCount")]
        public int ComboCount { get; protected set; }

        public override string Localize()
        {
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            var countTitle = Count > 1 ? $"{Count} {Strings.TimesTitle}" : "";

            return string.Format(Strings.CombosTitle, ComboCount, countTitle, inOneBattle);
        }
    }
}