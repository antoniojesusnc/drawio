using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrows : SingletonMB<PlayerArrows>
{
    public enum EArrowType
	{
		BUCKET,
        BRUSH
	}

	public class ArrowRef
	{
		public Transform 		m_Target;
		public GameObject 		m_Arrow;
	}

    public GameObject 			m_BucketArrowPrefab;
	public GameObject           m_BrushArrowPrefab;
    public float 				m_ArrowDistance;

    private List<ArrowRef> 		m_Refs = new List<ArrowRef>();
    private GameManager 		m_GM;
    private Camera 				m_MainCam;

    private Rect 				m_ViewPort = new Rect(0, 0, 1, 1);
    private Plane 				m_ProjectionPlane;
    private Ray 				m_ProjectionRay;

    [Header("Debug")]
    public bool 				m_ShowGizmos;

	// Use this for initialization
	void Start ()
	{
        m_GM = GameManager.Instance;
        m_MainCam = MainCamera.Instance.GetComponent<Camera>();

        m_GM.onGamePhaseChanged += OnPhaseChanged;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
        Vector2 viewPortPos = new Vector2();
        float realPos;

		for (int i = m_Refs.Count - 1; i >= 0; i--)
        {
			if (m_Refs [i] == null)
				continue;

			GameObject arrowRef = m_Refs [i].m_Arrow;

			if (arrowRef == null)
                continue;

			Transform targetTr = m_Refs [i].m_Target;

            if (targetTr == null)
                continue;

			viewPortPos = m_MainCam.WorldToViewportPoint(targetTr.position);

            if (m_ViewPort.Contains(viewPortPos))
            {
				arrowRef.SetActive(false);
                continue;
            }

			arrowRef.SetActive(true);

            m_ProjectionPlane = new Plane(this.transform.up, this.transform.position);
            m_ProjectionRay = m_MainCam.ViewportPointToRay(viewPortPos);

            if (m_ProjectionPlane.Raycast(m_ProjectionRay, out realPos))
				m_Refs[i].m_Arrow.transform.localPosition = this.transform.InverseTransformPoint(m_ProjectionRay.GetPoint(realPos)).normalized * m_ArrowDistance;
			
			viewPortPos = m_MainCam.WorldToViewportPoint(arrowRef.transform.position);

            if(!m_ViewPort.Contains(viewPortPos))
            {
				viewPortPos = new Vector2(Mathf.Clamp01(viewPortPos.x), Mathf.Clamp(viewPortPos.y, 0.1f, 1.0f));

                m_ProjectionRay = m_MainCam.ViewportPointToRay(viewPortPos);

                if (m_ProjectionPlane.Raycast(m_ProjectionRay, out realPos))
					m_Refs[i].m_Arrow.transform.position = m_ProjectionRay.GetPoint(realPos);
            }

			m_Refs[i].m_Arrow.transform.rotation = Quaternion.LookRotation(this.transform.up, arrowRef.transform.position - this.transform.position);
        }
	}

    void OnPhaseChanged(GamePhase _Phase)
    {
        if (_Phase == GamePhase.END)
            ClearArrows();
        
    }

	public int Register(Transform _Transform, Color _Color, EArrowType _Type)
    {
        ArrowRef newArrow = new ArrowRef();
		GameObject m_ArrowPrefab;

        switch (_Type)
		{
			case EArrowType.BRUSH:
				m_ArrowPrefab = m_BrushArrowPrefab;
				break;
			case EArrowType.BUCKET:
			default:
				m_ArrowPrefab = m_BucketArrowPrefab;
				break;
		}

		newArrow.m_Target = _Transform;
		newArrow.m_Arrow = Instantiate(m_ArrowPrefab, this.transform);
		newArrow.m_Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _Color;

		for (int i = 0; i < m_Refs.Count; ++i)
		{
			if (m_Refs [i] == null)
			{
				m_Refs [i] = newArrow;
				return i;
			}
		}
        
		m_Refs.Add(newArrow);
		return (m_Refs.Count - 1);
    }

	public void Unregister(int _Index)
    {
        if (m_Refs.Count > _Index && m_Refs[_Index] != null)
        {
            Destroy(m_Refs[_Index].m_Arrow);
            m_Refs[_Index] = null;
        }
    }

    void ClearArrows()
    {
		for (int i = m_Refs.Count - 1; i >= 0; i--)
		{
			if (m_Refs [i] != null && 
				m_Refs [i].m_Arrow != null)
				Destroy (m_Refs [i].m_Arrow);
			
			m_Refs.RemoveAt (i);
		}
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos)
            return;

        Color c = Gizmos.color;
        Matrix4x4 m = Gizmos.matrix;

        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.matrix = this.transform.localToWorldMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(100, 0.1f, 100));

        Gizmos.color = c;
        Gizmos.matrix = m;
    }
}
