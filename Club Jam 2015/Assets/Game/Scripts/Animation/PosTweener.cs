using System;
using UnityEngine;


public class PosTweener : Tweener<Vector3, Transform>
{
	public bool GetMinOnAwake = true;
	public bool UseLocalPos = false;

	private Vector3 realMax;


	public Vector3 Pos
	{
		get
		{
			if (UseLocalPos)
				return Tweenee.localPosition;
			else return Tweenee.position;
		}
		set
		{
			if (UseLocalPos)
				Tweenee.localPosition = value;
			else Tweenee.position = value;
		}
	}


	void Awake()
	{
		realMax = Max;
	}
	public void Start()
	{
		if (Tweenee == null)
		{
			Tweenee = transform;
		}
		if (GetMinOnAwake)
		{
			Min = Pos;
			Max = realMax + Min;
		}
	}
	protected override void Apply(Vector3 min, Vector3 max, float lerp)
	{
		if (Tweenee != null)
		{
			Pos = Vector3.Lerp(min, max, lerp);
		}
	}
}