﻿using System;
using System.Collections.Generic;
using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItemView : MonoBehaviour
{
    private const string DAY_TEXT_FORMAT = "DAY {0}";
    
    private DailyRewardConfigData _dailyRewardConfigData;

    [SerializeField] 
    private TextMeshProUGUI _dayText;
    [SerializeField] 
    private TextMeshProUGUI _rewardText;
    [SerializeField] 
    private List<Image> _rewardImages;
    [SerializeField] 
    private GameObject _claimedObject;

    public bool IsClaimed { get; private set; }
    
    private void Awake()
    {
        _claimedObject.gameObject.SetActive(false);
    }

    public void SetConfig(DailyRewardConfigData dailyRewardConfigData)
    {
        _dailyRewardConfigData = dailyRewardConfigData;

        Signals.Get<OnClaimDailyRewardEvent>().AddListener(OnClaimDailyReward);
        SetData();
    }

    private void OnClaimDailyReward(DailyRewardConfigData configData)
    {
        if (configData == _dailyRewardConfigData)
        {
            SetAsClaimed(true);
        }
    }

    private void SetData()
    {
        _dayText.text = string.Format(DAY_TEXT_FORMAT, _dailyRewardConfigData.Day);
        _rewardText.text = _dailyRewardConfigData.Reward.ToString();

        for (int i = 0; i < _rewardImages.Count; i++)
        {
            if (_dailyRewardConfigData.GraphicsToShow.Count > i)
            {
                _rewardImages[i].gameObject.SetActive(true);
                _rewardImages[i].sprite = _dailyRewardConfigData.GraphicsToShow[i];
            }
            else
            {
                _rewardImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetAsClaimed(bool isClaimed)
    {
        IsClaimed = isClaimed;
        _claimedObject.SetActive(isClaimed);
    }
}
