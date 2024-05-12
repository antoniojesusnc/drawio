using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : SingletonMB<ScreenShaker> 
{
	const float 		DEFAULT_SHAKE_INTENSITY = 0.15f;
	const float 		DEFAULT_WAVE_INTENSITY = 2f;
	const float 		DEFAULT_DURATION = 0.2f;

	private Transform 	m_Transform;
	private Camera 		m_Camera;

	private Coroutine 	shakeCo;

	void Awake()
	{
		m_Transform = transform;
		m_Camera = GetComponent<Camera> ();
	}

	#region Shake

	public void Shake (bool fadeOut = false)
	{
		Shake (DEFAULT_SHAKE_INTENSITY, DEFAULT_DURATION, fadeOut);
	}

	public void Shake (float intensity, float duration, bool fadeOut = false)
	{
		if(shakeCo != null)
			StopCoroutine(shakeCo);
		shakeCo = StartCoroutine (ShakeCo (intensity, duration, fadeOut));
	}

	private IEnumerator ShakeCo (float intensity, float duration, bool fadeOut = false)
	{
		float t = 0;		
		float _intensity = intensity;

		while (t < duration)
		{
			Vector3 basePos = m_Camera.transform.position;
			m_Transform.position = basePos + ((Vector3)Random.insideUnitCircle * _intensity);

			yield return new WaitForEndOfFrame();
			m_Transform.position = basePos;

			yield return null;

			if(fadeOut)
				_intensity = Mathf.Lerp(intensity, 0, t/duration);
			
			t += Time.deltaTime;
		}

		yield break;
	}

	#endregion


	#region Wave

	public void Wave (bool fadeOut = true)
	{
		Wave(Vector3.right, DEFAULT_WAVE_INTENSITY, DEFAULT_SHAKE_INTENSITY, fadeOut);
	}

	public void Wave (Vector3 axis, float intensity, float duration, bool fadeOut = true)
	{
		if(shakeCo != null)
			StopCoroutine(shakeCo);
		
		shakeCo = StartCoroutine (WaveCo (axis, intensity, duration, fadeOut));
	}

	private IEnumerator WaveCo (Vector3 axis, float intensity, float duration, bool fadeOut = true)
	{
		float t = 0;		
		float _intensity = intensity;
		int sign = 1;

		while (t < duration)
		{
			Quaternion baseRot = m_Transform.rotation;
			m_Transform.rotation = baseRot;
			m_Transform.Rotate(axis, _intensity * sign, Space.Self);

			yield return new WaitForEndOfFrame();

			m_Transform.rotation = baseRot;

			yield return null;

			int seg  = (int)Mathf.Floor(t / (duration/4f));

			if(fadeOut)
				_intensity = Mathf.Lerp( Mathf.Lerp(intensity, 0, 0.25f * seg), 0, (t-(duration/4*seg)) / (duration/4f));
			
			t += Time.deltaTime;
			sign = seg % 2 == 0 ? 1 : -1;
		}

		yield break;
	}

	#endregion
}
