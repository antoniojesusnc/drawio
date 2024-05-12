using UnityEngine;

public class Pattern : MonoBehaviour
{
    public bool             m_CanRot = false;
    public float            m_MinRotSpeed = 1.0f;
    public float            m_MaxRotSpeed = 10.0f;
    public bool             m_CanMove = false;
    public AnimationCurve   m_Curve;
    public float            m_MinY = -1.0f;
    public float            m_MaxY = 1.0f;

    private float           m_RRotSpeed;
    private float           m_RMinY;
    private float           m_RMaxY;

    // Cache
    private Transform       m_Transform;
    private StatsManager    m_StatsManager;

    // Buffers
    private Vector3         m_StartPos;
    private Vector3         m_EndPos;

    void Awake ()
    {
        // Cache
        m_Transform = transform;
        m_StatsManager = StatsManager.Instance;

        // Buffers
        m_StartPos = m_Transform.position;
        m_StartPos.y += m_RMaxY;
        m_EndPos = m_Transform.position;
        m_EndPos.y += m_RMinY;
    }

    void OnEnable ()
    {
        float lvlPercent = ((float) m_StatsManager.GetLevel()) / ((float) Constants.c_MaxLevel);
        m_RRotSpeed = Mathf.Lerp(m_MinRotSpeed, m_MaxRotSpeed, lvlPercent);
        m_RMinY = Mathf.Lerp(0.0f, m_MinY, lvlPercent);
        m_RMaxY = Mathf.Lerp(0.0f, m_MaxY, lvlPercent);
    }
	
	void Update ()
    {
        if (m_CanRot)
            m_Transform.RotateAround(m_Transform.position, Vector3.forward, m_RRotSpeed * Time.deltaTime);

    }
}
