using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathView : View<DeathView> {

	public Text m_DeadNameText;

	protected override void Awake()
	{
        base.Awake();
		BattleRoyaleManager.Instance.onElimination += OnElimination;
	}

	void OnElimination(Player _EliminatedPlayer)
	{
		if (m_GameManager.currentPhase != GamePhase.GAME)
			return;
		
		m_DeadNameText.text = _EliminatedPlayer.Name;
		if (_EliminatedPlayer is IAPlayer)
		    StartCoroutine(ShowAndHide());
	}

    IEnumerator ShowAndHide()
	{
		Transition(true);
		yield return new WaitForSeconds(1f);
		Transition(false);
	}
}
