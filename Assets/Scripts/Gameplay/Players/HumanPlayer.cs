using UnityEngine;
using System.Collections;

public class HumanPlayer : Player, IDirectionController
{
    private const float             c_SmoothMovementTurningSpeed = 17f;

    // Cache
    private MainCamera 		        m_MainCamera;
	private MessageView		        m_MessageView;
	private MobileHapticManager     m_HapticManager;
	private StatsManager            m_StatsManager;
    private ScreenShaker            m_ScreenShaker;

	// Buffers
	private bool			        m_IsMoving;
	private Vector3			        m_Input;
    private string                  m_Cohort;

    protected override void Awake ()
	{
		base.Awake ();

		// Cache
		m_MainCamera = MainCamera.Instance;
		m_MessageView = MessageView.Instance;
		m_HapticManager = MobileHapticManager.Instance;
		m_StatsManager = StatsManager.Instance;
        m_ScreenShaker = ScreenShaker.Instance;

        m_MainCamera.SetTarget(this);

		// Buffers
		m_IsMoving = false;
	}

	protected override void Update()
	{
		base.Update();

        if (!m_GameManager.m_IsPlaying)
            return;

        m_Direction = m_Input;
        if (m_IsMoving && m_Input.sqrMagnitude > Mathf.Epsilon)
            m_Transform.rotation = Quaternion.LookRotation(m_Input);

        m_Direction = m_Transform.forward * m_Input.magnitude;

        Vector3 endPos = m_Transform.position + m_Direction * Time.deltaTime;
        ClampPosition(ref endPos);
        m_Transform.position = endPos;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
		{
			AddBrush();
		}
        if (Input.GetKeyDown(KeyCode.E))
		{
			LevelUp();
		}
#endif
	}

	protected override void LateUpdate ()
	{
		base.LateUpdate ();

		m_MainCamera.SetDistance (m_WorldPercent);
	}

    protected override void Kill(Player _Player)
    {
        base.Kill(_Player);
        m_HapticManager.Vibrate(MobileHapticManager.E_FeedBackType.ImpactHeavy);
    }

    public override void Eliminate()
    {
        base.Eliminate();

        m_IsMoving = false;
        m_Input = Vector3.zero;
		m_HapticManager.Vibrate(MobileHapticManager.E_FeedBackType.Failure);
    }

    public override void Killed()
    {
        base.Killed();

        m_Brush.Blink(true);
        for (int i = 0; i < m_BrushesFollowing.Count; ++i)
        {
            m_BrushesFollowing[i].Blink(true);
        }

        m_ScreenShaker.Shake(0.5f, 0.2f);

        m_HapticManager.Vibrate(MobileHapticManager.E_FeedBackType.Failure);
    }

    protected override void Revive()
    {
        base.Revive();
    }

    public void OnStartMove()
	{
		if (m_IsEliminated)
			return;

		m_IsMoving = true;
		ChangeMoveStatus(true);
	}

	public void OnMove(Vector3 _Offset)
	{
        if (m_IsEliminated)
            return;
		if (m_IsMoving == false)
			OnStartMove();

		m_Input = _Offset * GetSpeed();
	}

	public void OnEndMove()
	{

        if (m_IsEliminated || (m_Cohort != null && m_Cohort.Equals(Constants.c_CohortForceMove)))
			return;
        
		m_IsMoving = false;
		m_Input = Vector3.zero;
		ChangeMoveStatus(false);
	}

	public override void LevelUp ()
	{
		base.LevelUp ();

        if (m_Cohort != null && m_Cohort.Equals(Constants.c_CohortSlimesIoGameplay))
		    m_MessageView.QueueMessage ("Level up!");
	}

	protected override void GetPowerUp(PowerUp _PowerUp)
	{
		base.GetPowerUp(_PowerUp);
		m_HapticManager.Vibrate(MobileHapticManager.E_FeedBackType.ImpactMedium);
	}

	public override void Init(string _Name, BrushData _Brush, Color _Color)
	{
		string playerName = m_StatsManager.GetNickname();

		if (string.IsNullOrEmpty(playerName))
			playerName = Constants.c_DefaultPlayerName;
		
		base.Init(playerName, _Brush, _Color);
	}

	public override bool IsMoving()
	{
		return m_IsMoving;
	}
}
