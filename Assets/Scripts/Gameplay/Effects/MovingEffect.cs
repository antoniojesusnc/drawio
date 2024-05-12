using UnityEngine;

public sealed class MovingEffect : BrushEffect 
{
	public ParticleSystem 	m_LeftEffect;
	public ParticleSystem 	m_RightEffect;
	public ParticleSystem 	m_SplashEffect;
	public ParticleSystem   m_SpeedEffect;

	// Cache
	private Transform 		m_LeftTransform;
	private Transform 		m_RightTransform;

	// Buffers
	private Vector3			m_PosBuffer;

	void Awake()
	{
		// Cache
		m_LeftTransform = m_LeftEffect.transform;
		m_RightTransform = m_RightEffect.transform;

		// Buffers
		m_PosBuffer = Vector3.zero;
	}

    public void Init(Player _Player)
	{
		SetColor(_Player.m_Color);
        _Player.onDeath += _Player_OnDeath;
        _Player.onRevive += _Player_OnRevive;
		_Player.onMoveStatusChanged += _Player_OnMoveStatusChanged;

		if (_Player.IsMoving() == false)
			Stop();
		else
			Play();
	}

	void _Player_OnDeath()
	{
		Stop();
		m_SpeedEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

    void _Player_OnRevive()
    {
        Play();
    }

	void _Player_OnMoveStatusChanged(bool _Moving)
	{
		if (_Moving == true)
			Play();
		else
			Stop();
	}

	public override void Play()
	{
		m_LeftEffect.Play ();
		m_RightEffect.Play ();
		m_SplashEffect.Play ();
	}

	public override void Stop()
	{
		m_LeftEffect.Stop ();
		m_RightEffect.Stop ();
		m_SplashEffect.Stop ();
	}

	public override void SetColor(Color _Color)
	{
		SetParticleSysColor (m_LeftEffect, _Color);
		SetParticleSysColor (m_RightEffect, _Color);
		SetParticleSysColor (m_SplashEffect, _Color);

	}

	public void SetSize(float _Size)
	{
		m_PosBuffer.x = -_Size * 0.5f;
		m_LeftTransform.localPosition = m_PosBuffer;
		m_PosBuffer.x = _Size * 0.5f;
		m_RightTransform.localPosition = m_PosBuffer;
	}
}
