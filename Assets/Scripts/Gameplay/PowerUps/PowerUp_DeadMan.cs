using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_DeadMan : PowerUp {

	public ParticleSystem 			m_AmbientParticleSystem;
	private Player 					m_DeadPlayer;
	private TerrainManager 			m_TerrainManager;
	private PlayerArrows 			m_PlayerArrows;
	private int 					m_ArrowIndex;
    private Player                  m_HumanPlayer;

    protected override void Awake()
    {
        base.Awake();

		m_TerrainManager = TerrainManager.Instance;
		m_PlayerArrows = PlayerArrows.Instance;
        m_HumanPlayer = BattleRoyaleManager.Instance.GetHumanPlayer();
    }

    public void SetDeadPlayer(Player _DeadPlayer)
	{
		m_DeadPlayer = _DeadPlayer;
		m_Model.material.color = _DeadPlayer.m_Color;
		m_AmbientParticleSystem.SetColor(_DeadPlayer.m_Color);
		m_ArrowIndex = m_PlayerArrows.Register (transform, _DeadPlayer.m_Color, PlayerArrows.EArrowType.BUCKET);
	}

	public override void OnPlayerTouched(Player _Player)
    {
        UnregisterMap();
        if (m_HumanPlayer.isEliminated == false)
		    m_PlayerArrows.Unregister (m_ArrowIndex);

		m_Model.gameObject.SetActive(false);
        m_ParticleSystem.Play(true);
		m_IdleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        m_Shadow.SetActive(false);
		StartCoroutine(SpreadColor(_Player));
    }

	private IEnumerator SpreadColor(Player _Player)
    {
		Vector3 center = m_Transform.position;

		yield return null;

		float startWidth = 0.0f;
		float offset = 2.0f;
		float endWidth = offset;

		int oldColorHash = m_DeadPlayer.m_ColorHash;
        do
        {
			m_TerrainManager.ReplaceColor(center, startWidth, endWidth, oldColorHash, _Player.m_ColorHash, ref _Player.m_Color);

			startWidth = endWidth;
			endWidth += offset;

            yield return null;
        }
		while (endWidth < TerrainManager.s_Size);

		Destroy(gameObject);
    }

}
