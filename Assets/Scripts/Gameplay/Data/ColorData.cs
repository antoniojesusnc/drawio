using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Data/Color", order = 1)]
public class ColorData : ScriptableObject
{
	public List<Color> 		m_Colors;
}
