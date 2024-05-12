using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerName", menuName = "Hoover.io/PlayerName", order = 1)]
public class PlayerNameData : ScriptableObject 
{
	public List<string>			m_Names;

	private List<int>			m_PickedIndexes;

	public void Init()
	{
		if (m_PickedIndexes == null)
			m_PickedIndexes = new List<int> ();
		else
			m_PickedIndexes.Clear ();
	}

	public string PickName()
	{
		int pickedIndex = 0;
		do
		{
			pickedIndex = Random.Range (0, m_Names.Count);
		} while (m_PickedIndexes.Contains (pickedIndex));

		m_PickedIndexes.Add (pickedIndex);

		string last = "";
		if (Random.Range (0, 3) == 0)
			last = Random.Range (1, 99).ToString ();

		return (m_Names [pickedIndex] + last);
	}
}
