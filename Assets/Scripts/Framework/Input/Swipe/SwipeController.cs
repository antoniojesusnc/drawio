using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SwipeController : InputController
{
	public float 					m_Sensitivity = 1.0f;

	// Cache
	private ISwipeController		m_SwipeController;

	// Buffers
	private bool 					m_IsMoving;
	private float 					m_LastPos;
	private float					m_Pos;
	private Vector3					m_PosBuffer;

	protected override void Awake ()
	{
		base.Awake ();

		// Cache
		m_SwipeController = GetComponent<ISwipeController> ();

		// Buffers
		m_IsMoving = false;
		m_LastPos = 0.0f;
		m_Pos = 0.0f;
		m_PosBuffer = Vector3.zero;
	}

	private void GetPos(ref float _Pos)
	{
		switch (m_PosType)
		{
		case PositionType.SCREEN:
			_Pos = Input.mousePosition.x;
			break;

		case PositionType.SCREEN_PERCENT:
			_Pos = (Input.mousePosition.x / Screen.width);
			break;

		case PositionType.WORLD:
			m_PosBuffer.Set (Input.mousePosition.x, 0.0f, GetDepth ());
			_Pos = m_Camera.ScreenToWorldPoint (m_PosBuffer).x;
			break;
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_IsMoving = true;
			GetPos (ref m_LastPos);
			m_SwipeController.OnStartMove (m_LastPos);
		}

		if (m_IsMoving)
		{
			GetPos (ref m_Pos);
			m_SwipeController.OnMove ((m_Pos - m_LastPos) * m_Sensitivity);
			m_LastPos = m_Pos;
		}

		if (Input.GetMouseButtonUp(0))
		{
			m_IsMoving = false;
			GetPos (ref m_Pos);
			m_SwipeController.OnEndMove (m_Pos);
		}
	}
}
