using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_NewBrush : PowerUp {

	public Color            m_ArrowColor;
	private PlayerArrows    m_PlayerArrows;
    private int             m_ArrowIndex;
    private Player          m_HumanPlayer;

    protected override void Awake()
    {
        base.Awake();
        
        m_PlayerArrows = PlayerArrows.Instance;
        m_HumanPlayer = BattleRoyaleManager.Instance.GetHumanPlayer();
        m_ArrowIndex = m_PlayerArrows.Register(transform, m_ArrowColor, PlayerArrows.EArrowType.BRUSH);
    }

	public override void OnPlayerTouched(Player _Player)
    {
        base.OnPlayerTouched(_Player);

		_Player.AddBrush();
        if (m_HumanPlayer.isEliminated == false)
            m_PlayerArrows.Unregister(m_ArrowIndex);
    }
}
