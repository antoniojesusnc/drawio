using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePool : MonoBehaviour 
{
	public string 		m_PoolName;
	public float 		m_Duration = 1.0f;

	private float 		m_Time;

	void OnEnable()
	{
		m_Time = Time.time;
	}

	void Update()
	{
		if (Time.time - m_Time > m_Duration)
			PoolManager.FreeInstance (m_PoolName, gameObject);
	}
}
