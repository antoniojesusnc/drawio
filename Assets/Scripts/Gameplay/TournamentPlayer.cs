using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentPlayer
{
    public bool     m_Human;
    public int      m_Round;
    public string   m_PlayerName;
    public int      m_BrushIndex;
	public int		m_ColorIndex = -1;
    public Player   Player { get; set; } // Used to link to a real player during gameplay
    public float    m_Score;
    public bool     m_Alive;

    public TournamentPlayer(bool _Human)
    {
        StatsManager statsManager = StatsManager.Instance;
        GameManager gameManager = GameManager.Instance;

        m_Human = _Human;
        m_Round = 1;
        m_Alive = true;

        if (_Human)
        {
            m_PlayerName = statsManager.GetNickname();
            m_BrushIndex = gameManager.ComputeCurrentPlayeBrushIndex(true, 0);
            m_ColorIndex = 0;
        }
        else // If Bot
        {
            m_PlayerName = gameManager.PickRandomName();
            m_BrushIndex = gameManager.PickBrushID();
        }
    }

    public void Eliminate()
    {
        m_Alive = false;
    }
}
