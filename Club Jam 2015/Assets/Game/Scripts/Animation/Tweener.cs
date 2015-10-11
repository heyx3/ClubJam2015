using System;
using UnityEngine;


public abstract class Tweener<T, ToTween> : MonoBehaviour where ToTween : class
{
	/// <summary>
	/// If set to null, the child class inheriting from this should set it to some sane default value.
	/// </summary>
	public ToTween Tweenee = null;

	public T Min, Max;
	public float Target = 0.0f;

	public float Speed = 1.0f;
	public float SpeedExponent = 1.0f;

	public float Lerp = 0.0f;

	public bool Smooth = false;


	protected virtual void Update()
	{
		float toTarget = Target - Lerp;
		float dir = toTarget;
		if (!Smooth)
		{
			dir = Mathf.Sign(toTarget);
		}

		float delta = dir * Speed * Time.deltaTime;
		if (Mathf.Abs(Lerp + delta - Target) > Mathf.Abs(toTarget))
		{
			Lerp = Target;
		}
		else
		{
			Lerp += delta;
		}

		Apply(Min, Max, Mathf.Pow(Lerp, SpeedExponent));
	}

	/// <summary>
	/// Interpolates between the values then applies it in whatever way it's supposed to be applied.
	/// </summary>
	protected abstract void Apply(T min, T max, float lerp);
}