using JsonFx.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MergeMarines
{
    [JsonOptIn]
    public partial class QuestData
    {
        private static Dictionary<string, QuestData> _data = new Dictionary<string, QuestData>();

        [JsonMember, JsonName("Type")] 
        public QuestType Type { get; protected set; }

        [JsonMember, JsonName("QuestID")] 
        public string QuestID { get; protected set; }

        [JsonMember, JsonName("BeginningOfProgress")]
        public string BeginningOfProgress { get; protected set; }
        
        [JsonMember, JsonName("Count")] 
        public int Count { get; protected set; }

        [JsonMember, JsonName("Reward")] 
        public RewardItem Reward { get; protected set; }
        
        public static QuestData Get(string questId)
        {
            if (!_data.TryGetValue(questId, out QuestData questData))
            {
#if UNITY_EDITOR
                Debug.LogException(new Exception($"Not found quest for {questId}"));
#endif
                return null;
            }

            return questData;
        }

        public virtual string Localize()
        {
            throw new Exception($"Quests class localization dosent set");
        }

        public static void Load(string json, bool hasTypeHint)
        {
            Load(json, hasTypeHint, false);
        }

        public static void Load(Dictionary<string, QuestData> data)
        {
            LoadData(data);
        }

        private static void Load(string json, bool hasTypeHint, bool isAlt)
        {
            LoadData(GameBalanceUtility.Parse<Dictionary<string, QuestData>>(json, hasTypeHint));
        }

        private static void LoadData(Dictionary<string, QuestData> data)
        {
            _data.Clear();
            _data = data;
        }
    }
    
}