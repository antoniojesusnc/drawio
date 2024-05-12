using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions {
    
	public static void Shuffle<T>(this List<T> list) {
		int swapIndex;
		for (int i = 0; i < list.Count; ++i)
        {
			swapIndex = Random.Range(0, list.Count);
			T temp = list[i];
			list[i] = list[swapIndex];
			list[swapIndex] = temp;
        }
	}
}
