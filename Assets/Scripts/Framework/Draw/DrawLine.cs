using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
	public enum DrawType
	{
		DISTANCE,
		TIME,
		DISTANCE_AND_TIME,
		DISTANCE_OR_TIME
	}

	public bool 		    m_DrawAtStart;
	public DrawType 	    m_DrawType;
	public float 		    m_PointDistance = 0.2f;
	public float		    m_PointTimeRate = 0.2f;

	// Cache
	private Transform 	    m_Transform;
	private IDrawLine	    m_DrawLine;
	private int             m_TaskID;
    private Brush           m_Brush;

	// Buffers
	private bool		    m_IsDrawing;
	private float		    m_LastTime;
	private Vector3		    m_LastPos;

	void Awake()
	{
		// Cache
		m_Transform = transform;
		m_DrawLine = GetComponent<IDrawLine> ();
		// Buffers
		m_IsDrawing = false;

	}

    public void Init(IDrawLine _DrawLineParent, Brush _Brush)
	{
        m_Brush = _Brush;
		m_DrawLine = _DrawLineParent;
	}

	void Start()
	{
		if (m_DrawAtStart)
			StartDraw ();
	}

	public void StartDraw()
	{
		m_IsDrawing = true;
        m_DrawLine.AddPoint(m_Transform.position);
        m_LastPos = m_Transform.position;
		m_LastTime = Time.time;
	}

	public void StopDraw()
	{
		m_IsDrawing = false;
		m_DrawLine.AddPoint (m_Transform.position);
	}

	void Update()
	{
		if (!m_IsDrawing)
			return;

		switch (m_DrawType)
		{
		case DrawType.DISTANCE:
			while (Vector3.Distance (m_LastPos, m_Transform.position) > m_PointDistance)
			{
				Vector3 pointPos = m_LastPos + (m_Transform.position - m_LastPos).normalized * m_PointDistance;
                    m_DrawLine.AddPoint (pointPos, m_Brush.m_Type == Brush.EBrushType.SECONDARY);
				m_LastTime = Time.time;
				m_LastPos = pointPos;
			}
			break;

		case DrawType.TIME:
			if (Time.time - m_LastTime > m_PointTimeRate)
			{
				m_LastTime = Time.time;
				m_LastPos = m_Transform.position;
				m_DrawLine.AddPoint (m_LastPos, m_Brush.m_Type == Brush.EBrushType.SECONDARY);
			}
			break;
		}
	}
}
