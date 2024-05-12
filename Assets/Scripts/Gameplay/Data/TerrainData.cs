using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Terrain", menuName = "Data/Terrain", order = 1)]
public class TerrainData : ScriptableObject
{
	public TerrainController m_Prefab;
}
