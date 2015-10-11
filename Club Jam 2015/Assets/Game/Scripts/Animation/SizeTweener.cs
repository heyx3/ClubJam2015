using System;
using UnityEngine;


public class SizeTweener : Tweener<Vector3, Transform>
{
	public bool GetMinOnAwake = true;
	private Vector3 realMax;


	public Vector3 Size
	{
		get { return Tweenee.localScale; }
		set { Tweenee.localScale = value; }
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
			Min = Size;
			Max = realMax + Min;
		}
	}
	protected override void Apply(Vector3 min, Vector3 max, float lerp)
	{
		if (Tweenee != null)
		{
			Size = Vector3.Lerp(min, max, lerp);
		}
	}
}