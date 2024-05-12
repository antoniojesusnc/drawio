using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : InputController
{
    public float                    m_MinDistance;
    public float                    m_MaxDistance;
    public AnimationCurve           m_SensibilityCurve;

    // Cache
    private IDirectionController    m_DirectionController;

    // Buffers
    private bool                    m_IsMoving;
    private Vector3                 m_InputPos;
    private Vector3                 m_CenterPos;
    private Vector3                 m_PosBuffer;
    private Plane                   m_InputPlane;

    protected override void Awake()
    {
        base.Awake();

        // Cache
        m_DirectionController = GetComponent<IDirectionController>();

        // Buffers
        m_InputPlane = new Plane(Vector3.up, m_Transform.position);
    }

    private void GetPos(Vector3 _InputPos, ref Vector3 _WorldPos)
    {
        float enterDistance;
        Ray inputRay = m_Camera.ScreenPointToRay(_InputPos);
        if (m_InputPlane.Raycast(inputRay, out enterDistance))
            _WorldPos = inputRay.GetPoint(enterDistance);
    }

    void StartMove()
	{
		m_IsMoving = true;
        m_InputPos = Input.mousePosition;
        GetPos(m_InputPos, ref m_CenterPos);
        m_DirectionController.OnStartMove();
	}

    void Update()
    {
		if (BattleRoyaleManager.Instance.m_IsPlaying == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
			StartMove();
        }
        
        if (m_IsMoving || Input.GetMouseButton(0))
        {
			if (m_IsMoving == false)
				StartMove();
				
            GetPos(m_InputPos, ref m_CenterPos);
            GetPos(Input.mousePosition, ref m_PosBuffer);

            Vector3 diff = m_PosBuffer - m_CenterPos;
            float distToCenter = diff.magnitude;
            if (distToCenter < m_MinDistance)
                distToCenter = 0.0f;

            float percent = m_SensibilityCurve.Evaluate(Mathf.Clamp01(distToCenter / m_MaxDistance));
            m_DirectionController.OnMove(diff.normalized * percent);
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_IsMoving = false;
            m_DirectionController.OnEndMove();
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, m_MinDistance);

        Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, m_MaxDistance);
    }
}