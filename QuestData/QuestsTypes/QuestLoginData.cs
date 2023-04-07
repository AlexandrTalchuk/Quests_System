using System;
using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestLoginData : QuestData
    {
        [JsonMember, JsonName("InARow")] 
        public bool InARow { get; protected set; }

        public override string Localize()
        {
            var countTitle = Count > 1 ? $" {Count} {Strings.TimesTitle}" : "";
            var inARowTitle = InARow ? $"{Strings.InARow}" : "";

            return $"{Strings.LoginTitle}{countTitle} {inARowTitle}";
        }
    }
}