using UnityEngine;
using System.Collections;

public class MainCamera : SingletonMB<MainCamera>
{
    private const float c_MinDistance = 150.0f;
    private const float c_MaxDistance = 300.0f;

    public Transform m_BasePos;
    public Transform m_EndPos;
    public Vector3 m_Offset;


    // Cache
    private Transform m_Transform;
    private GameManager m_GameManager;
    private HumanPlayer m_Target;

    // Buffers
    private bool m_IsPlaying = false;
    private Quaternion m_BaseRot;

    void Awake()
    {
        // Cache
        m_Transform = transform;
        
        // Buffers
        m_BaseRot = m_Transform.rotation;
        SetDistance(0.0f);
    }

    void Start()
    {
        m_GameManager = GameManager.Instance;
        m_GameManager.onGamePhaseChanged += OnGamePhaseChanged;
    }

    private void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        switch (_GamePhase)
        {
            case GamePhase.MAIN_MENU:
                m_Transform.rotation = m_BasePos.rotation;
                break;
            case GamePhase.END:
                m_IsPlaying = false;
                break;
        }
    }

    void Update()
    {
        GamePhase phase = m_GameManager.currentPhase;
        switch (phase)
        {
            case GamePhase.MAIN_MENU:
                m_Transform.position = Vector3.Lerp(m_Transform.position, m_BasePos.position, 0.1f);
                break;

            case GamePhase.GAME:
                Zoom();
                break;

            case GamePhase.PRE_END:
                Dezoom();
                break;

            case GamePhase.END:
                Dezoom();
                break;
        }
    }

    private void Dezoom()
    {
        m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, m_EndPos.rotation, 7f * Time.deltaTime);
        m_Transform.position = Vector3.Lerp(m_Transform.position, m_EndPos.position, 0.1f);
    }

    private void Zoom()
    {
        m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, m_BaseRot, 5f * Time.deltaTime);
        if (m_IsPlaying)
        {
            if (m_Target.isEliminated)
                m_Transform.position = Vector3.Lerp(m_Transform.position, m_Offset, Time.deltaTime);
            else
                m_Transform.position = Vector3.Lerp(m_Transform.position, m_Target.transform.position + m_Offset, 10.0f * Time.deltaTime);
        }
    }

    public void SetTarget(HumanPlayer _Target)
    {
        m_IsPlaying = true;
        m_Target = _Target;
    }

    public void SetDistance(float _Percent)
    {
        m_Offset = m_Offset.normalized * Mathf.Lerp(c_MinDistance, c_MaxDistance, _Percent);
    }
}
