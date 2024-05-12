using UnityEngine;

public sealed class BumpEffect : BrushEffect 
{
    private const float     c_Timeout = 1.0f;
    private const float     c_MinSize = 5.0f;
    private const float     c_MaxSize = 10.0f;

	public ParticleSystem 	m_MainDrop;

    private float           m_StartTime;

	public override void Play()
	{
        m_StartTime = Time.time;
		m_MainDrop.Play ();
	}

    void Update()
    {
        if (Time.time - m_StartTime > c_Timeout)
            Destroy(gameObject);
    }

	public override void SetColor(Color _Color)
	{
		SetParticleSysColor (m_MainDrop, _Color);
	}

    public void SetPower(float _Power)
    {
        // Set size according to power
        ParticleSystem.MainModule main = m_MainDrop.main;
        ParticleSystem.MinMaxCurve startSize = main.startSize;
        startSize.constantMax = Mathf.Lerp(c_MinSize, c_MaxSize, _Power);
        main.startSize = startSize;
    }
}
