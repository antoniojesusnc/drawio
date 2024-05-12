using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-10)]
public class TerrainManager : SingletonMB<TerrainManager>
{
	public static float				s_Size = 300.0f;
	public List<PaintSurface> 		m_Surfaces;
	public Color32 					m_DefaultColor;
    public AnimationCurve           m_DefaultFillCurve;

    // Cache
    private List<TerrainData> 		m_Terrains;
	private TerrainData 			m_CurrentTerrainData;
	private TerrainController 		m_CurrentTerrain;

	private void Awake()
	{
		m_Terrains = new List<TerrainData>();
		m_Terrains.AddRange(Resources.LoadAll<TerrainData>("Terrains"));

		SetTerrain();
	}

	public void SetTerrain()
	{
		if (m_CurrentTerrain != null)
			ClearTerrain();

		m_Terrains.Shuffle();
		m_CurrentTerrainData = m_Terrains[0];
		m_CurrentTerrain = Instantiate(m_CurrentTerrainData.m_Prefab, Vector3.zero, Quaternion.identity);
		m_Surfaces = m_CurrentTerrain.m_Surfaces;

		for (int i = 0; i < m_Surfaces.Count; ++i)
			m_Surfaces[i].Init(m_DefaultColor);
	}

	void ClearTerrain()
	{
		Destroy (m_CurrentTerrain.gameObject);
	}

	public void ClampPosition(ref Vector3 _Position, float _Size)
	{
		m_CurrentTerrain.ClampPosition(ref _Position, _Size);
	}

    public bool NearEdge(Vector3 _Position, float _Nearness)
    {
        return (m_CurrentTerrain.NearEdge(_Position, _Nearness));
    }

	public float WorldHalfWidth
	{
		get
		{
			return (m_CurrentTerrain.m_TerrainSize.x / 2f);
		}
	}

	public float WorldHalfHeight
	{
		get
		{
			return (m_CurrentTerrain.m_TerrainSize.y / 2f);
		}
	}

	public float ScoreMultiplier
	{
		get 
		{
			return (m_CurrentTerrain.m_ScoreMultiplier);
		}
	}

	#region Surfaces

	public float GetColorPercent(int _ColorHash)
	{
		float sum = 0.0f;

		for (int i = 0; i < m_Surfaces.Count; ++i)
			sum += m_Surfaces [i].GetColorPercent (_ColorHash);
		
		return (sum / m_Surfaces.Count);
	}

	public Vector3 GetLowestColoredPosition(int _ColorHash)
	{
		int minIndex = -1;
		float minPercent = float.MaxValue;

		for (int i = 0; i < m_Surfaces.Count; ++i)
		{
			float percent = m_Surfaces [i].GetColorPercent (_ColorHash);
			if (percent < minPercent)
			{
				minIndex = i;
				minPercent = percent;
			}
		}

		return m_Surfaces[minIndex].transform.position;
	}

	public void ReplaceColor(Vector3 _Center, float _StartWidth, float _EndWidth, int _OldColorHash, int _NewColorHash, ref Color _NewColor)
	{
		for (int i = 0; i < m_Surfaces.Count; ++i)
			m_Surfaces [i].ReplaceColor (_Center, _StartWidth, _EndWidth, _OldColorHash, _NewColorHash, ref _NewColor);
	}

	public void AddCircle(Vector3 _Position, float _Radius, int _ColorHash, ref Color _Color)
	{
		for (int i = 0; i < m_Surfaces.Count; ++i)
			m_Surfaces [i].AddCircle (_Position, _Radius, _ColorHash, ref _Color);
	}

    public void FillCircle(Player _Player, Vector3 _Pos, float _Radius, float _Duration, UnityAction _EndCallback = null)
    {
        StartCoroutine(FillCoroutine(_Player, _Pos, _Radius, _Duration, _EndCallback));
    }

    public IEnumerator FillCoroutine(Player _Player, Vector3 _Pos, float _Radius, float _Duration, UnityAction _EndCallback)
    {
		Color color = _Player.m_Color;
		int colorHash = _Player.m_ColorHash;

        float time = 0.0f;
        while (time < 1.0f)
        {
            time += Time.deltaTime / _Duration;
            this.AddCircle(_Pos, m_DefaultFillCurve.Evaluate(time) * _Radius, colorHash, ref color);
            yield return null;
        }

        if (_EndCallback != null)
            _EndCallback.Invoke();
    }

    #endregion
}