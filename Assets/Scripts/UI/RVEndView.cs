using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RVEndView : View<RVEndView> {

    public Button   m_ReviveButton;
    public Button   m_ContinueButton;

    public Image    m_ReviveFillImage;
    public Image    m_BottomReviveImage;

    public float    m_TimerRevive = 10;
    private float   m_Timer = 0;

    private bool    m_Active = false;
    private Color   _MainColor;

    public List<GameObject> m_BrushesPrefabs;

    protected override void Awake()
    {
        m_Active = false;
        base.Awake();
        m_GameManager = GameManager.Instance;
        m_ReviveButton.onClick.AddListener(OnClickRVButton);
        m_ContinueButton.onClick.AddListener(OnClickContinueButton);

        for (int i = 0; i < m_BrushesPrefabs.Count; i++)
            m_BrushesPrefabs[i].SetActive(false);
    }

    public void SetTitleColor(Color _Color)
    {
        m_BottomReviveImage.color = _Color;
        m_ReviveFillImage.color = _Color;
        for (int i = 0; i < m_BrushesPrefabs.Count; i++)
            m_BrushesPrefabs[i].GetComponent<BrushMenu>().SetNewColor(_Color);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        int favoriteBrush = m_GameManager.ComputeCurrentPlayeBrushIndex(true, 0);
        switch (_GamePhase)
        {
            case GamePhase.GAME:
            case GamePhase.END:
                m_BrushesPrefabs[favoriteBrush].SetActive(false);
                if (m_Visible)
                    Transition(false);
                break;

            case GamePhase.PRE_END:
                m_BrushesPrefabs[favoriteBrush].SetActive(true);
                m_Active = true;
                Transition(true);
                break;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_Active)
        {
            m_Timer += Time.deltaTime;
            m_ReviveFillImage.fillAmount = 1f - (m_Timer / m_TimerRevive);
            if (m_Timer >= m_TimerRevive)
                OnClickContinueButton();
        }

    }

    private void OnClickRVButton()
    {
        m_Active = false;
        m_GameManager.TryRevive();
    }

    private void OnClickContinueButton()
    {
        m_Active = false;
        m_GameManager.SkipRV();
    }
}
