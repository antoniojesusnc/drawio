using UnityEngine;

public class DailyRewardView : View<DailyRewardView>
{
    [SerializeField] 
    private Transform _scrollParent;
    
    private DailyRewardConfig _config;

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
        }
    }
}
