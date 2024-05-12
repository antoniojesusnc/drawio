using UnityEngine;

public class FixedWidthCamera : MonoBehaviour
{
    private const float         c_DesiredAspectRatio = 9f / 16f;

    public float                m_CameraHeight = 5.33f;

    private Camera              m_Camera;

    void Awake()
    {
        Refresh();
    }

#if UNITY_EDITOR

    void Update()
    {
        Refresh();
    }

#endif

    public void Refresh()
    {
        if (m_Camera == null)
            m_Camera = GetComponent<Camera>();

        float aspect = m_Camera.aspect;
        float ratio = c_DesiredAspectRatio / aspect;

        if (m_Camera.orthographic)
            m_Camera.orthographicSize = m_CameraHeight * ratio;
        else
            m_Camera.fieldOfView = m_CameraHeight * ratio;
    }
}
