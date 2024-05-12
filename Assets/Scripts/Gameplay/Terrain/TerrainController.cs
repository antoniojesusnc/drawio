using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainController : MonoBehaviour
{
	public Vector2                  m_TerrainSize;
	public List<PaintSurface>    	m_Surfaces;
	public float                    m_ScoreMultiplier = 1f;

	public virtual void ClampPosition(ref Vector3 _Position, float _Size)
	{
		_Position.x = Mathf.Clamp(_Position.x, -m_TerrainSize.x / 2f + _Size, m_TerrainSize.x / 2f - _Size);
		_Position.z = Mathf.Clamp(_Position.z, -m_TerrainSize.y / 2f + _Size, m_TerrainSize.y / 2f - _Size);      
	}

    public virtual bool NearEdge(Vector3 _Position, float _Nearness)
    {
        return (Mathf.Abs(Mathf.Abs(_Position.x) - m_TerrainSize.x * 0.5f) < _Nearness ||
                Mathf.Abs(Mathf.Abs(_Position.z) - m_TerrainSize.y * 0.5f) < _Nearness);
    }
}
