using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushMainMenu : MonoBehaviour {

    public Transform m_BrushParent;

    public Transform m_Current;

    public void Set(SkinData _skin)
    {
        if (m_Current != null)
            GameObject.Destroy(m_Current.gameObject);

        Brush brush = Instantiate(_skin.Brush.m_Prefab).GetComponent<Brush>();
        m_Current = brush.transform;
        m_Current.SetParent(m_BrushParent);
        m_Current.localScale = Vector3.one;
        m_Current.localPosition = Vector3.zero;
        m_Current.localRotation = Quaternion.identity;
        foreach (Renderer renderer in brush.m_Renderers)
        {
            renderer.material.color = _skin.Color.m_Colors[0];
        }
    }
}
