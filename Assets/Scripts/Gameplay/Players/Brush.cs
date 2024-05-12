using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public enum EBrushType
	{
		MAIN,
        SECONDARY
	}

	private const float         c_MaxNameSize = 12f;

	public Transform 			m_ModelTr;
	public List<Renderer> 		m_Renderers;
    public List<Renderer>       m_DarkRenderers;
    [Range(0,1)]
    public float                m_DarkenValue;
	public MovingEffect         m_MovingEffect;
	public GameObject           m_Arrow;

	// Cache
	private Transform			m_Transform;
	private Rigidbody 			m_Rigidbody;
    private GameObject          m_ModelGO;
    private GameObject          m_MovingEffectGO;

	private Color				m_BaseColor;
	private Coroutine 			m_FallCoroutine;
	private Coroutine 			m_BlinkCoroutine;
    private Coroutine           m_CycleColorCoroutine;
	private Vector3             m_StartNamePos;
	public  EBrushType          m_Type;

	void Awake()
	{
		// Cache
		m_Transform = transform;
		m_Rigidbody = GetComponent<Rigidbody> ();

        m_ModelGO = m_ModelTr.gameObject;
        m_MovingEffectGO = m_MovingEffect.gameObject;

		m_Rigidbody.isKinematic = true;
	}

    public void Init(Player _Player, EBrushType _Type)
	{
        m_Type = _Type;
		SetColor(_Player.m_Color);
		m_MovingEffect.Init(_Player);
		if (_Type == EBrushType.MAIN)
			m_Arrow.SetActive(true);
	}

	public void SetColor(Color _Color)
	{
		m_BaseColor = _Color;

		for (int i = 0; i < m_Renderers.Count; ++i)
			m_Renderers [i].material.color = _Color;

        for (int i = 0; i < m_DarkRenderers.Count; i++)
            m_DarkRenderers[i].material.color = new Color(_Color.r * m_DarkenValue, _Color.g * m_DarkenValue, _Color.b * m_DarkenValue, 1);
    }

    public void SetVisible(bool _Visible)
    {
        m_ModelGO.SetActive(_Visible);
        m_MovingEffectGO.SetActive(_Visible);
        m_Arrow.SetActive(_Visible);
    }

	public void Fall (bool _Fall)
	{
		m_Rigidbody.isKinematic = !_Fall;

		if (_Fall)
			m_Rigidbody.angularVelocity = Vector3.one * 2.0f;
		else
		{
			m_Transform.localPosition = Vector3.zero;
			m_Transform.localRotation = Quaternion.identity;
		}
	}

    public void CycleColors(bool _Cycle)
    {
        if (_Cycle)
            m_CycleColorCoroutine = StartCoroutine(CycleColorCoroutine());
        else
            StopCoroutine(m_CycleColorCoroutine);

        for (int i = 0; i < m_Renderers.Count; ++i)
            m_Renderers[i].material.color = m_BaseColor;
    }

    private IEnumerator CycleColorCoroutine()
    {
        float time;
        int oldRand = -1;
        Color oldColor = m_BaseColor;

        while (true)
        {
            int rand = 0;
            Color newColor = PickPrimaryColor(oldRand, out rand);

            time = 0.0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime / 0.2f;

                for (int i = 0; i < m_Renderers.Count; ++i)
                    m_Renderers[i].material.color = Color.Lerp(oldColor, Color.yellow, time);

                yield return null;
            }

            oldColor = newColor;
            oldRand = rand;
        }
    }

    public bool IsBlinking()
    {
        return (m_BlinkCoroutine != null);
    }

	public void Blink(bool _Blink)
	{
        if (m_BlinkCoroutine != null)
            StopCoroutine(m_BlinkCoroutine);
        if (_Blink)
        {
            m_BlinkCoroutine = StartCoroutine(BlinkCoroutine());
        }

        m_ModelTr.gameObject.SetActive(true);
	}

	private IEnumerator BlinkCoroutine()
	{
        bool visible = true;

		while (true)
		{
            m_ModelTr.gameObject.SetActive(visible);

            yield return new WaitForSeconds(Constants.c_PlayerInvincibilityDuration / 10f);
            visible = !visible;
		}
	}

	private Color PickPrimaryColor(int oldRand, out int rand)
	{
		rand = Random.Range (0, 6);

		while (rand == oldRand)
			rand = Random.Range (0, 6);

		switch (rand)
		{
		case 0:
			return Color.blue;

		case 1:
			return Color.red;

		case 2:
			return Color.magenta;

		case 3:
			return Color.green;

		case 4:
			return Color.yellow;
		}

		return Color.white;
	}

	public Color GetColor()
	{
		return (m_BaseColor);
	}
}
