using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingView : View<LoadingView> {

    public float        m_DelayBetweenPlayersConnection;
    public GameObject   m_PrefabPeople;

    public Sprite       m_PeopleSprite;

    public Text        	m_PrepareText;
	public Text 		m_WaitingText;

    public Text         m_WaitingLabel;
    public Transform    m_GridParent;
    private List<PeopleLoadingSlot> m_Slots;

    [Header("WAITING/READY")]
    public CanvasGroup  m_WaitingObj;
    public CanvasGroup  m_ReadyObj;
	public Image        m_GoImage;

    [Header("TIPS")]
    public Text m_TipObj;
    public List<string> m_TipsText;

    private bool m_LoadingFinish = false;

    protected override void Awake()
    {
        base.Awake();
        m_Slots = new List<PeopleLoadingSlot>();
        m_WaitingObj.alpha = 1;
        m_ReadyObj.alpha = 0;

        m_TipObj.text = m_TipsText[Random.Range(0, m_TipsText.Count)];

        m_LoadingFinish = false;
		for (int i = 0; i < Constants.s_PlayerCount; i++)
        {
            GameObject obj = Instantiate(m_PrefabPeople, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(m_GridParent);
            obj.transform.localScale = Vector3.one;
            m_Slots.Add(obj.GetComponent<PeopleLoadingSlot>());
        }
    }

    public void SetTitleColor(Color _Color)
    {
		m_PrepareText.color = _Color;
		m_WaitingText.color = _Color;
		m_GoImage.color = _Color;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && m_GameManager.currentPhase == GamePhase.LOADING && m_LoadingFinish)
            m_GameManager.ChangePhase(GamePhase.GAME);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.LOADING:
                Clear();
                Transition(true);
                StartLoading();
                break;

            case GamePhase.GAME:
                if (m_Visible)
                    Transition(false);
                break;
        }
    }

    public void StartLoading()
    {
        StartCoroutine(LoadingPeopleCoroutine());
    }

    void Clear()
    {
        m_LoadingFinish = false;
        for (int i = 0; i < m_Slots.Count; ++i)
        {
            m_Slots[i].Unload();
        }
    }

    private void ActivatePeopleUI(int _ID, float _Timer = 0.5f)
    {
        m_Slots[_ID].LoadPlayer();
    }

    private IEnumerator LoadingPeopleCoroutine()
    {
		int playersLeftID = Random.Range(1, (Constants.s_PlayerCount / 2) + 1);
		m_WaitingLabel.text = "WAITING " + (Constants.s_PlayerCount - playersLeftID).ToString() + " PLAYERS"; 

        for (int i = 0; i < playersLeftID; i++)
        {
            ActivatePeopleUI(i, 0);
        }

        m_WaitingObj.alpha = 1;
        m_ReadyObj.alpha = 0;

        yield return new WaitForSeconds(1);

		while (playersLeftID < Constants.s_PlayerCount)
        {
            ActivatePeopleUI(playersLeftID++);

			m_WaitingLabel.text = "WAITING " + (Constants.s_PlayerCount - playersLeftID).ToString() + " PLAYERS";
            yield return new WaitForSeconds(m_DelayBetweenPlayersConnection + Random.Range(-m_DelayBetweenPlayersConnection, m_DelayBetweenPlayersConnection / 2));
        }
        m_WaitingObj.DOFade(0, 0.1f).OnComplete(() =>
        {
            m_ReadyObj.DOFade(1, 0.1f);
            m_LoadingFinish = true;
        });

        yield return new WaitForSeconds(1.1f);


    }

}
