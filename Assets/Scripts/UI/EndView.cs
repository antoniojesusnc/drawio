using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerSlot : MonoBehaviour
{
    public Image m_DeathImage;
    public Image m_FillBar;
    public Text m_Text; // Player name and rank

    public void Refresh(int _Rank, string _Name, float _Percent, Color _Color, bool _Dead)
    {
        m_DeathImage.enabled = _Dead;
        m_FillBar.color = _Color;
        m_Text.text = "<b>" + (_Rank + 1).ToString("D2") + "-" + _Name + "</b> " + (_Percent * 100.0f).ToString("0") + "%";
    }
}

public class EndView : View<EndView>
{
    private const float c_RefreshTime = 1f;

    public List<GameObject> m_PlayerSlots;
    public RectTransform m_Separator;
    public GameObject m_ContinueButton;
    public Text m_RankText;

    private List<Image> m_BarImages;
    private List<Image> m_DeathImages;
    private List<Text> m_Texts;
    private int m_DeadId;
    private float m_RefreshTimer;

    private BattleRoyaleManager m_BattleRoyaleManager;

    protected override void Awake()
    {
        base.Awake();

        m_BattleRoyaleManager = BattleRoyaleManager.Instance;

        m_BarImages = new List<Image>();
        m_DeathImages = new List<Image>();
        m_Texts = new List<Text>();

        for (int i = 0; i < m_PlayerSlots.Count; ++i)
        {
            Transform slotTr = m_PlayerSlots[i].transform;
            Transform uiTr = slotTr.Find("UI");
            Transform barTr = uiTr.Find("Bar");

            Image deathImage = uiTr.Find("DeathImage").GetComponent<Image>();
            Image barImage = barTr.Find("FillBar").GetComponent<Image>();
            Text text = barTr.Find("Text").GetComponent<Text>();

            m_BarImages.Add(barImage);
            m_DeathImages.Add(deathImage);
            m_Texts.Add(text);
        }
    }

    public void OnContinueButton()
    {
        if (m_GameManager.currentPhase == GamePhase.END)
            SceneManager.LoadScene("Game");
    }

    IEnumerator TickingPercents(Text _Text, int _Percent)
    {
        int currentPercent = 0;
        while (currentPercent <= _Percent)
        {
            _Text.text = currentPercent.ToString() + "% COVERED";
            ++currentPercent;
            yield return (new WaitForSeconds(0.008f));
        }
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        m_ContinueButton.SetActive(_GamePhase == GamePhase.END);

        switch (_GamePhase)
        {
            case GamePhase.MAIN_MENU:
                if (m_Visible)
                    Transition(false);

                m_DeadId = m_PlayerSlots.Count;
                break;
        }
    }

    public void Display()
    {
        Transition(true);
        m_DeadId = m_PlayerSlots.Count;
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
		m_RankText.color = m_BattleRoyaleManager.GetHumanPlayer().m_Color;

        for (int i = 0; i < m_PlayerSlots.Count; ++i)
        {
            m_PlayerSlots[i].SetActive(i < Constants.s_PlayerCount);
        }
    }

    public void RefreshPlayerSlot(int _Rank, string _Name, float _Percent, Color _Color, bool _Dead)
    {
        m_Texts[_Rank].text = "<b>" + (_Rank + 1).ToString("D2") + "-" + _Name + "</b> " + (_Percent * 100.0f).ToString("0") + "%";
        m_BarImages[_Rank].color = _Color;
        m_DeathImages[_Rank].gameObject.SetActive(_Dead);

        if (_Dead && _Rank < m_DeadId)
        {
            m_DeadId = _Rank;
            //m_Separator.SetSiblingIndex (m_DeadId);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (m_RefreshTimer <= Time.time)
        {
            m_RefreshTimer = Time.time + c_RefreshTime;
            Player player;
            for (int i = 0; i < m_GameManager.m_Players.Count; ++i)
            {
                player = m_GameManager.m_Players[i];
				RefreshPlayerSlot(player.m_Rank, player.Name, player.Score, player.m_Color, player.isEliminated);
            }
        }
    }
}