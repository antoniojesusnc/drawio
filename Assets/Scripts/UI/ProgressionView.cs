using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.PostProcessing;

public class ProgressionView : View<ProgressionView>
{
	public const string         c_TimeUpMessage = "";

	public Text 				m_Countdown;
	public GameObject			m_PlayerSlot;
	public List<Image>          m_PercentBars = new List<Image>();
	public List<Image>          m_PercentSeparator = new List<Image>();
	public Image                m_Crown;
	public Text                 m_CurrentPercent;
	public Text                 m_TimeBeforeElimination;
	public GameObject           m_TimeAndPercentObject;

    public Image                m_BackgroundPlayerColor;

    public Text                 m_CountDownStart;

    public Sprite m_SkullIcon;
    public Sprite m_CrownIcon;

	// Cache
	private BattleRoyaleManager m_BattleRoyaleManager;
    private CanvasGroup         m_TimeAndPercentCanvasGroup;
	private ScreenShaker 		m_ScreenShaker;

    private int m_LastSec = 30;

    private bool m_IsInfoDisplay = false;
    private int m_SecondsBuffer = 0;

	protected override void Awake()
	{
		base.Awake ();

		// Cache
		m_BattleRoyaleManager = BattleRoyaleManager.Instance;
        m_CountDownStart.transform.localScale = Vector2.zero;
        m_TimeAndPercentCanvasGroup = m_TimeAndPercentObject.GetComponent<CanvasGroup>();
		m_ScreenShaker = ScreenShaker.Instance;

        HideGroup();
	}

	protected override void OnGamePhaseChanged(GamePhase _GamePhase)
	{
		base.OnGamePhaseChanged (_GamePhase);

		switch (_GamePhase)
		{
		case GamePhase.MAIN_MENU:
				m_Crown.gameObject.SetActive(false);
				m_TimeAndPercentObject.SetActive(true);
			break;

            case GamePhase.LOADING:
                {
					List<Player> players = m_GameManager.m_Players;

					for (int i = 0; i < m_PercentBars.Count && i < players.Count; ++i)
						m_PercentBars[i].color = players[i].m_Color;
				
					m_CountDownStart.color = players[0].m_Color;
                    break;
                }

		case GamePhase.GAME:
				{
                    m_TimeBeforeElimination.text = (m_BattleRoyaleManager.GetTimeBeforeNextElimination()).ToString("0");
                    Transition(true);
                    break;
				}

        case GamePhase.PRE_END:
                {
                    
                    if (m_Visible)
                        Transition(false);
                    break;
                }

		case GamePhase.END:
				Transition(false);
				m_Crown.gameObject.SetActive(false);
				m_Countdown.text = c_TimeUpMessage;
			break;
		}
	}

    public void SetCountDownTime(int _Seconds)
    {
        m_CountDownStart.transform.localScale = Vector2.zero;
        m_CountDownStart.text = _Seconds.ToString();
        m_CountDownStart.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.InOutQuart).OnComplete(() => m_CountDownStart.transform.DOScale(Vector3.zero, 0.1f));
    }

	public void SetRemainingTime(float _Time)
	{
		int minCount = Mathf.FloorToInt(_Time / 60.0f);
		int secCount = Mathf.FloorToInt(_Time % 60.0f);
		m_Countdown.text = minCount.ToString ("0") + ":" + secCount.ToString ("00");
	}

    public void HideGroup()
    {
        m_TimeAndPercentCanvasGroup.alpha = 0;
    }

    public void ShowGroup()
    {
        m_TimeAndPercentCanvasGroup.DOFade(1, 0.5f);
    }


	public void UpdateView()
	{
        if (m_GameManager.currentPhase == GamePhase.GAME && BattleRoyaleManager.Instance.m_IsPlaying) 
        {
			Player bestPlayer = m_GameManager.GetBestPlayer();
			List<Player> players = m_GameManager.m_Players;

            // Displaying Crown for best team
			if (bestPlayer != null)
			{
				if (m_Crown.gameObject.activeSelf == false)
				    m_Crown.gameObject.SetActive(true);
				m_Crown.color = bestPlayer.m_Color;
			}
            
            // Updating and calculating total percentage covered
			float totalPercentCovered = 0f;

			for (int i = 0; i < m_PercentBars.Count && i < players.Count; ++i) {
				totalPercentCovered += players[i].percent;
			}

			// Updating the bars
			// Last team is always the rest of the bar so skipping it
			float currentPercentage = 0f;
            
			for (int i = 0; i < m_PercentBars.Count - 1 && i < players.Count - 1; ++i)
			{
				Image separator = m_PercentSeparator[i];
				currentPercentage += players[i].percent;
				float relativePercent = currentPercentage / totalPercentCovered;
				m_PercentBars[i].fillAmount = relativePercent;
				separator.rectTransform.anchorMin = new Vector2(relativePercent, 0f);
				separator.rectTransform.anchorMax = new Vector2(relativePercent, 1f);
				separator.rectTransform.anchoredPosition = Vector2.zero;
            }
            
			m_CurrentPercent.text = (m_BattleRoyaleManager.GetHumanPlayer().Score * 100f).ToString("0") + "%";
            if (m_BattleRoyaleManager.GetAlivePlayersCount() > 1)
            {
                if (m_LastSec != (int)Mathf.Ceil(m_BattleRoyaleManager.GetTimeBeforeNextElimination() - 0.5f))
                {
                    m_SecondsBuffer++;

                    if (m_SecondsBuffer > 4)
                    {
                        if (m_IsInfoDisplay == false)
                        {
                            m_IsInfoDisplay = true;
                            ShowGroup();
                        }

                        if (m_BattleRoyaleManager.GetTimeBeforeNextElimination() > 5.5f)
							UpdateAnimationScreenshaking(0.3f, 1f);
                    }

                    if (m_BattleRoyaleManager.GetTimeBeforeNextElimination() <= 5.5f)
                    {
                        m_TimeAndPercentObject.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 0, 0).SetEase(Ease.OutSine);

                        if (m_BattleRoyaleManager.GetTimeBeforeNextElimination() > 4.5f)
							UpdateAnimationScreenshaking(0.4f, 0.5f, true);
                        else
							UpdateAnimationScreenshaking(0.4f, 0.5f);
                    }

                    m_LastSec = (int)Mathf.Ceil(m_BattleRoyaleManager.GetTimeBeforeNextElimination() - 0.5f);
                }

                m_TimeBeforeElimination.text = (m_BattleRoyaleManager.GetTimeBeforeNextElimination()).ToString("0");
            }
			else
				m_TimeAndPercentObject.SetActive(false);
		}
	}

    private void UpdateAnimationScreenshaking(float _Intensity, float _Time, bool _Force = false)
    {
        if (m_BattleRoyaleManager.GetWorstPlayer() == m_BattleRoyaleManager.GetHumanPlayer())
		    m_ScreenShaker.Shake (_Intensity, 0.1f);
    }
}