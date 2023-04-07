using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestKillWaveData : QuestBattleData
    {        
        [JsonMember, JsonName("EnemyType")] 
        public EnemyType EnemyType { get; protected set; }
        
        [JsonMember, JsonName("WaveStars")] 
        public int WaveStars { get; protected set; }
        
        [JsonMember, JsonName("IsBoss")] 
        public bool IsBoss { get; protected set; }
       
        public override string Localize()
        {
            var isBoss = IsBoss ? $" {Strings.BattleBossName.ToLower()}" : "";
            var countTitle = $"{Count}{isBoss}";
            var starsTitle = WaveStars > -1 ? string.Format(Strings.WithStarsTitle, WaveStars) : "";
            var inOneBattle = IsOneGame ? Strings.InOneBattleTitle : "";
            
            return string.Format(Strings.KillWavesTitle, countTitle, starsTitle, inOneBattle);
        }
    }
}