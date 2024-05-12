using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class StatsManager : SingletonMB<StatsManager>
{
	public List<int>    m_XPForLevel;
	public int          m_LastGain = 0;

    public int FavoriteSkin
    {
        get
        {
            return (PlayerPrefs.GetInt("FavoriteSkin", 0));
        }
        set
        {
            PlayerPrefs.SetInt("FavoriteSkin", value);
        }
    }

    private int GetGameResult(int _Index)
	{
		string key = Constants.c_GameResultSave + "_" + _Index.ToString ();

		if (PlayerPrefs.HasKey(key))
			return PlayerPrefs.GetInt(key);
		else
			return 0;
	}

	public void AddGameResult(int _WinScore)
	{
		// Move results
		for (int i = Constants.c_SavedGameCount - 1; i >= 0; --i)
		{
			string key = Constants.c_GameResultSave + "_" + i.ToString ();
			PlayerPrefs.SetInt (key, GetGameResult (i - 1));
		}

		// Set new result
		PlayerPrefs.SetInt (Constants.c_GameResultSave + "_0", _WinScore);
	}

	public float GetLevel()
	{
		int result = 0;

		for (int i = 0; i < Constants.c_SavedGameCount; ++i)
			result += GetGameResult (i);
            
		float percent = ((float)result) / ((float)Constants.c_SavedGameCount);
		return Mathf.Clamp01(percent);
	}

	public void TryToSetBestScore(int _Score)
	{
		int score = GetBestScore ();
		if (score < _Score)
		{
			PlayerPrefs.SetInt(Constants.c_BestScoreSave, _Score);
		}
	}

	public int GetBestScore()
	{
		if (PlayerPrefs.HasKey(Constants.c_BestScoreSave))
			return PlayerPrefs.GetInt(Constants.c_BestScoreSave);
		else
			return 0;
	}

    public void SetNickname(string _Name)
	{
			PlayerPrefs.SetString(Constants.c_PlayerNameSave, _Name);
	}

    public string GetNickname()
	{
		return (PlayerPrefs.GetString(Constants.c_PlayerNameSave, null));
	}

    public void SetLastXP(int _XP)
    {
        m_LastGain = _XP;
    }

    public void GainXP()
    {
        
        int _XP  = m_LastGain;


            int xp = _XP + GetXP();

            while (xp >= XPToNextLevel())
            {
                xp -= XPToNextLevel();
                LevelUp();
            }
            PlayerPrefs.SetInt(Constants.c_PlayerXPSave, xp);

	}
    
	public int GetXP()
	{
		return (PlayerPrefs.GetInt(Constants.c_PlayerXPSave, 0));
	}

	public int GetPlayerLevel()
	{
		return (PlayerPrefs.GetInt(Constants.c_PlayerLevelSave, 1));
	}

    void LevelUp()
	{
		PlayerPrefs.SetInt(Constants.c_PlayerLevelSave, GetPlayerLevel() + 1);
	}

    void LevelDown()
    {
        PlayerPrefs.SetInt(Constants.c_PlayerLevelSave, GetPlayerLevel() - 1);
    }

	public int XPToNextLevel(int _LevelStart = -1)
	{
		int currentLevel = _LevelStart == -1 ? GetPlayerLevel() - 1 : _LevelStart;
		int index = Mathf.Min(currentLevel, m_XPForLevel.Count - 1);
		return (m_XPForLevel[index]);
	}

	#region IAs

	// Behaviour probas
	private const int 			c_MaxRandomProbaLevel = 50;
	private const float 		c_FirstMinRandomProba = 0.1f;
	private const float 		c_FirstMaxRandomProba = 0.2f;
	private const float 		c_SecondMinRandomProba = 0.15f;
	private const float 		c_SecondMaxRandomProba = 0.3f;

	// Random duration
	private const int 			c_MaxRandomDurationLevel = 50;
	private const float 		c_FirstMinRandomDuration = 10.0f;
	private const float 		c_FirstMaxRandomDuration = 20.0f;
	private const float 		c_SecondMinRandomDuration = 5.0f;
	private const float 		c_SecondMaxRandomDuration = 10.0f;

	public AnimationCurve		m_RandomProbaCurve;
	public AnimationCurve		m_RandomDurationCurve;

	public float GetRandomProba()
	{
		return GetRandomValue (c_MaxRandomProbaLevel, c_FirstMinRandomProba, c_FirstMaxRandomProba, c_SecondMinRandomProba, c_SecondMaxRandomProba, m_RandomProbaCurve);
	}

	public float GetRandomDuration()
	{
		return GetRandomValue (c_MaxRandomDurationLevel, c_FirstMinRandomDuration, c_FirstMaxRandomDuration, c_SecondMinRandomDuration, c_SecondMaxRandomDuration, m_RandomDurationCurve);
	}

	private float GetRandomValue(int _MaxLevel, float _FirstMin, float _FirstMax, float _SecondMin, float _SecondMax, AnimationCurve _Curve)
	{
		float level = GetLevel();
		float percent = _Curve.Evaluate(level / ((float)_MaxLevel));
		float minValue = Mathf.Lerp(_FirstMin, _SecondMin, _Curve.Evaluate(percent));
		float maxValue = Mathf.Lerp(_FirstMax, _SecondMax, _Curve.Evaluate(percent));
		return Random.Range(minValue, maxValue);
	}

	#endregion

	#region Coins

	public int GetCoins()
	{
		return (PlayerPrefs.GetInt(Constants.c_PlayerCoins, 0));
	}
	
	public void AddCoins(int coinsToAdd)
	{
		PlayerPrefs.SetInt(Constants.c_PlayerCoins, GetCoins()+coinsToAdd);
	}

	#endregion
}
