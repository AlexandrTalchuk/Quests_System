using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestDifficultyData
    {
        [JsonMember, JsonName("Difficulty")]
        public int Difficulty
        {
            get;
            private set;
        }

        [JsonMember, JsonName("Count")]
        public int Count
        {
            get;
            private set;
        }

        [JsonMember, JsonName(nameof(Reward))]
        public RewardItem Reward { get; protected set; }
    }
}