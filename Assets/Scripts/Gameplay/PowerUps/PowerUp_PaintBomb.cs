using System.Collections;
using UnityEngine;

public sealed class PowerUp_PaintBomb : PowerUp
{
    public static AnimationCurve s_DefaultFillCurve = null;

    public float 			m_Radius = 6.0f;
	public float			m_FillDuration = 0.3f;
	public AnimationCurve	m_FillCurve;
	private TerrainManager	m_TerrainManager;
    private float           m_RadiusMultiplier;

	protected override void Awake ()
	{
		base.Awake ();

        m_RadiusMultiplier = 1f;
		m_TerrainManager = TerrainManager.Instance;

        s_DefaultFillCurve = m_FillCurve;
    }

	public override void OnPlayerTouched (Player _Player)
	{

        m_RadiusMultiplier = Mathf.Clamp(_Player.GetSize() / _Player.GetMinSize(), 1f, 2.5f);
		UnregisterMap();
        m_Model.enabled = false;
        m_ParticleSystem.Play(true);
		m_IdleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        m_Shadow.SetActive(false);

        m_TerrainManager.FillCircle(_Player, m_Transform.position, m_Radius * m_RadiusMultiplier, m_FillDuration, SelfDestroy);
	}

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
