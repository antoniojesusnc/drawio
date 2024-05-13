using System;
using deVoid.Utils;
using UnityEngine;

public class DailyRewardManager : SingletonMB<DailyRewardManager>
{
    [field: SerializeField] 
    public DailyRewardConfig DailyRewardConfig { get; private set; }

    private int _currentIndex;
    private DateTime _lastClaimed;
    
    void Start()
    {
        if (!FeatureManager.Instance.DailyReward)
        {
            return;
        }
        
        LoadLastClaimedData();
        CheckIfClaimReward();
        ShowIfDailyRewardView();
    }

    private void ShowIfDailyRewardView()
    {
        if (_lastClaimed.Date == DateTime.UtcNow.Date)
        {
            return;
        }
        
        if(_currentIndex <= DailyRewardConfig.DailyReward.Count)
        {
            GameManager.Instance.ChangePhase(GamePhase.DAILY_REWARD);
        }
    }

    private void LoadLastClaimedData()
    {
        var lastClaimedRaw = PlayerPrefs.GetString(Constants.c_DailyRewardLastClaimedDataKey, DateTime.MinValue.ToString());
        _lastClaimed = DateTime.Parse(lastClaimedRaw);
    }

    private void CheckIfClaimReward()
    {
        if (_lastClaimed.Date == DateTime.UtcNow.AddDays(-1).Date)
        {
            _currentIndex = PlayerPrefs.GetInt(Constants.c_DailyRewardLastClaimedIndexKey, 0);
        }
    }

    public void Claim(Vector3 originPosition)
    {
        var dailyRewardConfigData = DailyRewardConfig.DailyReward[_currentIndex];
        StatsManager.Instance.AddCoins(dailyRewardConfigData.Reward, originPosition);
        
        _currentIndex++;
        SaveClaimData();
        
        Signals.Get<OnClaimDailyRewardEvent>().Dispatch(dailyRewardConfigData);
    }

    private void SaveClaimData()
    {
        _lastClaimed = DateTime.UtcNow;
        PlayerPrefs.SetInt(Constants.c_DailyRewardLastClaimedIndexKey, _currentIndex);
        PlayerPrefs.SetString(Constants.c_DailyRewardLastClaimedDataKey, _lastClaimed.ToString());
    }

    public bool IsClaimed(DailyRewardConfigData dailyRewardConfigData)
    {
        return _currentIndex > dailyRewardConfigData.Day;
    }
}
