using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestMergeData : QuestBattleData
    {
        [JsonMember, JsonName("UnitType")] 
        public UnitType UnitType { get; protected set; } 
        
        [JsonMember, JsonName("UnitStars")] 
        public int UnitStars { get; protected set; }

        public override string Localize()
        {
            var starsTitle = UnitStars > -1 ? $" {UnitStars} {Strings.StarTitle}" : "";
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            var countTitle = Count > 1 ? $" {Count} {Strings.TimesTitle}" : "";
            var unitType = UnitType == UnitType.Random ? $"{Strings.AnyTitle.ToLower()} {Strings.MarinesTitle}" : UnitType.Localized();

            return $"{Strings.MergeTitle}{starsTitle} {unitType}{countTitle} {inOneBattle}";
        }
    }
}