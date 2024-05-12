using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[DefaultExecutionOrder(20)]
public class PreEndView : View<PreEndView> {

	public EndView m_EndView;
	public Text m_CurrentXPText;
    public Text m_CurrentLevelText;
    public Text m_NextLevelText;
    public Text m_XBLeftText;
    public Image m_XPBar;
	public Text m_RankText;
    public List<Image> m_ColoredImages;

	private BattleRoyaleManager m_BattleRoyaleManager;
	private StatsManager m_StatsManager;
	private int m_XP;
	private int m_XPGain;
	private int m_Level;

    private int m_LastGain;

	protected override void Awake()
	{
		base.Awake();

		m_BattleRoyaleManager = BattleRoyaleManager.Instance;
		m_StatsManager = StatsManager.Instance;
	}

    private void Start()
    {
        m_XBLeftText.text = "";
        m_XP = m_StatsManager.GetXP();
        m_Level = m_StatsManager.GetPlayerLevel();
    }

    protected override void Update()
    {
        base.Update();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(PreEndCoroutine());
#endif
    }

	protected override void OnGamePhaseChanged(GamePhase _GamePhase)
	{
		base.OnGamePhaseChanged(_GamePhase);
        switch (_GamePhase)
		{
			case GamePhase.GAME: //Save level and xp before they got changed

				
				m_XPGain = 0;
				break;
			case GamePhase.END:
                
				break;
		}
	}

    public void LaunchPreEnd()
    {
        m_XP = m_StatsManager.GetXP();
        m_Level = m_StatsManager.GetPlayerLevel() - 1;
        Transition(true);
		Display(m_BattleRoyaleManager.GetHumanPlayer().m_Color);
        StartCoroutine(PreEndCoroutine());
    }

    void Display(Color _Color)
	{
		int ranking = m_BattleRoyaleManager.GetHumanPlayer().m_Rank + 1;
		string rankString;
		switch (ranking)
		{
			case 1:
				rankString = "st";
				break;
			case 2:
                rankString = "nd";
                break;
			case 3:
                rankString = "rd";
                break;
			default:
				rankString = "th";
				break;
		}

		m_RankText.text = ranking.ToString() + "<size=140>" + rankString + "</size>";
		m_RankText.color = _Color;

		for (int i = 0; i < m_ColoredImages.Count; ++i)
            m_ColoredImages[i].color = _Color;
		
        SetXPBar(m_Level, m_XP);
        Color color = m_XBLeftText.color;
        color.a = 1f;
        m_XBLeftText.color = color;

    }

    void SetXPBar(int _CurrentLevel, int _CurrentXP)
	{
		float levelPercent = (float)_CurrentXP / (float)m_StatsManager.XPToNextLevel(_CurrentLevel);
        m_XPBar.gameObject.SetActive(levelPercent > 0.02f);

		m_CurrentXPText.text = _CurrentXP.ToString() + "/" + m_StatsManager.XPToNextLevel(_CurrentLevel);
        m_CurrentLevelText.text = (_CurrentLevel + 1).ToString();
		m_NextLevelText.text = (_CurrentLevel + 2).ToString();
        m_XPBar.rectTransform.anchorMax = new Vector2(levelPercent, 1f);
        m_XPBar.rectTransform.anchoredPosition = Vector2.zero;
	}

    void UpdateBar(int _XP)
	{
        m_XP += _XP - m_XPGain;

        if (m_XP > m_StatsManager.XPToNextLevel(m_Level))
		{
            m_XP -= m_StatsManager.XPToNextLevel(m_Level);
			m_Level += 1;
		
		}

		SetXPBar(m_Level, m_XP);
		m_XPGain = _XP;
        m_XBLeftText.text = (m_StatsManager.m_LastGain > 0 ? "+" : "") + (m_StatsManager.m_LastGain  - _XP).ToString();
	}

    IEnumerator PreEndCoroutine()
    {
        m_LastGain = m_StatsManager.m_LastGain;

        m_XPGain = 0;
        yield return new WaitForSeconds(1.5f);
        DOTween.To(() => m_XPGain, UpdateBar, m_LastGain, 1.3f).OnComplete(() => m_XBLeftText.DOFade(0f, 0.25f));
		yield return new WaitForSeconds(2.5f);
        Transition(false);
		yield return new WaitForSeconds(0.1f);
		m_EndView.Display();
	}
}
