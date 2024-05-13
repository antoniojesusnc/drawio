using DG.Tweening;
using UnityEngine;

public abstract class GenericDotweenConfig<T> : ScriptableObject 
{
    public abstract Tween PlayEffect(T component);
}

public abstract class GenericDotweenConfig<T1, T2> : ScriptableObject 
{
    public abstract Tween PlayEffect(T1 component, T2 component2);
}

public abstract class GenericDotweenConfig<T1, T2, T3> : ScriptableObject 
{
    public abstract Tween PlayEffect(T1 component, T2 component2, T3 component3);
}