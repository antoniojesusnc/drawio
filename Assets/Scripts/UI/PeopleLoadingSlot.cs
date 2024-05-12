using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum EPeopleSlotState
{
    Playing,
    Dead,
    Hidden
}

public class PeopleLoadingSlot : MonoBehaviour
{
    public Image m_Cross;
    public Image m_Icon;
    public Color m_DeadColor;
    public Color m_AliveColor;
    public EPeopleSlotState m_State;

    public void LoadPlayer()
    {
        m_Icon.DOFade(0, 0.125f).OnComplete(() =>
        {
            m_Icon.DOFade(1, 0.125f);
        });

        if (m_Cross)
            m_Cross.fillAmount = 0f;
    }

    public void Unload()
    {
        Color color = m_AliveColor;
        color.a = 0f;
        m_Icon.color = color;
        if (m_Cross)
            m_Cross.fillAmount = 0f;
    }

    public void SetState(EPeopleSlotState _State)
    {
        m_State = _State;
        switch (_State)
        {
            case EPeopleSlotState.Dead:
                m_Cross.DOFillAmount(1f, 0.2f).SetDelay(Random.Range(0f, 0.3f));
                m_Icon.DOColor(m_DeadColor, 0.3f).SetDelay(0.5f);
                break;
            case EPeopleSlotState.Playing:
                m_Icon.color = m_AliveColor;
                m_Cross.fillAmount = 0f;
                break;
            case EPeopleSlotState.Hidden:
                Unload();
                break;
        }
    }

    public void SetColor(Color _Color)
    {
        switch (m_State)
        {
            case EPeopleSlotState.Playing:
                m_Icon.color = _Color;
                break;
        }
    }
}
