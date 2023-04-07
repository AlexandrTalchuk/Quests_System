using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace MergeMarines.UI
{
    public class QuestsWindow : Window
    {
        [SerializeField]
        private TextMeshProUGUI _timerLabel = null;
        [SerializeField]
        private ScrollRect _scroll = null;
        [SerializeField]
        private UIQuestsItem _questsItemPrefab = null;
        [SerializeField]
        private Slider _slider = null;

        [Header("Buttons")]
        [SerializeField]
        private Button _dailyButton = null;
        [SerializeField]
        private Button _weeklyButton = null;
        [SerializeField]
        private TextMeshProUGUI _weeklyButtonText = null;
        [SerializeField]
        private TextMeshProUGUI _dailyButtonText = null;
        [SerializeField]
        private UINotificationBadge _weeklyBadge = null;
        [SerializeField]
        private UINotificationBadge _dailyBadge = null;
        
        private readonly List<UIQuestsItem> _questItems = new List<UIQuestsItem>();
        private Coroutine _timerCor = null;
        private UserQuests _userQuests;
        private QuestGroup _activeQuestGroup;

        public override bool IsPopup => false;
        
        protected override void Start()
        {
            base.Start();
            _dailyButton.onClick.AddListener(OnDailyButtonClicked);
            _weeklyButton.onClick.AddListener(OnWeeklyButtonClicked);
            SetupQuests(QuestGroup.Daily);
            Localize();
        }

        private void Localize()
        {
            _weeklyButtonText.text = Strings.WeeklyTitle;
            _dailyButtonText.text = Strings.DailyTitle;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            UIQuestsItem.Claimed += UIMissionItem_Claimed;
            UserQuests.TimerStarted += QuestProgressManager_TimerStarted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            UIQuestsItem.Claimed -= UIMissionItem_Claimed;
            UserQuests.TimerStarted -= QuestProgressManager_TimerStarted;
        }

        public override bool Preload()
        {
            _userQuests = User.Current.Quests;
            
            return base.Preload();
        }
        
        private void Update()
        {
            var time = _activeQuestGroup == QuestGroup.Daily ? _userQuests.GetNextDayTime() : _userQuests.GetNextWeekTime();
            
            _timerLabel.text = string.Format(Strings.TimeLeftFormat, TimeSpan.FromSeconds(time).FormatTimer());
        }

        protected override void Refresh()
        {
            base.Refresh();

            var missions = User.Current.Missions;

            _slider.value = Mathf.Max((float)missions.MissionPoints / missions.GetMilestoneData().Max(data => data.MilestonePoints).MilestonePoints, 0.05f);
            
            RefreshQuests(_activeQuestGroup);
            RefreshBadge(QuestGroup.Weekly, _weeklyBadge);
            RefreshBadge(QuestGroup.Daily, _dailyBadge);
        }

        private void SetupQuests(QuestGroup questGroup)
        {
            var quests = GetActiveQuests(questGroup);
            _activeQuestGroup = questGroup;

            foreach (var questId in quests)
            {
                var questItem = Instantiate(_questsItemPrefab, _scroll.content);
                _questItems.Add(questItem);

                questItem.gameObject.SetActive(true);
                questItem.Setup(QuestData.Get(questId), questGroup);
            }
        }
        
        private void RefreshQuests(QuestGroup questGroup)
        {
            var quests = GetActiveQuests(questGroup);

            for (int i = 0; i < quests.Length; i++)
            {
                if (i < _questItems.Count)
                {
                    _questItems[i].gameObject.SetActive(true);
                    _questItems[i].Setup(QuestData.Get(quests[i]), questGroup);
                }
            }
        }

        private string[] GetActiveQuests(QuestGroup questGroup)
        {
            string[] quests = questGroup == QuestGroup.Daily
                ? _userQuests.GetActiveDailyQuestsIds()
                : _userQuests.GetActiveWeeklyQuestsIds();
            return quests;
        }
        
        private void RefreshBadge(QuestGroup questGroup, UINotificationBadge badge)
        {
            var activeQuests = GetActiveQuests(questGroup);
            int count = 0;

            foreach (var questId in activeQuests)
            {
                var questData= QuestData.Get(questId);
                var canTakeReward = _userQuests.CanTakeReward(questData.QuestID);
                var isClaimed = _userQuests.IsQuestRewardClaimed(questData.QuestID, questGroup);
                
                if (canTakeReward && !isClaimed)
                {
                    count++;
                }
            }
            
            badge.Refresh(count, isRed: true, isNeedAnimation: true);
        }
        
        private void OnWeeklyButtonClicked()
        {
            _activeQuestGroup = QuestGroup.Weekly;

            RefreshQuests(_activeQuestGroup);
        }

        private void OnDailyButtonClicked()
        {
            _activeQuestGroup = QuestGroup.Daily;
            
            RefreshQuests(_activeQuestGroup);
        }
        
        private void UIMissionItem_Claimed()
        {
            Refresh();
        }
        
        private void QuestProgressManager_TimerStarted()
        {
            Refresh();
        }
    }
}