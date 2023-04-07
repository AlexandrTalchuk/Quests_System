using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestLootData : QuestData
    {
        [JsonMember, JsonName("Currency")] 
        public CurrencyType Currency { get; protected set; }

        public override string Localize()
        {
            return string.Format(Strings.LootCurrencyTitle, Count, Currency.LocalizedCurrency());
        }
    }
}