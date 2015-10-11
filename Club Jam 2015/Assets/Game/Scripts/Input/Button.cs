using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// When a Leap Motion hand gets close enough to this GameObject, it triggers.
/// Provides both a hit sphere and a hit box.
/// </summary>
public abstract class Button : MonoBehaviour
{
	public Transform MyTransform { get; private set; }

	
	public float HitRadius = 0.25f;
	public Vector3 HitBoxSize = new Vector3(0.1f, 0.1f, 0.1f);

	protected List<KinematicsTracker> touching = new List<KinematicsTracker>();


	protected virtual void Awake()
	{
		MyTransform = transform;
	}
	protected virtual void Update()
	{
		foreach (KinematicsTracker tracker in InputController.Instance.PalmTrackers)
		{
			if (tracker != null)
			{
				Vector3 pos = tracker.PositionLogs[tracker.GetLogIndex(0)];
				if (IsTouching(tracker.PositionLogs[tracker.GetLogIndex(0)]))
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
	private bool IsTouching(Vector3 pos)
	{
		Vector3 myPos = MyTransform.position;
		Vector3 minBox = myPos - (HitBoxSize * 0.5f),
				maxBox = minBox + HitBoxSize;

		return ((pos - myPos).sqrMagnitude < (HitRadius * HitRadius)) ||
			   (pos.x >= minBox.x && pos.y >= minBox.y && pos.z >= minBox.z &&
			    pos.x <= maxBox.x && pos.y <= maxBox.y && pos.z <= maxBox.z);
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, HitRadius);
		Gizmos.DrawCube(transform.position, HitBoxSize);
	}

	protected virtual void OnEnterTrigger(KinematicsTracker tracker) { }
	protected virtual void OnDuringTrigger(KinematicsTracker tracker) { }
	protected virtual void OnExitTrigger(KinematicsTracker tracker) { }
}