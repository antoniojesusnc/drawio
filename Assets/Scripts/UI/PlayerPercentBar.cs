using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPercentBar : MonoBehaviour {
    
	public Text m_PercentText;
	public Image m_FillBar;
	public RectTransform m_Bars;
    public Text m_PointsEarn;
    public Image m_IconInGame;

    private Tween m_Tween;
    private Tween m_Tween2;

	Player _lastPlayer;

    private void Start()
    {
        if (m_PointsEarn != null)
            m_PointsEarn.text = "";
		
        if (m_IconInGame != null)
            m_IconInGame.gameObject.SetActive(false);
    }

    public void UpdateBar(int _Rank, Player _Player, bool _Last, float _Percent)
	{
		gameObject.SetActive(true);
		
        m_PercentText.text = (_Player.Score * 100f).ToString("0") + "%";
        float percent = Mathf.Clamp(_Percent, 0.5f, 1f);
        m_Bars.anchorMax = new Vector2(1f, 0.5f + percent * 3);
        if (_lastPlayer != _Player)
            Switch(_Rank, _Player, _Last, _Percent);
		_lastPlayer = _Player;

        if (m_IconInGame != null)
            m_IconInGame.gameObject.SetActive(_Last);

    }

    private void Switch(int _Rank, Player _Player, bool _First, float _Percent)
    {
        m_Tween.Kill();
        m_Tween2.Kill();

        RectTransform rect = transform.GetChild(0).GetComponent<RectTransform>();
        m_Tween = rect.DOLocalMoveY(500, 0.25f).OnComplete(() => {


			m_FillBar.color = _Player.m_Color;

            m_Bars.anchoredPosition = Vector3.zero;

            m_Tween2 = rect.DOLocalMoveY(0, 0.25f);
        });
    }

    public void Hide()
	{
		gameObject.SetActive(false);
	}
}
