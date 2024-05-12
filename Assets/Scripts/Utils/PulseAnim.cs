using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PulseAnim : MonoBehaviour {

    [Header("First Pulse")]
    public bool m_HasFirstPulse;
    public float m_FirstPulseScale;
    public float m_FirstPulseDuration;

    public Ease m_PulseEase;
    public float m_PulseScale;
    public float m_PulseDuration;
    public float m_BetweenPulseDelay;

    private Transform m_Transform;
    private Sequence m_PulseSequence;

	// Use this for initialization
	void Start () {

        m_Transform = this.transform;

        if (m_HasFirstPulse)
        {
            m_PulseSequence = DOTween.Sequence();
            m_PulseSequence.Append(m_Transform.DOPunchScale(Vector3.one * m_FirstPulseScale, m_FirstPulseDuration, 0, 0).SetEase(m_PulseEase));
            m_PulseSequence.AppendInterval(m_BetweenPulseDelay);
            m_PulseSequence.AppendCallback(SetupSequence);
        }
        else
            SetupSequence();
	}

    void SetupSequence()
    {
        m_PulseSequence = DOTween.Sequence().SetLoops(-1);
        m_PulseSequence.Append(m_Transform.DOPunchScale(Vector3.one * m_PulseScale, m_PulseDuration, 0, 0).SetEase(m_PulseEase));
        m_PulseSequence.AppendInterval(m_BetweenPulseDelay);
    }
	
}
