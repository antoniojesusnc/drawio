using System.Collections.Generic;
using deVoid.Utils;
using DG.Tweening;
using UnityEditorInternal;
using UnityEngine;

public class DailyRewardView : View<DailyRewardView>
{
    [SerializeField] private Transform _scrollParent;

    private DailyRewardConfig _config;

    private bool _claimed = false;

    private List<DailyRewardItemView> _itemRewardList = new List<DailyRewardItemView>();
        
    void Start()
    {
        _config = DailyRewardManager.Instance.DailyRewardConfig;

        GenerateDailyRewards();
    }

    private void GenerateDailyRewards()
    {
        for (int i = 0; i < _config.DailyReward.Count; i++)
        {
            var rewardItemView = GetRewardItemView();
            rewardItemView.SetConfig(_config.DailyReward[i]);
            rewardItemView.SetAsClaimed(DailyRewardManager.Instance.IsClaimed(_config.DailyReward[i]));
            _itemRewardList.Add(rewardItemView);
        }

        foreach (Transform child in _scrollParent)
        {
            child.SetAsFirstSibling();
        }
    }

    private DailyRewardItemView GetRewardItemView()
    {
        return Instantiate(_config.DailyRewardItemPrefab, _scrollParent);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.DAILY_REWARD:
                Transition(true);
                break;
            case GamePhase.MAIN_MENU:
                if (m_Group.alpha > 0)
                {
                    Transition(false);
                }
                break;
        }
    }

    public void OnClickClaim()
    {
        if (!_claimed)
        {
            var firstUnclaimedPosition = GetPositionOfFirstUnclaimed();
            DailyRewardManager.Instance.Claim(firstUnclaimedPosition);
            _claimed = true;
        }
        
        Signals.Get<OnCoinAnimationFinishedEvent>().AddListener(OnCoinAnimationFinished);
    }

    private void OnCoinAnimationFinished()
    {
        Signals.Get<OnCoinAnimationFinishedEvent>().RemoveListener(OnCoinAnimationFinished);
        Close();
    }

    private Vector3 GetPositionOfFirstUnclaimed()
    {
        for (int i = 0; i < _itemRewardList.Count; i++)
        {
            if (!_itemRewardList[i].IsClaimed)
            {
                return _itemRewardList[i].transform.position;
            }
        }

        return Vector3.zero;
    }

    private void Close()
    {
        GameManager.Instance.ChangePhase(GamePhase.MAIN_MENU);
    }
}