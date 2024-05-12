using DG.Tweening;
using UnityEngine;

public class DailyRewardView : View<DailyRewardView>
{
    [SerializeField] private Transform _scrollParent;

    private DailyRewardConfig _config;

    private bool _claimed = false;

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
            DailyRewardManager.Instance.Claim();
            _claimed = true;
            ClaimAnimation();
        }

        DOVirtual.DelayedCall(_config.AnimationDuration, Close);
    }

    private void Close()
    {
        GameManager.Instance.ChangePhase(GamePhase.MAIN_MENU);
    }


    private void ClaimAnimation()
    {
    }
}