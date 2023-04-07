using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestsPackData
    {
        [JsonMember, JsonName(nameof(Quests))]
        public string[] Quests { get; private set; }
    }
}