using JsonFx.Json;
using UnityEngine;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestBattleData : QuestData
    {
        [JsonMember, JsonName("IsOneGame")]
        public bool IsOneGame { get; protected set; }
    }
}
