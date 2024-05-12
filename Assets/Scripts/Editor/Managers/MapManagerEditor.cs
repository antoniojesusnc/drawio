using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
	private MapManager 		m_Target;

	void OnEnable()
	{
		m_Target = (MapManager)target;
	}

	public override void OnInspectorGUI()
	{
		if (Application.isPlaying)
			EditorGUILayout.TextArea(m_Target.GetNames ());
	}
}
