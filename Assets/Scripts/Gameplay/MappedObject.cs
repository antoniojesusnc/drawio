using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MappedObject : MonoBehaviour 
{
	public Transform 		m_MappedTransform;

	// Cache
	protected Transform		m_Transform;

	// Buffers
	protected int			m_MapIndex;
	protected int 			m_LastMapKey = -1;

	protected virtual void Awake()
	{
		// Cache
		m_Transform = transform;
	}

	protected void RegisterMap()
	{
		m_MapIndex = MapManager.RegisterEntity (gameObject);
		UpdateMap ();
	}

	protected void UpdateMap()
	{
		if (m_MappedTransform != null)
			m_LastMapKey = MapManager.UpdateEntity (m_LastMapKey, m_MappedTransform.position, m_MapIndex);
		else
			m_LastMapKey = MapManager.UpdateEntity (m_LastMapKey, m_Transform.position, m_MapIndex);
	}

	protected void UnregisterMap()
	{
		MapManager.UnregisterEntity (m_LastMapKey, m_MapIndex);
	}
}
