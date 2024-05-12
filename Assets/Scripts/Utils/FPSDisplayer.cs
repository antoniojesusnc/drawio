using UnityEngine;

public class FPSDisplayer : MonoBehaviour
{
	private float       m_DeltaTime = 0.0f;
    private GUIStyle    m_Style;

    void Awake()
    {
        m_Style = new GUIStyle();
        m_Style.alignment = TextAnchor.LowerRight;
		m_Style.normal.textColor = Color.black;
    }

	private void Update()
    {
        m_DeltaTime += (Time.deltaTime - m_DeltaTime) * 0.1f;
	}

	private void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

		float msec = m_DeltaTime * 1000.0f;
		float fps = 1.0f / m_DeltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        Rect rect = new Rect(width * -0.02f, height * 0.96f, width, height * 0.02f);
        m_Style.fontSize = Screen.height * 2 / 100;
        GUI.Label(rect, text, m_Style);
	}

}