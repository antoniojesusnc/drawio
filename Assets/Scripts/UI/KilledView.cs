using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledView : View<KilledView> 
{
    private Player m_HumanPlayer;
    private BattleRoyaleManager m_BattleRoyale;

    protected override void Awake()
    {
        base.Awake();
        m_BattleRoyale = BattleRoyaleManager.Instance;
        m_GameManager.onGamePhaseChanged += OnGamePhaseChanged;
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);
        switch (_GamePhase)
        {
            case GamePhase.GAME:
                m_HumanPlayer = m_BattleRoyale.GetHumanPlayer();
                m_HumanPlayer.onKilled += OnKilled;
                m_HumanPlayer.onRevive += OnRevive;
                break;
        }
    }

    void OnKilled()
    {
        Transition(true);
    }

    void OnRevive()
    {
        Transition(false);
    }
}
