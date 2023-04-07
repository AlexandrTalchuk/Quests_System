using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace MergeMarines.UI
{
    public class UIQuestsItem : MonoBehaviour
    {
        public static event Action Claimed = delegate { };
        public static event Action<int, MissionType> GoClicked = delegate { };

        [Header("Buttons")]
        [SerializeField]
        private Button _goToButton = null;
        [SerializeField]
        private Button _claimButton = null;
        [SerializeField]
        private Button _questItemButton = null;
        
        [Space]
        [SerializeField]
        private Slider _progressSlider = null;
        [SerializeField]
        private TextMeshProUGUI _questProgressLabel = null;
        [SerializeField]
        private TextMeshProUGUI _questDescription = null;
        [SerializeField]
        private GameObject _unavailableGroup = null;
        [SerializeField]
        private GameObject _claimedGroup = null;

        private QuestData _data = null;
        private QuestGroup _questGroup;
        private bool _canClaimQuest;
        private bool _isQuestClaimed;

        private void Start()
        {
            _goToButton.onClick.AddListener(OnGoToButtonClick);
            _claimButton.onClick.AddListener(OnClaimButtonClick);
            _questItemButton.onClick.AddListener(OnQuestsItemClicked);
        }
        
        public void Setup(QuestData questData, QuestGroup questGroup)
        {
            _data = questData;
            _questGroup = questGroup;
            _questDescription.text = questData.Localize();
            _questProgressLabel.text = questData.Count.ToString();
           
            Refresh();
        }

        private void Refresh()
        {
            var quests = User.Current.Quests;

            _canClaimQuest = quests.CanTakeReward(_data.QuestID);
            _isQuestClaimed = quests.IsQuestRewardClaimed(_data.QuestID, _questGroup);
            int questMaxProgress = _data.Count;
            int questCurrentProgress = Mathf.Clamp(quests.GetQuestProgress(_data.QuestID), 0, questMaxProgress);

            _progressSlider.value = Mathf.Max(0.05f, questCurrentProgress / questMaxProgress);
            _questProgressLabel.text = $"{questCurrentProgress}/{questMaxProgress}";

            _claimButton.gameObject.SetActive(_canClaimQuest && !_isQuestClaimed);
            _claimedGroup.SetActive(_isQuestClaimed);
            //_goToButton.gameObject.SetActive(!canClaim && !isClaimed);
            _unavailableGroup.SetActive(!_isQuestClaimed && !_canClaimQuest);
        }

        private void OnGoToButtonClick()
        {
            // Action action = null;
            //
            // if (_data.Type == MissionType.CompleteStage)
            // {
            //     action = () =>
            //     {
            //         LocalConfig.ResetDungeons();
            //         UISystem.ShowWindow<MainWindow>();
            //     };
            // }
            //
            // action?.Invoke();
            //
            // GoClicked(_dayIndex, _data.Type);
        }
        
        private void OnClaimButtonClick()
        {
            UserManager.Instance.TryGetQuestReward(_data.QuestID, _questGroup,
                isSuccess =>
                {
                    if (isSuccess)
                    {
                        Claimed();
                    }
                });
        }

        private void OnQuestsItemClicked()
        {
            UISystem.Instance.DialogueSystem.Setup(_data.QuestID, _isQuestClaimed);
        }
    }
}