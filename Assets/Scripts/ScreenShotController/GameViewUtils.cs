#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
 
public static class GameViewUtils
{
    static object gameViewSizesInstance;
    static MethodInfo getGroup;
 
	static GameViewUtils()
	{
		var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
		var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
		var instanceProp = singleType.GetProperty("instance");
		getGroup = sizesType.GetMethod("GetGroup");
		gameViewSizesInstance = instanceProp.GetValue(null, null);
	}

	public static void SetSize(int index)
	{
		var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
		var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		var gvWnd = EditorWindow.GetWindow(gvWndType);
		selectedSizeIndexProp.SetValue(gvWnd, index, null);
	}

	public static void SetSize(string name)
	{
		SetSize(FindSize(name));
	}

	public static void SetSize(Vector2 resolution)
	{
		SetSize(FindSize((int)resolution.x, (int)resolution.y));
	}

	public static bool SizeExists(string text)
	{
		return FindSize(text) != -1;
	}

	public static int FindSize(string text)
	{
		var group = GetGroup(GetCurrentGroupType());
		var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
		var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
		for(int i = 0; i < displayTexts.Length; i++)
		{
			string display = displayTexts[i];
			int pren = display.IndexOf('(');
			if (pren != -1)
				display = display.Substring(0, pren-1);
			if (display == text)
				return i;
		}
		return -1;
	}

	public static int FindSize(int width, int height)
    {
		var group = GetGroup(GetCurrentGroupType());
		var groupType = group.GetType();
		var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
		var getCustomCount = groupType.GetMethod("GetCustomCount");
		int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
		var getGameViewSize = groupType.GetMethod("GetGameViewSize");
		var gvsType = getGameViewSize.ReturnType;
		var widthProp = gvsType.GetProperty("width");
		var heightProp = gvsType.GetProperty("height");
		var indexValue = new object[1];
		for(int i = 0; i < sizesCount; i++)
		{
			indexValue[0] = i;
			var size = getGameViewSize.Invoke(group, indexValue);
			int sizeWidth = (int)widthProp.GetValue(size, null);
			int sizeHeight = (int)heightProp.GetValue(size, null);
			if (sizeWidth == width && sizeHeight == height)
				return i;
		}
		return -1;
    }

	static object GetGroup(GameViewSizeGroupType type)
	{
		return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
	}

	public static GameViewSizeGroupType GetCurrentGroupType()
	{
		var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
		return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);
	}

	public static Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView, UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}
}
#endif