using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// When a Leap Motion hand gets close enough to this GameObject, it triggers.
/// </summary>
public abstract class Button : MonoBehaviour
{
	public Transform MyTransform { get; private set; }

	
	public float HitRadius = 0.25f;

	private List<KinematicsTracker> touching = new List<KinematicsTracker>();


	protected virtual void Awake()
	{
		MyTransform = transform;
	}
	protected virtual void Update()
	{
		foreach (KinematicsTracker tracker in InputController.Instance.Trackers)
		{
			if (tracker != null)
			{
				Vector3 pos = tracker.PositionLogs[tracker.GetLogIndex(0)];
				
				bool isTouching = ((pos - MyTransform.position).sqrMagnitude) < (HitRadius * HitRadius);
				if (isTouching)
				{
					if (touching.Contains(tracker))
					{
						OnDuringTrigger(tracker);
					}
					else
					{
						touching.Add(tracker);
						OnEnterTrigger(tracker);
					}
				}
				else
				{
					if (touching.Contains(tracker))
					{
						touching.Remove(tracker);
						OnExitTrigger(tracker);
					}
				}
			}
		}
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, HitRadius);
	}

	protected virtual void OnEnterTrigger(KinematicsTracker tracker) { }
	protected virtual void OnDuringTrigger(KinematicsTracker tracker) { }
	protected virtual void OnExitTrigger(KinematicsTracker tracker) { }
}