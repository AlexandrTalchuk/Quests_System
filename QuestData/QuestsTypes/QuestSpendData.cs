using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestSpendData : QuestData
    {
        [JsonMember, JsonName("Currency")] 
        public CurrencyType Currency { get; protected set; }

        public override string Localize()
        {
            return string.Format(Strings.SpendCurrencyTitle, Count, Currency.LocalizedCurrency());
        }
    }
}