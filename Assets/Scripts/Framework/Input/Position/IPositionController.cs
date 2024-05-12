using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionController
{
	void OnStartMove (Vector3 _Pos);
	void OnMove (Vector3 _Offset);
	void OnEndMove (Vector3 _Pos);
}
