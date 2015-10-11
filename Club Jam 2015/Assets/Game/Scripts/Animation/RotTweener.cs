using System;
using UnityEngine;


public class RotTweener : Tweener<Quaternion, Transform>
{
	public Vector3 MaxFromEulers = Vector3.zero;
	public bool MaxIsRelative = true;

	public bool UseRelativeRot = false;


	public Quaternion Rot
	{
		get { return (UseRelativeRot ? Tweenee.localRotation : Tweenee.rotation); }
		set
		{
			if (UseRelativeRot)
				Tweenee.localRotation = value;
			else Tweenee.rotation = value;
		}
	}


	public void Start()
	{
		if (Tweenee == null)
		{
			Tweenee = transform;
		}

		Min = Rot;
		Max = Quaternion.Euler(MaxFromEulers);
		if (MaxIsRelative)
		{
			Max = Min * Max;
		}
	}

	protected override void Apply(Quaternion min, Quaternion max, float lerp)
	{
		if (Tweenee != null)
		{
			Quaternion rot = Quaternion.Lerp(min, max, lerp);
			Rot = rot;
		}
	}
}