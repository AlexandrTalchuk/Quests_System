using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestCollectData : QuestData
    {
        [JsonMember, JsonName("Currency")] 
        public CurrencyType Currency { get; protected set; }

        public override string Localize()
        {
            var countTitle = Count > 1 ? $"{Count} {Strings.TimesTitle}" : "";

            return string.Format(Strings.CollectCurrencyTitle, Currency.LocalizedCurrency(), countTitle);
        }
    }
}