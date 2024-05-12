using UnityEngine;
using UnityEngine.UI;

public class RankingView : MonoBehaviour
{
    private StatsManager m_StatsManager;

    public Text m_PreviousRank;
    public Text m_ActualRank;
    public Text m_NextRank;

    public Text m_CurrentXPText;
    public Text m_CurrentLevelText;
    public Text m_NextLevelText;
    public Image m_XPBar;
    public Image m_RankIcon;

    public Text m_LevelCrownText;

    private void Awake()
    {
        m_StatsManager = StatsManager.Instance;
    }

    public void RefreshNormal()
    {
        int tmpLvl = m_StatsManager.GetPlayerLevel() - 1;
        if (tmpLvl >= MainMenuView.Instance.m_Ratings.Length)
        {
            tmpLvl = MainMenuView.Instance.m_Ratings.Length - 1;
        }

        m_CurrentXPText.text = m_StatsManager.GetXP().ToString() + "/" + m_StatsManager.XPToNextLevel(m_StatsManager.GetPlayerLevel() - 1).ToString();
        m_CurrentLevelText.text = m_StatsManager.GetPlayerLevel().ToString();
        m_LevelCrownText.text = "LVL" + m_StatsManager.GetPlayerLevel().ToString();
        m_NextLevelText.text = (m_StatsManager.GetPlayerLevel() + 1).ToString();

        float levelPercent = (float)m_StatsManager.GetXP() / (float)m_StatsManager.XPToNextLevel(m_StatsManager.GetPlayerLevel() - 1);
        m_XPBar.rectTransform.anchorMax = new Vector2(levelPercent, 1f);
        m_XPBar.rectTransform.anchoredPosition = Vector2.zero;
        m_XPBar.gameObject.SetActive(levelPercent > 0.02f);

        m_PreviousRank.text = GetNameByRank(tmpLvl - 1);
        m_ActualRank.text = GetNameByRank(tmpLvl);
        m_NextRank.text = GetNameByRank(tmpLvl + 1);
    }

    private string GetNameByRank(int _Rank)
    {
        if (_Rank < 0)
            return "";
        if (_Rank > MainMenuView.Instance.GetRankingCount() - 1)
            return "";
        return MainMenuView.Instance.GetRanking(_Rank);
    }

}
