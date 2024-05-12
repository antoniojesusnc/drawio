using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemExtensions
{
	public static void SetColor(this ParticleSystem _ParticleSystem, Color _Color)
    {

		var module = _ParticleSystem.main;
		module.startColor = _Color;

    }
}