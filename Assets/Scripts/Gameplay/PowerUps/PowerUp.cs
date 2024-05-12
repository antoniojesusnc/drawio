using UnityEngine;
using System.Collections;

public abstract class PowerUp : MappedObject
{
	public float 			m_Duration = 5.0f;
	public bool             ready { get { return (Time.time > Constants.c_PowerUpPreWarm + m_SpawnTime); } }
	public float            availabilityTime { get { return (Mathf.Max(Constants.c_PowerUpPreWarm + m_SpawnTime - Time.time, 0f)); } }
	public MeshRenderer     m_Model;
	public ParticleSystem   m_ParticleSystem;
	public ParticleSystem   m_IdleParticleSystem;
	public GameObject       m_Shadow;
    
	protected float         m_SpawnTime;
	private bool            m_Prewarm;
	private Coroutine       m_PrewarmCoroutine;
	private bool            m_Alive;
	private float           m_ScaleFactor;
	private Vector3			m_BasePosition;

	protected override void Awake ()
	{
		base.Awake ();

		m_ScaleFactor = m_Model.transform.localScale.x;
		m_Model.transform.localScale = Vector3.zero;
		m_Prewarm = true;
		m_PrewarmCoroutine = StartCoroutine(VisualPrewarm());
		m_SpawnTime = Time.time;
		m_BasePosition = m_Transform.position;
		
		m_Alive = true;
	}

	void Start()
	{
		RegisterMap();
	}

	void Update()
	{
		if (m_Alive == true)
		{
			if (m_Prewarm && ready)
				SetReady();

			m_Transform.RotateAround(m_Transform.position, Vector3.up, 150.0f * Time.deltaTime);
			m_Transform.position = m_BasePosition + Vector3.up * Mathf.Sin (Time.time * 5.0f) * 3.0f;
		}
		else if (m_ParticleSystem.IsAlive(true) == false)
			Destroy(gameObject);
	}

	public virtual void OnPlayerTouched(Player _Player)
	{
		UnregisterMap();

        m_Model.enabled = false;
		m_ParticleSystem.Play(true);
		m_IdleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		m_Shadow.SetActive(false);
		m_Alive = false;      
	}

	protected virtual void SetReady()
	{
		m_Prewarm = false;

		if (m_PrewarmCoroutine != null)
			StopCoroutine(m_PrewarmCoroutine);
	}

	protected IEnumerator VisualPrewarm()
	{
		float time = 0;

		while (time < Constants.c_PowerUpPreWarm)
		{
			float value = Mathf.Lerp(0f, 1f, time / Constants.c_PowerUpPreWarm) * m_ScaleFactor;
			m_Model.transform.localScale = new Vector3(value, value, value);
			yield return (null);
			time += Time.deltaTime;
		}

		m_Model.transform.localScale = Vector3.one * m_ScaleFactor;
	}
}