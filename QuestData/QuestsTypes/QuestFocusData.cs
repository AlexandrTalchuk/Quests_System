using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestFocusData : QuestBattleData
    {
        [JsonMember, JsonName("EnemyType")] 
        public EnemyType EnemyType { get; protected set; }

        [JsonMember, JsonName("EnemyStars")] 
        public int EnemyStars { get; protected set; }

        public override string Localize()
        {
            var starsTitle = EnemyStars > -1 ? $" {EnemyStars} {Strings.StarTitle}" : "";
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            var countTitle = Count > 1 ? $" {Count} {Strings.TimesTitle}" : "";
            
            var enemyTitle = EnemyType != EnemyType.None
                ? $"{Strings.TheTitle}{starsTitle} {EnemyType.Localized()}{countTitle}"
                : $"{Strings.AnyTitle.ToLower()}{starsTitle} {Strings.EnemyTitle.ToLower()}{countTitle}";

            return string.Format(Strings.FocusEnemyTitle, enemyTitle, inOneBattle);
        }
    }
}