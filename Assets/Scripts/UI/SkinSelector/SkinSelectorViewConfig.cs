using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkinSelectorViewConfig", menuName = "Data/UI/SkinSelectorViewConfig", order = 1)]
public class SkinSelectorViewConfig : ScriptableObject
{
    [field: SerializeField]
    public int ElementsPerRow { get; private set; }
    
    [field: SerializeField]
    public SkinSelectorItemView SkinSelectorItemViewPrefab { get; private set; }
    
    [FormerlySerializedAs("SkinSelectorItem")] [field: SerializeField]
    public List<SkinData> SkinSelectorItems = new List<SkinData>();
}
