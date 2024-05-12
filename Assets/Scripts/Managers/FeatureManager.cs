using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : SingletonMB<FeatureManager>
{
    public bool BrushCollider { get; private set; } = true;
    public bool DailyReward { get; private set; } = true;
    public bool UseScreenSelector { get; private set; } = true;
    public bool UseStoreItems { get; private set; } = true;

    private void Awake()
    {
        
    }

    public void SetBrushCollider(bool brushCollider)
    {
        BrushCollider = brushCollider;
    }
    
    public void SetDailyReward(bool dailyReward)
    {
        DailyReward = dailyReward;
    }
    
    
    public void SetUseSkinSelector(bool useScreenSelector)
    {
        UseScreenSelector = useScreenSelector;
    }
    
    public void SetUseStoreItems(bool useStoreItems)
    {
        UseStoreItems = useStoreItems;
    }
}
