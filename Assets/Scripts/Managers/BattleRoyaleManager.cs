using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(+10)]
public class BattleRoyaleManager : SingletonMB<BattleRoyaleManager>
{
    // Normal Games
    private const float                         c_WarmUpTime = 18f;
    private const float                         c_EliminationWaitingTime = 12f;
    private const float                         c_NormalGameTimer = 120f;
    private const int                           c_CountDownTimer = 3;

    public GameObject 							m_Crown;
	public GameObject							m_Skull;
	public bool 								m_IsPlaying;

	// Inspector
	[SerializeField] private PowerUp_DeadMan    m_PowerUpOnDeathPrefab;

	// Buffer
	private Coroutine                           m_CountdownCoroutine;
	public List<Player>                         m_Players;
	private Player                              m_HumanPlayer;
	private Player                              m_OldLastPlayer; // Worst player of last frame
	private GameManager                         m_GameManager;
	private float                               m_TimeBeforeElimination;

	public delegate void OnElimination(Player _EliminatedPlayer);
    public event OnElimination onElimination;

	private void Awake()
	{
		m_GameManager = GameManager.Instance;
		m_GameManager.onGamePhaseChanged += Instance_OnGamePhaseChanged;

		m_Crown.SetActive (false);
		m_Skull.SetActive (false);
        m_IsPlaying = false;
	}

    private void Update()
	{
        if (m_IsPlaying == false)
            return;

		bool updateIcons = (m_Players != null && m_Players.Count >= 2);

        m_Skull.SetActive (updateIcons);
        m_Crown.SetActive (updateIcons);

		if (!updateIcons)
			return;
        
		Player first = GetBestPlayer ();
		Player last = GetWorstPlayer ();

        m_Skull.transform.position = last.transform.position + Vector3.up * 5.0f * last.GetSize() + (last.transform.forward * 3f);
        m_Skull.transform.localScale = Vector3.one * 10.0f * last.GetSize();

        m_Crown.transform.position = first.transform.position + Vector3.up * 5.0f * first.GetSize() + (first.transform.forward * 3f);
        m_Crown.transform.localScale = Vector3.one * 10.0f * first.GetSize();
    }

	// Kind mechanic to allow player at least 5 seconds before getting eliminated
    public void ApplySaveMechanic(Player _LastPlayer)
	{
		if (_LastPlayer != m_OldLastPlayer)
        {
			if (_LastPlayer == m_HumanPlayer && m_TimeBeforeElimination <= Time.time + Constants.c_PlayerCountdownHelp)
                m_TimeBeforeElimination = Time.time + Constants.c_PlayerCountdownHelp;
        }

		m_OldLastPlayer = _LastPlayer;
	}

	public float GetTimeBeforeNextElimination()
	{
		return (m_TimeBeforeElimination - Time.time);
	}

	void Instance_OnGamePhaseChanged(GamePhase _GamePhase)
	{
		switch (_GamePhase)
		{
			case GamePhase.MAIN_MENU:
                EndGame();
                break;
			case GamePhase.GAME:
                StartCoroutine(CountDownStart());
				break;
            case GamePhase.PRE_END:
                {
                    if (m_CountdownCoroutine != null)
                        StopCoroutine(m_CountdownCoroutine);
                    m_CountdownCoroutine = null;
                    break;
                }
		}
	}

    IEnumerator CountDownStart()
    {
        for (int i = 3; i > 0; i--)
        {
            ProgressionView.Instance.SetCountDownTime(i);
            yield return new WaitForSeconds(1f);
        }

        m_IsPlaying = true;
        m_CountdownCoroutine = StartCoroutine(CountDownElimination());
    }

    IEnumerator CountDownElimination()
    {
        m_TimeBeforeElimination = Time.time + c_WarmUpTime + c_EliminationWaitingTime;
        yield return (new WaitForSeconds(c_WarmUpTime));

        while (m_Players.Count > 1)
        {
            m_TimeBeforeElimination = Time.time + c_EliminationWaitingTime;
            do
            {
                yield return null;
                ApplySaveMechanic(GetWorstPlayer());
            }
            while (Time.time < m_TimeBeforeElimination);
            m_Players.Sort((x, y) => (x.m_Rank - y.m_Rank)); // Get worst player score-wise

#if UNITY_EDITOR
            ShowCurrentLeaderboard();
#endif
            Eliminate(m_Players[m_Players.Count - 1]);
        }
        yield return (new WaitForSeconds(3f));
        m_GameManager.OnGameFinished();
    }


    public void Order()
	{
		m_Players.Sort((x, y) => (x.m_Rank - y.m_Rank)); // Get worst player score-wise
	}

	public void SetPlayers(List<Player> _Players)
	{
		m_Players = new List<Player>(_Players);
	}

	public void SetHumanPlayer(Player _Player)
	{
		m_HumanPlayer = _Player;
	}

	void Eliminate(Player _Player)
	{
		if (_Player is HumanPlayer)
            m_GameManager.OnGameFinished();
        else
            DestroyPlayer(_Player);
	}

    public void KillHumanPlayer()
    {
        DestroyPlayer(GetHumanPlayer());
    }

    private void DestroyPlayer(Player _Player)
    {
        _Player.Eliminate();
        m_Players.Remove(_Player);

        PowerUp_DeadMan powerUp = Instantiate(m_PowerUpOnDeathPrefab, _Player.transform.position, _Player.transform.rotation);
        powerUp.SetDeadPlayer(_Player);
        m_GameManager.AddMapObject(powerUp.gameObject);

        for (int i = 0; i < m_Players.Count; ++i)
        {
            m_Players[i].LevelUp();
        }
        if (onElimination != null)
            onElimination.Invoke(_Player);
    }

	public Player GetHumanPlayer()
	{
		return (m_HumanPlayer);
	}

	public Player GetWorstPlayer()
	{
		return (m_Players[m_Players.Count - 1]);
	}

	public Player GetBestPlayer()
	{
		return (m_Players[0]);
	}

	public Player GetPlayer(int _Rank)
	{
		if (_Rank >= m_Players.Count || _Rank < 0)
		{
			return (null);
		}
		return (m_Players[_Rank]);
	}

	public int GetAlivePlayersCount()
	{
		return (m_Players.Count);
	}

#if UNITY_EDITOR
	void ShowCurrentLeaderboard()
	{
		Debug.Log("==================");
		for (int i = 0; i < m_Players.Count; ++i)
		{
			Debug.Log("[" + m_Players[i].Name + "] " + (m_Players[i].Score * 100) + "%");
		}
	}
#endif

    public void EndGame()
    {
        m_IsPlaying = false;
        if (m_CountdownCoroutine != null)
            StopCoroutine(m_CountdownCoroutine);
    }
}
