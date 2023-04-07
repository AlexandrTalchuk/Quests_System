using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestKillData : QuestBattleData
    {
        [JsonMember, JsonName("Ultimate")] 
        public bool Ultimate { get; protected set; }

        [JsonMember, JsonName("UnitType")] 
        public UnitType UnitType { get; protected set; } 
        
        [JsonMember, JsonName("UnitStars")] 
        public int UnitStars { get; protected set; }
        
        [JsonMember, JsonName("EnemyType")] 
        public EnemyType EnemyType { get; protected set; }
        
        [JsonMember, JsonName("EnemyStars")] 
        public int EnemyStars { get; protected set; }
        
        [JsonMember, JsonName("IsBoss")] 
        public bool IsBoss { get; protected set; }

        public override string Localize()
        {
            var timesTitle = $"{Count} {Strings.TimesTitle}";
            var isBoss = IsBoss ? Strings.BattleBossName : "";
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            var isUltimate = Ultimate ? $"{ Strings.UltimateUnitAbility.ToLower()}" : "";
            var unitStarsTitle = UnitStars > -1 ? $"{UnitStars} {Strings.StarTitle} " : "";
            var enemyStarsTitle = EnemyStars > -1 ? $" {EnemyStars} {Strings.StarTitle} " : "";

            var enemyTitle = EnemyType != EnemyType.None
                ? $"{Strings.TheTitle}{enemyStarsTitle}{EnemyType.Localized()}"
                : $"{enemyStarsTitle}{Strings.AnyTitle.ToLower()} {Strings.EnemyTitle.ToLower()}";

            var unitTitle = UnitType == UnitType.Random
                ? $"{unitStarsTitle}{isUltimate}"
                : $"{UnitType.Localized()}{isUltimate}";

            var usingTitle = $"{Strings.UsingTitle} {unitTitle}";
            
            return string.Format(Strings.KillDamageTitle, enemyTitle, isBoss,
                usingTitle, timesTitle, inOneBattle);
        }
    }
}