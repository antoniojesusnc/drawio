using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
	public enum PositionType
	{
		SCREEN,
		SCREEN_PERCENT,
		WORLD
	}

	public PositionType 	m_PosType;

	// Cache
	protected Transform		m_Transform;
	protected Camera		m_Camera;
	protected Transform 	m_CameraTr;

	protected virtual void Awake()
	{
		m_Transform = transform;
		m_Camera = Camera.main;
		m_CameraTr = m_Camera.transform;
	}

	protected float GetDepth()
	{
		return Vector3.Project (m_Transform.position - m_CameraTr.position, m_CameraTr.forward).magnitude;
	}
}