using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushMenu : MonoBehaviour {

    public List<GameObject> m_BrushParts;

    public void SetNewColor(Color _Color)
    {
        for (int i = 0; i < m_BrushParts.Count; i++)
        {
            m_BrushParts[i].GetComponent<Renderer>().material.color = _Color;
        }
    }
}
