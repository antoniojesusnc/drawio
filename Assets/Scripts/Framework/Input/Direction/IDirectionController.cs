using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectionController
{
    void OnStartMove();
    void OnMove(Vector3 _Offset);
    void OnEndMove();
}
