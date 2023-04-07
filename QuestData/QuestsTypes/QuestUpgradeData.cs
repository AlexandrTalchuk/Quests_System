using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestUpgradeData : QuestData
    {
        [JsonMember, JsonName("Ultimate")] 
        public bool Ultimate { get; protected set; }

        [JsonMember, JsonName("UnitType")] 
        public UnitType UnitType { get; protected set; } 
        
        [JsonMember, JsonName("UnitLevel")] 
        public int UnitLevel { get; protected set; }

        public override string Localize()
        {
            var countTitle = Count > 1 ? $"{Count} {Strings.TimesTitle}" : "";
            var toLevel = UnitLevel > -1 ? $"{string.Format(Strings.ToLevelTitle, UnitLevel)} " : "";
            var levelAndCountTitle = $"{toLevel}{countTitle}";
            var unitTitle = UnitType == UnitType.Random ? $"{Strings.AnyTitle.ToLower()} {Strings.MarinesTitle}" : $"{Strings.TheTitle} {UnitType.Localized()}";
            return string.Format(Strings.UpgradeUnitTitle, unitTitle, levelAndCountTitle);
        }
    }
}