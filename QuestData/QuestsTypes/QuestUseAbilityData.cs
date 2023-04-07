using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestUseAbilityData : QuestBattleData
    {
        [JsonMember, JsonName("Ultimate")] 
        public bool Ultimate { get; protected set; }

        [JsonMember, JsonName("Ability")] 
        public string Ability { get; protected set; } 
        
        [JsonMember, JsonName("AbilityLevel")] 
        public int AbilityLevel { get; protected set; }

        public override string Localize()
        {
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            var levelTitle = AbilityLevel > -1 ? $"{string.Format(Strings.LevelTitle, AbilityLevel)} " : "";
            var countTitle = Count > 1 ? $"{Count} {Strings.TimesTitle}" : "";

            var countLevelTitle = $"{levelTitle}{countTitle}";
            string ability = "";
            
            foreach (var gameAbilityGroup in AbilityGroupData.AbilityGroups)
            {
                if (gameAbilityGroup.Abilities.TryGetValue(Ability, out AbilityData abilityData))
                {
                    ability = $"{Strings.TheTitle} {LocalizationExtentions.LocalizeAbilityData(abilityData)} {Strings.AbilityTitle.ToLower()}";
                }
                else if(Ability == "Random")
                {
                    ability = $"{Strings.AnyTitle.ToLower()} {Strings.AbilityTitle.ToLower()}";
                }
            }
            
            return string.Format(Strings.UseAbilityTitle, ability, countLevelTitle, inOneBattle);
        }
    }
}