using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushRotation : MonoBehaviour
{
	// Cache
	private Transform m_Transform;

	void Awake()
	{
		// Cache
		m_Transform = transform;
	}

	void Update ()
	{
		m_Transform.RotateAround(m_Transform.position, m_Transform.up, Time.deltaTime * 90f);
	}
}
