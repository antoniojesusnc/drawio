using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoHand : MonoBehaviour
{
	public RectTransform 	m_Center;
	public float 			m_Radius;

	// Cache
	private RectTransform	m_Transform;

	void Awake()
	{
		// Cache
		m_Transform = GetComponent<RectTransform> ();
	}

	void Update()
	{
		m_Transform.position = m_Center.position + Quaternion.AngleAxis(360.0f * Mathf.Sin(Time.time), Vector3.forward) * (Vector3.left * m_Radius);
	}
}
