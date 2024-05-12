using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "Data/Config/DailyRewardConfig", order = 1)]
public class DailyRewardConfig : ScriptableObject
{
    [field: SerializeField] 
    public float AnimationDuration { get; private set; }
    
    [field: SerializeField] 
    public DailyRewardItemView DailyRewardItemPrefab { get; private set; }
    
    [field: SerializeField]
    public List<DailyRewardConfigData> DailyReward = new List<DailyRewardConfigData>();
}

[Serializable]
public class DailyRewardConfigData
{
    [field: SerializeField]
    public int Day { get; private set; }
    
    [field: SerializeField]
    public int Reward { get; private set; }
    
    [field: SerializeField]
    public List<Sprite> GraphicsToShow { get; private set; }
}
