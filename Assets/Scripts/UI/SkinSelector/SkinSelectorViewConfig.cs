using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinSelectorViewConfig", menuName = "Data/UI/SkinSelectorViewConfig", order = 1)]
public class SkinSelectorViewConfig : ScriptableObject
{
    [field: SerializeField]
    public int ElementsPerRow { get; private set; }
    
    [field: SerializeField]
    public SkinSelectorItemView SkinSelectorItemViewPrefab { get; private set; }
    
    [field: SerializeField]
    public List<SkinData> SkinSelectorItems { get; private set; } = new List<SkinData>();
}
