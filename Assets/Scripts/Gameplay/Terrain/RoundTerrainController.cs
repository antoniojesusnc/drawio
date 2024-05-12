using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundTerrainController : TerrainController
{
	public float 		m_Radius;

	private Transform 	m_Transform;

	private void Awake()
	{
		m_Transform = transform;
	}

	public override void ClampPosition(ref Vector3 _Position, float _Size)
	{
		if (Vector3.Distance(m_Transform.position, _Position) + _Size >= m_Radius)
			_Position = (_Position - m_Transform.position).normalized * (m_Radius - _Size);         
	}

    public override bool NearEdge(Vector3 _Position, float _Nearness)
    {
        return (Vector3.Distance(m_Transform.position, _Position) <= _Nearness + m_Radius);
    }
}
