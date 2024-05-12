using UnityEditor;

[CustomEditor(typeof(FixedWidthCamera))]
public class FixedWidthCameraEditor : Editor
{
    private FixedWidthCamera m_Target;

    void OnEnable()
    {
        m_Target = (FixedWidthCamera)target;
    }

    public override void OnInspectorGUI()
    {
        m_Target.m_CameraHeight = EditorGUILayout.FloatField("Camera height", m_Target.m_CameraHeight);
        m_Target.Refresh ();
    }
}
