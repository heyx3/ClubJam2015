using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MoveTowards : MonoBehaviour
{
	public float Speed = 10.0f;


	public Vector3 StartPos { get; private set; }
	public Vector3 Target { get; private set; }
	public float PathDistance { get; private set; }
	
	public bool IsSeeking { get; private set; }
	public float SeekingLerp { get; private set; }


	public delegate void FinishedMovementDelegate(MoveTowards mt);

	/// <summary>
	/// This event clears itself out every time it's triggered.
	/// Doesn't trigger (but does clear itself) when "SetTarget()" is called.
	/// </summary>
	public event FinishedMovementDelegate OnMovementFinished;


	public Transform MyTransform { get; private set; }


	void Awake()
	{
		MyTransform = transform;
		IsSeeking = false;
	}
	void Update()
	{
		if (IsSeeking)
		{
			SeekingLerp += Time.deltaTime * (Speed / PathDistance);
			MyTransform.position = Vector3.Lerp(StartPos, Target, SeekingLerp);

			if (SeekingLerp >= 1.0f)
			{
				MyTransform.position = Target;

				FinishedMovementDelegate evnt = OnMovementFinished;
				OnMovementFinished = null;
				if (evnt != null)
					evnt(this);
			}
		}
	}

	public void SetTarget(Vector3 newTarget)
	{
		OnMovementFinished = null;

		Target = newTarget;
		IsSeeking = true;

		SeekingLerp = 0.0f;

		StartPos = MyTransform.position;

		PathDistance = (StartPos - Target).magnitude;
	}
}