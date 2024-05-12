using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "Data/Pattern", order = 1)]
public class PatternData : ScriptableObject
{
    public int 				m_MinLevel = -1;
    public int 				m_MaxLevel = -1;
    public GameObject 		m_Prefab;
}