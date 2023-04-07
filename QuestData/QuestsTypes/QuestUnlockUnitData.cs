using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestUnlockUnitData : QuestData
    {
        [JsonMember, JsonName("UnitType")] 
        public UnitType UnitType { get; protected set; } 
        
        [JsonMember, JsonName("UnlockAction")] 
        public UnlockActionTypes UnlockAction { get; protected set; }

        public override string Localize()
        {
            var unitTitle = UnitType == UnitType.Random
                ? $"{Count} {Strings.New} {Strings.MarinesTitle}"
                : UnitType.Localized();

            return string.Format(Strings.UnlockUnitTitle, UnlockAction.DescriptionLocalized(), unitTitle);
        }
        
    }
}