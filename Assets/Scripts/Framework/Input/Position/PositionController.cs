using UnityEngine;

public class PositionController : InputController
{
	public float 					m_Sensitivity = 1.0f;

	// Cache
	private IPositionController		m_PositionController;

	// Buffers
	private bool					m_IsMoving;
	private Vector3					m_LastPos;
	private Vector3					m_PosBuffer;

	protected override void Awake ()
	{
		base.Awake ();

		// Cache
		m_PositionController = GetComponent<IPositionController> ();

		// Buffers
		m_IsMoving = false;
		m_LastPos = Vector3.zero;
		m_PosBuffer = Vector3.zero;
	}

	private void GetPos(ref Vector3 _PosBuffer)
	{
		switch (m_PosType)
		{
		case PositionType.SCREEN:
			_PosBuffer.Set (Input.mousePosition.x, Input.mousePosition.y, 0.0f);
			break;

		case PositionType.SCREEN_PERCENT:
			_PosBuffer.Set (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0.0f);
			break;

		case PositionType.WORLD:
			_PosBuffer.Set (Input.mousePosition.x, Input.mousePosition.y, GetDepth ());
			_PosBuffer = m_Camera.ScreenToWorldPoint (_PosBuffer);
			break;
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_IsMoving = true;
			GetPos (ref m_LastPos);
			m_PositionController.OnStartMove (m_LastPos);
		}

		if (m_IsMoving)
		{
			GetPos (ref m_PosBuffer);
			m_PositionController.OnMove ((m_PosBuffer - m_LastPos) * m_Sensitivity);
			m_LastPos = m_PosBuffer;
		}

		if (Input.GetMouseButtonUp(0))
		{
			m_IsMoving = false;
			GetPos (ref m_PosBuffer);
			m_PositionController.OnEndMove (m_PosBuffer);
		}
	}
}
