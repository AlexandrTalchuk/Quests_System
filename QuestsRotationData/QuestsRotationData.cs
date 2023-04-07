using JsonFx.Json;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestsRotationData
    {
        public static QuestsRotationData Data
        {
            get;
            private set;
        }

        [JsonMember, JsonName(nameof(WeeklyPacks))]
        public QuestsPackData[] WeeklyPacks { get; private set; }

        [JsonMember, JsonName(nameof(DailyPacks))]
        public QuestsPackData[] DailyPacks { get; private set; }

        public static void Load(string json, bool hasTypeHint)
        {
            Load(json, hasTypeHint, false);
        }

        public static void Load(QuestsRotationData data)
        {
            LoadData(data);
        }

        private static void Load(string json, bool hasTypeHint, bool isAlt)
        {
            LoadData(GameBalanceUtility.Parse<QuestsRotationData>(json, hasTypeHint));
        }

        private static void LoadData(QuestsRotationData data)
        {
            Data = data;
        }
    }
}