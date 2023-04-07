using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MergeMarines
{
    public class UserQuests : IUserQuestsInternal
    {
        public static event Action TimerStarted = delegate { };
        private const int WeekDaysCount = 7;

        private int _timer;
        private int _weeklyTimer;
        private int _dailyTimer;
        private int _weekDaysCount;
        private CancellationTokenSource _cancellationTokenSource;

        [JsonMember, JsonName("ClaimedDaily")]
        private List<string> _claimedDailyQuests = new List<string>();

        [JsonMember, JsonName("ClaimedWeekly")]
        private List<string> _claimedWeeklyQuests = new List<string>();

        [JsonMember, JsonName("QuestsProgress")]
        private Dictionary<string, int> _questsProgress = new Dictionary<string, int>();

        [JsonMember, JsonName("StartDayTime")]
        public DateTime StartDayTime { get; private set; }

        [JsonMember, JsonName("StartWeekTime")]
        public DateTime StartWeekTime { get; private set; }

        [JsonMember, JsonName("CurrentDay")] 
        public int CurrentDay { get; private set; }

        [JsonMember, JsonName("CurrentWeek")] 
        public int CurrentWeek { get; private set; }

        private IUserQuestsInternal Internal => this;

        public UserQuests()
        {
            Internal.Reset();
        }

        public async UniTaskVoid StartTimer(Action callback)
        {
            StopTimer();
            TimerStarted();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _timer = 0;
            _dailyTimer = GetNextDayTime();
            _weeklyTimer = GetNextWeekTime();

            while (_weeklyTimer > _timer)
            {
                if (_dailyTimer < _timer)
                {
                    callback.Invoke();
                }

                await UniTask.Delay(1000, cancellationToken: token);

                _weeklyTimer--;
                _dailyTimer--;
            }

            callback?.Invoke();
        }

        private void StopTimer() 
        {
            if (_cancellationTokenSource == null)
                return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void UpdateQuestsRotation()
        {
            UpdateTimers();

            if (CurrentDay % WeekDaysCount == 0)
            {
                GetActiveWeeklyQuestsIds().Do(q =>
                {
                    if (_questsProgress.ContainsKey(q))
                        _questsProgress.Remove(q);
                });
            }

            GetActiveDailyQuestsIds().Do(q =>
            {
                if (_questsProgress.ContainsKey(q))
                    _questsProgress.Remove(q);
            });

            _claimedDailyQuests.Clear();
            _claimedWeeklyQuests.Clear();
        }

        private void UpdateTimers()
        {
            CurrentDay++;
            _weekDaysCount++;

            StartDayTime = UpdateUTCTime();
            StartWeekTime = UpdateUTCTime();
            StartWeekTime = StartWeekTime.AddDays(-_weekDaysCount);

            _dailyTimer = GetNextDayTime();
            _weeklyTimer = GetNextWeekTime();

            if (CurrentDay > 1 && CurrentDay % WeekDaysCount == 0)
            {
                CurrentWeek++;
                _weekDaysCount = WeekDaysCount;
                StartWeekTime = UpdateUTCTime();
            }
        }

        private DateTime UpdateUTCTime()
        {
            DateTime utcNow = UnbiasedTime.Instance.Now().ToUniversalTime();
            DateTime todayUtcUpdate =
                new DateTime(utcNow.Year, utcNow.Month, utcNow.Day) + UserData.Data.AdsEnergyUpdateTime;

            return todayUtcUpdate;
        }

        public int GetNextDayTime()
        {
            int secondsInDay = 24 * 60 * 60;

            return (secondsInDay - (UnbiasedTime.Instance.Now() - StartDayTime).TotalSeconds).RoundArithmeticToInt();
        }

        public int GetNextWeekTime()
        {
            int secondsInDay = 24 * 60 * 60;

            return (7 * secondsInDay - (UnbiasedTime.Instance.Now() - StartWeekTime).TotalSeconds).RoundArithmeticToInt();
        }

        public string[] GetActiveDailyQuestsIds()
        {
            return QuestsRotationData.Data.DailyPacks[CurrentDay].Quests;
        }

        public string[] GetActiveWeeklyQuestsIds()
        {
            return QuestsRotationData.Data.WeeklyPacks[CurrentWeek].Quests;
        }

        public string[] GetAllActiveQuestsIds()
        {
            return GetActiveDailyQuestsIds().Union(GetActiveWeeklyQuestsIds()).ToArray();
        }

        public int GetQuestProgress(string id)
        {
            if (_questsProgress.TryGetValue(id, out int progress))
            {
                return progress;
            }

            return 0;
        }

        public bool CanTakeReward(string id)
        {
            bool canTake = false;
            if (_questsProgress.TryGetValue(id, out int progress))
            {
                return progress >= QuestData.Get(id).Count;
            }

            return canTake;
        }

        public bool IsQuestRewardClaimed(string id, QuestGroup group)
        {
            var claimedSource = group == QuestGroup.Daily ? _claimedDailyQuests : _claimedWeeklyQuests;
            return claimedSource.Contains(id);
        }

        void IUserQuestsInternal.Reset()
        {
            CurrentDay = 0;
            CurrentWeek = 0;
            _weekDaysCount = 0;
            StartDayTime = UpdateUTCTime();
            StartWeekTime = UpdateUTCTime();
        }

        void IUserQuestsInternal.ClaimQuestReward(string id, QuestGroup group)
        {
            if (group == QuestGroup.Daily)
                _claimedDailyQuests.Add(id);
            else if (group == QuestGroup.Weekly)
                _claimedWeeklyQuests.Add(id);

            new Reward(Reward.ReasonType.Quest, new RewardItem[] { QuestData.Get(id).Reward });
        }

        void IUserQuestsInternal.AdjustProgress(string id, int count)
        {
            if (!_questsProgress.TryGetValue(id, out int _))
            {
                _questsProgress.Add(id, 0);
            }

            _questsProgress[id] += count;
        }

        void IUserQuestsInternal.CancelProgress(string id)
        {
            if (_questsProgress.TryGetValue(id, out int _))
            {
                _questsProgress[id] = 0;
            }
        }

#if UNITY_EDITOR || FORCE_DEBUG_MENU

        public void ResetRewards()
        {
            _claimedDailyQuests.Clear();
        }

        public void SkipTotalTime(int seconds)
        {
            _dailyTimer -= seconds;
            _weeklyTimer -= seconds;
            StartWeekTime = StartWeekTime.AddSeconds(-seconds);

            if (GetNextWeekTime() < 0)
            {
                StartWeekTime = UpdateUTCTime();
                StartDayTime = UpdateUTCTime();
            }
        }
#endif
    }
}