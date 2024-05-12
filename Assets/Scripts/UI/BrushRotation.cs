using UnityEngine;

public class BrushRotation : MonoBehaviour
{
	[SerializeField] private bool _randomSpeed = true;
	// Cache
	private Transform m_Transform;

	[SerializeField]
	private float _speed = 60;
	
	void Awake()
	{
		// Cache
		m_Transform = transform;

		if (_randomSpeed)
		{
			_speed = Random.Range(_speed * 0.5f, _speed * 1.5f);
		}
	}

	void Update ()
	{
		
		m_Transform.RotateAround(m_Transform.position, m_Transform.up, Time.deltaTime * _speed);
	}
}
