using System;
using UnityEngine;


public class ColorTweener : Tweener<Color, Renderer>
{
	void Start()
	{
		if (Tweenee == null)
			Tweenee = GetComponentInChildren<Renderer>();
	}
	protected override void Apply(Color min, Color max, float lerp)
	{
		Color val = Color.LerpUnclamped(min, max, lerp);
		if (Tweenee != null)
		{
			Tweenee.material.color = val;
		}
	}
}