using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwipeController
{
	void OnStartMove (float _Pos);
	void OnMove (float _Offset);
	void OnEndMove (float _Pos);
}
