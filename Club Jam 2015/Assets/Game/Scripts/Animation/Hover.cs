using System;
using UnityEngine;


public class Hover : MonoBehaviour
{
	public float Amplitude = 0.5f;
	public float Speed = 1.0f;
	public Vector3 Direction = new Vector3(0.0f, 1.0f, 0.0f);
	public float MovementSmoothness = 1.0f;

	[NonSerialized]
	public Vector3 BasePos;


	private Transform tr;


	void Awake()
	{
		tr = transform;
		BasePos = tr.position;
	}
	void Update()
	{
		float lerp = Amplitude * Mathf.Sin(Time.timeSinceLevelLoad * Speed);
		lerp = Mathf.Sign(lerp) * Mathf.Pow(Mathf.Abs(lerp), MovementSmoothness);

		//tr.position = Vector3.LerpUnclamped(BasePos, BasePos + Direction, lerp);
	}
}