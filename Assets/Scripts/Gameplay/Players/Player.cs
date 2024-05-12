using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MappedObject, IDrawLine
{
	private const float 		c_LeaderboardRate = 1.0f;
    
	private const float			c_MinSpeed = 50.0f;
    private const float         c_MaxSpeed = 100.0f;

	private const float			c_MinSize = 4.0f;
	private const float			c_MaxSize = 10.0f;
	private const float         c_MaxFinalSize = 24.0f;
	private const float 		c_OffsetSize = 1.4f;
	private const float 		c_GrowDuration = 0.5f;
	private const int 			c_MaxGrowStepCount = 5;
	private const float			c_WorldPercentGrowUp = 0.15f;
	private const float			c_WorldPercentGrowDown = 0.1f;

    private const float         c_MaxForce = 250.0f;
	private const float         c_NearEdgeDistance = 6.0f;
	private const float			c_FragTimeout = 1.0f;

	private enum EBonus
	{
		SPEED_UP,
		SIZE_UP
	}

	public delegate void OnDeath();
	public event OnDeath onDeath;

    public delegate void OnRevive();
    public event OnRevive onRevive;

    public delegate void OnKilled();
    public event OnKilled onKilled;

    public delegate void OnMoveStatusChanged(bool _Moving);
	public event OnMoveStatusChanged onMoveStatusChanged;

	// Cache
	protected GameManager		m_GameManager;
	protected TerrainManager	m_TerrainManager;
	protected Brush				m_Brush;
	protected List<DrawLine> 	m_DrawLines;

	// Buffers
	protected Vector3			m_Direction;
	private Coroutine 			m_GrowCoroutine;
	protected List<GameObject>	m_SearchBuffer;
	private Coroutine           m_SpeedPowerUpCoroutine;
	private Coroutine           m_SizePowerUpCoroutine;

	// Runtime
	private string				m_PlayerName;
	public Color				m_Color;
	public int					m_ColorHash;
	public int					m_Rank;
	protected float				m_WorldPercent;
	private int 				m_GrowStep = 0;
	private float 				m_Size = c_MinSize;
	protected float				m_Speed = c_MinSpeed;
    public bool                 m_Invincible { get; private set; }
    private float               m_SizeSecondaryBrush;
    protected int               m_Level;
	private float 				m_Percent;
	public float 				percent { get { return m_Percent; } }

	// Bonuses
	private float				m_BonusStartTime;
	private float 				m_BonusDuration;
	private float				m_DeathStartTime;
    private float               m_RespawnStartTime;
    protected bool              m_IsEliminated = false;
    protected bool              m_IsDead = false;
	public bool                 isEliminated { get { return m_IsEliminated; } }
    public bool                 isDead { get { return m_IsDead; } }
	protected float 			m_SizeFactor = 1.0f;

	// Frags
	protected int				m_FragCount = 0;
	private Player 				m_FragOrg;
    
	// BrushHerd
	protected List<Brush>       m_BrushesFollowing = new List<Brush>();
	public BrushData            m_BrushData;
	private readonly Vector3    c_HerdOffset = new Vector3(0.86f, 0f, -0.54f);

    protected float m_DeadTime
    {
        get
        {
            return Time.time - m_DeathStartTime;
        }
    }

    public string Name 
	{
		get
		{
			return m_PlayerName;
		}
	}

	public virtual void Init (string _Name, BrushData _Brush, Color _Color)
	{
        m_SizeSecondaryBrush = Constants.c_SizeSecondaryMultipler;

        // Cache
		m_GameManager = GameManager.Instance;
		m_Transform = transform;
		m_Direction = Vector3.zero;
		m_TerrainManager = TerrainManager.Instance;
        m_DrawLines = new List<DrawLine>(1)
        {
            GetComponent<DrawLine>()
        };
		m_BrushData = _Brush;

		// Buffers
		m_SearchBuffer = new List<GameObject> ();

		// Runtime
		GameObject brushObject = Instantiate (_Brush.m_Prefab, m_Transform) as GameObject;
		m_Brush = brushObject.GetComponent<Brush> ();

        m_DrawLines[0].Init(this, m_Brush);

		m_Color = _Color;
		m_ColorHash = PaintSurface.GetColorHash (_Color);
		m_Brush.Init (this, Brush.EBrushType.MAIN);
		   
		m_PlayerName = _Name;
		m_WorldPercent = 0.0f;
        m_Level = 0;

		RegisterMap ();
        
		m_GameManager.onEndGame += OnEndGame;
	}

	void OnEndGame()
	{
		//Destroy(gameObject);
	}

	void OnDestroy()
	{
		m_GameManager.onEndGame -= OnEndGame;
	}

    protected void ChangeMoveStatus(bool _Moving)
	{
		if (onMoveStatusChanged != null)
			onMoveStatusChanged.Invoke(_Moving);
	}

	protected virtual void Update()
	{
        if (BattleRoyaleManager.Instance.m_IsPlaying == false)
        {
            if (m_Brush.IsBlinking())
                Blink(false);
            return;
        }
            
		if (m_IsEliminated)
			return;

        if (m_Invincible)
        {
            if (Time.time - m_RespawnStartTime > Constants.c_PlayerInvincibilityDuration)
            {
                m_Invincible = false;
                Blink(false);
            }
        }
        else if (m_Brush.IsBlinking() && m_Invincible == false && m_IsDead == false)
            Blink(false);
	}

    void Blink(bool _Value)
    {
        m_Brush.Blink(_Value);
        for (int i = 0; i < m_BrushesFollowing.Count; ++i)
            m_BrushesFollowing[i].Blink(_Value);
    }

	protected virtual void LateUpdate()
	{
        if (BattleRoyaleManager.Instance.m_IsPlaying == false)
            return;

		if (m_IsEliminated)
			return;

		float lerpFactor = 3.5f * Time.deltaTime;

		m_Transform.localScale = Vector3.Lerp(m_Transform.localScale, Vector3.one * GetSize(), lerpFactor);

		UpdateFollowingBrushes();

		ComputeCollisions ();
		UpdateMap ();
	}

	void UpdateFollowingBrushes()
	{
		Transform brushTransform;
		Vector3 target;
		for (int i = 0; i < m_BrushesFollowing.Count; ++i)
		{
			target = transform.TransformPoint(CalculateHerdOffset(i));
			ClampPosition(ref target);
			brushTransform = m_BrushesFollowing[i].transform;
			brushTransform.rotation = transform.rotation;
			brushTransform.position = Vector3.MoveTowards(brushTransform.position, target, Time.deltaTime * GetSpeed() * 1.3f);
            brushTransform.localScale = Vector3.one * GetSize() * GetBrushSizeMultiplier(m_BrushesFollowing[i].m_Type == Brush.EBrushType.SECONDARY);
		}
	}

    protected virtual void CheckLevelUp()
    {
        if (m_Level < Constants.c_GameplayMaxLevel - 1)
        {
            if (GetLevelUpRequiredPercentage() <= Score)
            {
                LevelUp();
                m_Level += 1;
            }
        }
    }

	public void CalculatePercentage ()
	{
		m_Percent = m_TerrainManager.GetColorPercent(m_ColorHash);
	}

    public float GetLevelUpRequiredPercentage(int _Level = -1)
    {
        if (_Level < 0)
            _Level = m_Level;
        return (Constants.c_GameplayRequiredPercentPerLevel[_Level]);
    }

    public float GetLevelUpPercentage()
    {
        float lastScoreRequired = 0f;
        if (m_Level > 0)
            lastScoreRequired = GetLevelUpRequiredPercentage(m_Level - 1);
        return (Mathf.Clamp((Score - lastScoreRequired) / (GetLevelUpRequiredPercentage() - lastScoreRequired), 0f, 1f));
    }

    public float Score
	{
		get 
		{
			return (m_Percent * TerrainManager.Instance.ScoreMultiplier);
		}
	}

	protected void ClampPosition(ref Vector3 _Position)
    {
		TerrainManager.Instance.ClampPosition(ref _Position, GetSize() / 1.5f);
        
    }

	private void ComputeCollisions()
	{
		float size = GetSize () * 1.2f + 4f;

		ComputeRadiusSubCollisions(m_Transform.position, size);
		ComputeCloseSubCollisions(m_Transform.position, GetSize());
		
		for (int i = 0; i < m_BrushesFollowing.Count; ++i)
		{
			ComputeRadiusSubCollisions(m_BrushesFollowing[i].transform.position, size);
		}      
	}
	
	private void ComputeRadiusSubCollisions(Vector3 center, float size)
	{
        if (isDead == true)
            return;

		MapManager.FindEntities(center, size * size, ref m_SearchBuffer);

        for (int i = 0; i < m_SearchBuffer.Count; ++i)
        {
	        CheckForPowerUp(m_SearchBuffer[i]);
        }
	}
	
	private void ComputeCloseSubCollisions(Vector3 center, float size)
	{
		if (isDead == true)
			return;

		MapManager.FindEntities(center, size * size, ref m_SearchBuffer);

		for (int i = 0; i < m_SearchBuffer.Count; ++i)
		{
			CheckForPlayer(m_SearchBuffer[i]);
		}
	}

	private void CheckForPowerUp(GameObject collisionGameObject)
	{
		PowerUp powerUp = collisionGameObject.GetComponent<PowerUp>();
		if (powerUp != null && powerUp.ready)
			GetPowerUp(powerUp);
	}
	
	private void CheckForPlayer(GameObject collisionGameObject)
	{
		Player player = collisionGameObject.GetComponent<Player>();
		if (player != null && player != this)
			PushAway(player);
	}

	public void PushAway(Player otherPlayer, bool pushOtherPlayer = true)
	{
		var finalPos = transform.position -
		               (otherPlayer.transform.position - transform.position).normalized * otherPlayer.GetSize();
		finalPos.Set(finalPos.x, transform.position.y, finalPos.z);
		SetFinalPosition(finalPos);
		if (pushOtherPlayer)
		{
			otherPlayer.PushAway(this, pushOtherPlayer:false);
		}
	}

	protected void SetFinalPosition(Vector3 position)
	{
		ClampPosition(ref position);
		m_Transform.position = position;
	}

	protected virtual void Kill(Player _Player)
    {
        _Player.Killed();
        m_TerrainManager.FillCircle(this, _Player.m_Transform.position, Constants.c_KillPlayerSplashRadius, Constants.c_KillPlayerSplashDuration);
        for (int i = 0; i < _Player.m_BrushesFollowing.Count; ++i)
        {
            m_TerrainManager.FillCircle(this, _Player.m_BrushesFollowing[i].transform.position, Constants.c_KillPlayerSplashRadius, Constants.c_KillPlayerSplashDuration);
        }
    }

	protected virtual void GetPowerUp(PowerUp _PowerUp)
	{
		_PowerUp.OnPlayerTouched(this);
	}

	public virtual void Frag()
	{
		m_FragCount++;
	}

	public virtual void LevelUp()
	{
		m_GrowStep++;
		m_WorldPercent += c_WorldPercentGrowUp;
		Grow (m_Size + c_OffsetSize);
	}

	public virtual void LevelDown()
	{
		m_GrowStep--;
		m_WorldPercent -= c_WorldPercentGrowUp;
		Grow (m_Size - c_OffsetSize);
	}

	private void Grow(float _TargetSize)
	{
		if (m_GrowCoroutine != null)
			StopCoroutine (m_GrowCoroutine);
		
		m_GrowCoroutine = StartCoroutine(GrowCoroutine(_TargetSize));
	}

	private IEnumerator GrowCoroutine(float _TargetSize)
	{
		float baseSize = m_Size;
		float time = 0.0f;
		while (time < 1.0f)
		{
			time += Time.deltaTime / c_GrowDuration;
			m_Size = Mathf.Lerp (baseSize, _TargetSize, time);
			yield return null;
		}
	}

    public virtual void Killed()
    {
        if (onKilled != null)
            onKilled.Invoke();

        Die();

        StartCoroutine(ReviveCoroutine());
    }

    public virtual void Die()
    {
        m_IsDead = true;
        m_DeathStartTime = Time.time;

        if (onDeath != null)
            onDeath.Invoke();
        
        for (int i = 0; i < m_DrawLines.Count; ++i)
            m_DrawLines[i].StopDraw();

        m_DeathStartTime = Time.time;
        StopAllCoroutines();
    }

    public void ShowVisual()
    {
        SetVisible(true);
    }

    public void HideVisual()
    {
        SetVisible(false);
    }

    public virtual void SetVisible(bool _Visible)
    {
        m_Brush.SetVisible(_Visible);
        for (int i = 0; i < m_BrushesFollowing.Count; ++i)
        {
            m_BrushesFollowing[i].SetVisible(_Visible);
        }
    }

	public virtual void Eliminate()
	{
        m_IsEliminated = true;
        Die();

        m_Brush.Fall (true);

		for (int i = 0; i < m_BrushesFollowing.Count; ++i)
        {
			m_BrushesFollowing[i].Fall(true);
        }

        StartCoroutine (EliminateCoroutine ());
    }

	private IEnumerator EliminateCoroutine()
	{
		yield return new WaitForSeconds (3.0f);
		m_Brush.gameObject.SetActive (false);
		for (int i = 0; i < m_BrushesFollowing.Count; ++i)
		{
			m_BrushesFollowing[i].gameObject.SetActive(false);
		}
	}

    protected IEnumerator ReviveCoroutine()
    {
        yield return new WaitForSeconds(Constants.c_PlayerDeathDuration);

        Revive();
    }

    protected virtual void Revive()
    {
        for (int i = 0; i < m_DrawLines.Count; ++i)
            m_DrawLines[i].StartDraw();

        m_IsDead = false;

        if (onRevive != null)
            onRevive.Invoke();

        m_RespawnStartTime = Time.time;
        m_Invincible = true;
        Blink(true);
    }

    #region IDrawLine impl

    public void AddPoint(Vector3 _Pos, bool _IsSubBrush = false)
    {
		m_TerrainManager.AddCircle(_Pos, GetSize() * 0.5f * GetBrushSizeMultiplier(_IsSubBrush), m_ColorHash, ref m_Color);
    }

    private float GetBrushSizeMultiplier(bool m_IsSubBrush)
    {
        return m_IsSubBrush ? m_SizeSecondaryBrush : 1.0f;
    }

	#endregion

	#region Internal

	protected float GetSpeed()
	{
		return Mathf.Clamp(m_Speed, c_MinSpeed, c_MaxSpeed);
	}

	public float GetSize()
	{
		return Mathf.Clamp(m_Size * m_SizeFactor, 1f, c_MaxFinalSize);
	}

    public float GetMinSize()
    {
        return (c_MinSize);
    }

	#endregion

	#region Bonuses

	private IEnumerator BonusCoroutine(EBonus _Bonus, float _Duration)
	{
		yield return new WaitForSeconds (_Duration);

		switch (_Bonus)
		{
		    case EBonus.SIZE_UP:
                m_SizeFactor = 1.0f;
			    break;
		}
	}

	public virtual void AddSizeUp(float _Factor, float _Duration)
	{
		m_SizeFactor = _Factor;
		if (m_SizePowerUpCoroutine != null)
			StopCoroutine(m_SizePowerUpCoroutine);
		m_SizePowerUpCoroutine = StartCoroutine (BonusCoroutine(EBonus.SIZE_UP, _Duration));
	}

	#endregion   

	public virtual void AddBrush()
	{
		GameObject brushObject = Instantiate(m_BrushData.m_Prefab, m_Transform.position, m_Transform.rotation) as GameObject;
        Brush brush = brushObject.GetComponent<Brush>();

		brush.Init(this, Brush.EBrushType.SECONDARY);

		//brush.transform.position = m_Transform.TransformPoint(CalculateHerdOffset());
		DrawLine drawLine = brushObject.AddComponent<DrawLine>();
        drawLine.Init(this, brush);
		drawLine.m_DrawAtStart = true;
		m_DrawLines.Add(drawLine);
		m_BrushesFollowing.Add(brush);
	}

	Vector3 CalculateHerdOffset(int index)
	{
		Vector3 result;
        int line = index / 2 + 1;
		float side = index % 2 == 0 ? -1f : 1f;

        result.x = c_HerdOffset.x * line * side * m_SizeSecondaryBrush;
		result.y = 0f;
		result.z = c_HerdOffset.x * line * -1f;

		return (result);
	}

	public virtual bool IsMoving()
    {
        return (true);
    }

    public virtual void Remove()
    {
        for (int i = 0; i < m_BrushesFollowing.Count; ++i)
        {
            Destroy(m_BrushesFollowing[i].gameObject);
        }
        Destroy(gameObject);
    }
}
