using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rank", menuName = "Data/Rank", order = 1)]
public class RankData : ScriptableObject
{
    public Sprite m_Icon;
    public string m_RankName;
}
