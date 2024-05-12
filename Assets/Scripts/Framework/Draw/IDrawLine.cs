using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrawLine
{
    void AddPoint(Vector3 _Pos, bool _IsSubBrush = false);
}