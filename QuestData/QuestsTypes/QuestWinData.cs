using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestWinData : QuestData
    {
        [JsonMember, JsonName("HPLess")]
        public int HPLess { get; protected set; }

        [JsonMember, JsonName("HPMore")]
        public int HPMore { get; protected set; }

        [JsonMember, JsonName("ReviveRequirement")] 
        public int ReviveRequirement { get; protected set; }

        [JsonMember, JsonName("InARow")]
        public bool InARow { get; protected set; }

        public override string Localize()
        {
            string barricadeHp;

            if (HPLess > -1)
            {
                barricadeHp = string.Format(Strings.BarricadeHP, '<', HPLess);
            }
            else if (HPMore > -1)
            {
                barricadeHp = string.Format(Strings.BarricadeHP, '>', HPMore);
            }
            else
            {
                barricadeHp = "";
            }

            var revive = ReviveRequirement > 0 ? Strings.WithRevive : Strings.WithoutRevive;
            var inARowTitle = InARow ? $"{Strings.InARow}" : "";

            return string.Format(Strings.WinBattleTitle, barricadeHp, revive, Count, inARowTitle);
        }
    }
}