using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IAPlayer : Player
{
	public enum IABehaviour
	{
		RANDOM,
        GO_TO_POINT,
		FOLLOWING_TRANSFORM,
		ESCAPING_TRANSFORM
	}

	private const float 	    c_MinRandomRange = 25.0f;
	private const float 	    c_MaxRandomRange = 30.0f;
	private const float 	    c_SqrToleranceDistance = 4.0f;
    private const float         c_CheckRate = 0.7f;
    private const float         c_SqrCheckRadius = 1800.0f;
	private const float         c_DifficultyCheckRadius = 7000.0f;
	private const float 		c_LerpFactor = 0.05f;
	private const float         c_FastLerpFactor = 0.9f;
    private const float         c_NormalLerpFactor = 0.014f;
	private const float         c_WarningEdgeDistance = 11f;
	private const int           c_MinPowerUpChasing = 50;
	private const int           c_MaxPowerUpChasing = 100;
	private const int 			c_MinCovering = 50;
	private const int 			c_MaxCovering = 100;
	private const int           c_MinAgressivity = 8;
	private const int           c_MaxAgressivity = 25;
	private const float         c_MinFollowTime = 1.5f;
	private const float         c_MaxFollowTime = 3.5f;
	private const float         c_DifficultyFollowTime = 1.4f;
	private const float         c_DifficultyToChaseLastPlayer = 0.4f;
	private const float         c_DifficultyToFocusOnPowerUp = 0.8f;
	private const float         c_DifficultyToMaybeFocusOnPowerUp = 0.55f;

    public IABehaviour 	        m_Behaviour;
	private Vector3 		    m_Destination;
    private float               m_LastCheckTime;
	private float 				m_CheckRate;
	private Vector3 			m_Forward;
    private Transform           m_Target;
    private Vector3             m_DeathDirection;
	private float               m_SqrCheckRadius;
	private float               m_Agressivity;
    
	private int					m_PowerUpChasing;
	private int					m_Covering;
	private float               m_LerpFactor;
	private float               m_Difficulty; // From 0 to 1

    private Tweener             m_Tween;

	void Start()
	{
		m_PowerUpChasing = c_MaxPowerUpChasing;
		m_Covering = c_MaxCovering;
		m_Agressivity = Random.Range(c_MinAgressivity, c_MaxAgressivity);
        m_LerpFactor = c_NormalLerpFactor;
        m_LastCheckTime = Time.time;
        m_Difficulty = Random.Range(Mathf.Clamp01(StatsManager.Instance.GetLevel() / 2f), 1f);
		m_SqrCheckRadius = c_SqrCheckRadius + c_DifficultyCheckRadius * m_Difficulty;
        Check();
        ChangeMoveStatus(false);
	}

    private void Check()
    {
        if (BattleRoyaleManager.Instance.m_IsPlaying == false)
            return;

		m_LastCheckTime = Time.time;

        // If very difficult or difficult and random
        if (m_Difficulty >= c_DifficultyToFocusOnPowerUp ||
		    (m_Difficulty >= c_DifficultyToMaybeFocusOnPowerUp && Random.Range(0, 1f) <= m_Difficulty / 10f))
		{         
		    MapManager.FindEntities(m_Transform.position, m_SqrCheckRadius * 5f, ref m_SearchBuffer, LayerMask.NameToLayer("ImportantPowerUp"));
		    for (int i = 0; i < m_SearchBuffer.Count; ++i)
		    {
		    	GameObject obj = m_SearchBuffer[i];
            
		    	// Check power up behaviour
		    	PowerUp powerUp = obj.GetComponent<PowerUp>();
		    	if (powerUp != null)
                {
                    Follow(powerUp.transform, 0.5f);
                    return;
                }
		    }
		}

		MapManager.FindEntities(m_Transform.position, m_SqrCheckRadius, ref m_SearchBuffer);
        for (int i = 0; i < m_SearchBuffer.Count; ++i)
        {
            GameObject obj = m_SearchBuffer[i];

			// Check power up behaviour
			PowerUp powerUp = obj.GetComponent<PowerUp>();
			if (powerUp != null && Random.Range(0, 100) < m_PowerUpChasing)
			{
				Follow(powerUp.transform, 0.4f);
				return;
			}

			Player player = obj.GetComponent<Player>();
			if (player != null)
			{
				if (Random.Range(0, 100) < m_Agressivity && player.m_Rank == m_Rank - 1)
				{
					Follow(player.transform, Random.Range(c_MinFollowTime, c_MaxFollowTime) + (c_DifficultyFollowTime * m_Difficulty));
					return;
				}

				if (m_Difficulty >= c_DifficultyToChaseLastPlayer && Random.Range(0, 100) < m_Agressivity / 6f && player.m_Rank == BattleRoyaleManager.Instance.GetAlivePlayersCount() - 1)
				{
					Follow(player.transform, Random.Range(c_MinFollowTime, c_MaxFollowTime) + (c_DifficultyFollowTime * m_Difficulty));
                    return;
				}
			}
        }

        // Check zone
		if (Random.Range(0, 100) < m_Covering)
        {
			m_Destination = m_TerrainManager.GetLowestColoredPosition(m_ColorHash);
			m_CheckRate = 0.5f;
            ChangePhase(IABehaviour.GO_TO_POINT);
            return;
        }

		m_CheckRate = c_CheckRate;
		ChangePhase(IABehaviour.RANDOM);
    }

	private void ChangePhase(IABehaviour _Behaviour)
	{
		switch (_Behaviour)
		{
		case IABehaviour.RANDOM:
			FindRandomDestination ();
			break;
		}

        if (_Behaviour == IABehaviour.FOLLOWING_TRANSFORM)
            SetBraking(c_FastLerpFactor, 2f);
        else
            SetBraking(c_NormalLerpFactor, 0f);

		ChangeMoveStatus(true);
		m_Behaviour = _Behaviour;
	}

	private void Follow(Transform _Target, float _CheckRate) 
	{
		m_Target = _Target;
		m_CheckRate = _CheckRate;
        ChangePhase(IABehaviour.FOLLOWING_TRANSFORM);
	}

    public override void Eliminate()
    {
        base.Eliminate();

        m_DeathDirection = m_Direction;
    }

    public void SetBraking(float _Braking, float _Time)
    {
        if (m_Tween != null)
            m_Tween.Kill();

        if (_Time > 0f)
            m_Tween = DOTween.To(() => m_LerpFactor, (x) => m_LerpFactor = x, _Braking, _Time);
        else
            m_LerpFactor = _Braking;
    }

    public override void Die()
    {
        base.Die();
        HideVisual();
    }

    protected override void Revive()
    {
        base.Revive();
        ShowVisual();
    }

    protected override void Update()
	{
        base.Update();

        if (BattleRoyaleManager.Instance.m_IsPlaying == false)
            return;

        if (m_IsEliminated)
        {
            m_DeathDirection = Vector3.Lerp(m_DeathDirection, Vector3.zero, Time.deltaTime);
            m_Transform.position += m_DeathDirection * Time.deltaTime;
            return;
        }
        
        switch (m_Behaviour)
		{
		case IABehaviour.FOLLOWING_TRANSFORM:
			if (m_Target != null)
				m_Destination = m_Target.position;
			else
				Check ();
            break;

        case IABehaviour.ESCAPING_TRANSFORM:
            if (m_Target != null)
                m_Destination = -m_Target.position;
            else
				Check ();
            break;
        }

		if (Move() || Time.time - m_LastCheckTime > m_CheckRate)
			Check ();
	}

    private bool Move()
    {
        float lerpFactor = CloseToEdges() ? c_FastLerpFactor : m_LerpFactor;

        Vector3 diff = m_Destination - m_Transform.position;
        diff.y = 0.0f;
        m_Forward = Vector3.Lerp(m_Forward, diff, lerpFactor * Time.deltaTime).normalized;

		m_Direction = m_Forward * GetSpeed();

		Vector3 pos = m_Transform.position;
		pos += m_Direction * Time.deltaTime;
		SetFinalPosition(pos);

		m_Transform.rotation = Quaternion.LookRotation (m_Forward);

        if (diff.sqrMagnitude < c_SqrToleranceDistance)
            return true;

        return false;
    }

    bool CloseToEdges() {
        return (m_TerrainManager.NearEdge(m_Transform.position, c_WarningEdgeDistance));
	}

	private void FindRandomDestination()
	{
		m_Destination.Set (
			m_Transform.position.x + GetRandomValue (-c_MinRandomRange, c_MaxRandomRange),
            0.0f,
            m_Transform.position.z + GetRandomValue (-c_MinRandomRange, c_MaxRandomRange));
		
		ClampPosition (ref m_Destination);
	}

	private float GetRandomValue(float _MinValue, float _MaxValue)
	{
		if (Random.Range (0, 8) == 0)
			return Random.Range (-_MaxValue, -_MinValue);
		else
			return Random.Range (_MinValue, _MaxValue);
	}

    void OnDrawGizmos()
    {
		if (!Application.isPlaying)
			return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_Transform.position, m_Destination);
    }
}