using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningView : View<WarningView> {

	private const float         c_TransitionCD = 0.5f;
	private const float         c_TimeBeforeWarning = 6f;

	public Text m_TimerText;

	private Player              m_HumanPlayer;
	private BattleRoyaleManager m_BattleRoyaleManager;
	private bool                m_Playing;
	private float               m_Timer;

	protected override void Awake()
	{
		base.Awake();
		m_BattleRoyaleManager = BattleRoyaleManager.Instance;
	}
   
	protected override void Update()
	{
		base.Update();

		if (m_Playing) 
		{
			int alivePlayers = m_BattleRoyaleManager.GetAlivePlayersCount();
			if (alivePlayers == 1)
				return;

			float time = m_BattleRoyaleManager.GetTimeBeforeNextElimination();
			if (time > c_TimeBeforeWarning)
				return;

			if (m_HumanPlayer == null)
				m_HumanPlayer = m_BattleRoyaleManager.GetHumanPlayer();
			
			if (m_HumanPlayer.m_Rank + 1 == alivePlayers) // If Player is last and about to die
			{
				TryToTransition(true);
				m_BattleRoyaleManager.ApplySaveMechanic(m_HumanPlayer);
			}
			else 
			{
				TryToTransition(false);
			}
			m_TimerText.text = time.ToString("0") + " SECS";
		}      
	}

    void TryToTransition(bool _Transition)
	{
		if (m_Visible == _Transition)
			return;
		if (m_Timer <= Time.time)
		{
			Transition(_Transition);
			m_Timer = Time.time + c_TransitionCD;
		}
	}

	protected override void OnGamePhaseChanged(GamePhase _GamePhase)
	{
		base.OnGamePhaseChanged(_GamePhase);
        switch (_GamePhase)
		{
            case GamePhase.PRE_END:
			case GamePhase.END:
			case GamePhase.MAIN_MENU:
				Transition(false);
				m_Playing = false;
				break;
			case GamePhase.GAME:
				m_Playing = true;
				break;
		}
	}
}
