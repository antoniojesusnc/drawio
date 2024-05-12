using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : SingletonMB<RankingManager> {

    public int m_PointsPerRank = 3000;
    public List<int> m_XPByRank;

    public int m_MaxRanks = 10;

    public int m_LastGain;

    public List<RankData> m_RankData;

    private void OnEnable()
    {
        m_RankData = new List<RankData>(Resources.LoadAll<RankData>("Ranks"));
    }

    public int GetPoints()
    {
        return (PlayerPrefs.GetInt(Constants.c_PlayerXPSave, 0));
    }

    private void LevelUp()
    {
        PlayerPrefs.SetInt(Constants.c_PlayerLevelSave, GetPlayerLevel() + 1);
    }

    private void LevelDown()
    {
        PlayerPrefs.SetInt(Constants.c_PlayerLevelSave, GetPlayerLevel() - 1);
    }

    public RankData GetRank(int _Level)
    {
        return (m_RankData[Mathf.Clamp(_Level, 0, m_RankData.Count - 1)]);
    }

    public int GetPlayerLevel()
    {
        return (PlayerPrefs.GetInt(Constants.c_PlayerLevelSave, 1));
    }
}
