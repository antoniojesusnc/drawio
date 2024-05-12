using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Data/PowerUp", order = 1)]
public class PowerUpData : ScriptableObject
{
	public int 			m_Probability = 1;
	public GameObject 	m_Prefab;
}
