using UnityEngine;

public static class Constants
{
	// Paths
	public const string			c_ZoningPath = "Zonings";

    // Pooling
    public const string         c_WheelPoolName = "WheelPool";
    public const string         c_CubePoolName = "CubePool";
	public const string         c_GrabEffectPoolName = "GrabEffectPool";
	public const string 		c_ConfettiPoolName = "ConfettiPool";

    // Save
    public const string         c_LevelSave = "Level";
	public const string			c_GameResultSave = "GameResult";
    public const string         c_BestScoreSave = "BestScore";
	public const string         c_PlayerNameSave = "Nickname";
	public const string         c_PlayerXPSave = "XP";
    public const string         c_PlayerLevelSave = "Lvl";
    public const string         c_VibrationSave = "Vibration";
    public const string         c_PlayerCoins = "Coins";
    public const string         c_PurchasedSkins = "PurchasedSkins";
    
    // Cohorts
    public const string         c_CohortScalingBombs = "ScalingBombs";
    public const string         c_CohortForceMove = "ForceMove";
    public const string         c_CohortSlimesIoGameplay = "SlimesIoGameplay";
    public const string         c_CohortFasterGames = "FasterGames";
    public const string         c_CohortNewRanking = "NewRanks";
    public const string         c_CohortTournament = "Tournament";
    public const string         c_CohortCubicPaint = "CubicPaint";
    public const string         c_CohortFasterEliminations = "FasterEliminations";
    public const string         c_CohortSmoothMovement = "SmoothMovement";
    public const string         c_CohortSixPlayers = "SixPlayers";

    // Gameplay
    public static int           s_PlayerCount = 8;
    public const string         c_DefaultPlayerName = "You";
    public const int            c_MaxLevel = 100;
    public const float          c_CubeOffsetDistance = 1.0f;
    public const float          c_GenDistance = 50.0f;
    public const float          c_SpawnBorderOffset = 10.0f;
	public const float			c_MaxTime = 130.0f;
	public const float          c_PlayerDeathDuration = 2f;
	public const float          c_PowerUpPreWarm = 0.5f;
	public const int			c_SavedGameCount = 5;
    public const float          c_PlayerInvincibilityDuration = 1.5f;
	public const float          c_PlayerCountdownHelp = 5f;
    public const float          c_SizeSecondaryMultipler = 0.75f;
    public const float          c_KillPlayerSplashRadius = 10f;
    public const float          c_KillPlayerSplashDuration = 0.2f;
    public const int            c_GameplayMaxLevel = 7;
    public static readonly float[] c_GameplayRequiredPercentPerLevel = { 0.02f, 0.05f, 0.10f, 0.15f, 0.20f, 0.25f, 0.25f };

    // IAP
    public const string			c_NoAdsBundleID = "com.voodoo.drawdotio.noads";
    
    // Daily Reward
    public const string c_DailyRewardLastClaimedDataKey = "DailyRewardLastClaimedDataKey";
    public const string c_DailyRewardLastClaimedIndexKey = "DailyRewardLastClaimedKey";
    
}
