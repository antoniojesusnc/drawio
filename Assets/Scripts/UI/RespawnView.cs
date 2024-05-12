using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnView : View<RespawnView> {
    
    public Text m_CountDownText;
    public Image m_Fill;

    public bool m_IsVisible
    {
        get
        {
            return m_Visible;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        if (_GamePhase != GamePhase.GAME)
            Transition(false);
    }

    public void UpdateCountdown(float _Time, float _Fill, string _Message = "{0}")
    {
        if (m_CountDownText)
            m_CountDownText.text = string.Format(_Message, _Time.ToString());

        if (m_Fill)
            m_Fill.fillAmount = _Fill;
    }
}
