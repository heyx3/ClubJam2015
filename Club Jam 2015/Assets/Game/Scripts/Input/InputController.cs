using UnityEngine;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Handles Leap Motion input parsing.
/// </summary>
public class InputController : Singleton<InputController>
{
	public KinematicsTracker.DebugData Debugging = new KinematicsTracker.DebugData();

	public KinematicsTracker[] Trackers = new KinematicsTracker[2] { null, null };
	public HandController HandControls;

	public float GestureDuration = 0.1f;
	public float VelThreshold = 0.9f;

	public float CalmTime = 0.6f;


	private float[] trackerCalmTime = new float[2] { 0.0f, 0.0f };



	protected override void Awake()
	{
		base.Awake();

		if (HandControls == null)
		{
			Debug.LogError("'HandControls' in 'InputController' component is null!");
		}

		HandControls.onCreateHand += (handModel =>
			{
				if (handModel.gameObject.GetComponent<RigidHand>() != null)
				{
					return;
				}

				KinematicsTracker tracker = handModel.palm.gameObject.AddComponent<KinematicsTracker>();
				if (Trackers[0] == null)
				{
					Trackers[0] = tracker;
					trackerCalmTime[0] = CalmTime;
				}
				else
				{
					if (Trackers[1] != null)
					{
						Debug.LogError("More than two hands!");
					}
					else
					{
						Trackers[1] = tracker;
						trackerCalmTime[1] = CalmTime;
					}
				}
			});
	}

	void Update()
	{
		for (int i = 0; i < Trackers.Length; ++i)
		{
			KinematicsTracker tracker = Trackers[i];
			if (tracker != null)
			{
				tracker.Debugging = Debugging;

				if (trackerCalmTime[i] <= 0.0f)
				{
					Vector3 avgVel = tracker.GetAverageVelocity(GestureDuration);
					if (Mathf.Abs(avgVel.x) >= VelThreshold)
					{
						SweepSideGesture(tracker, avgVel.x);
						trackerCalmTime[i] = CalmTime;
					}
					else if (Mathf.Abs(avgVel.y) >= VelThreshold)
					{
						SweepVerticalGesture(tracker, avgVel.y);
						trackerCalmTime[i] = CalmTime;
					}
					else if (Mathf.Abs(avgVel.z) >= VelThreshold)
					{
						SweepForwardGesture(tracker, avgVel.z);
						trackerCalmTime[i] = CalmTime;
					}
				}
				else
				{
					trackerCalmTime[i] -= Time.deltaTime;
				}
			}
		}
	}
	private void SweepSideGesture(KinematicsTracker tracker, float vel)
	{
		GameState state = GameFSM.Instance.CurrentState;
		if (state != null)
			state.OnSweepSideways(tracker, vel, GestureDuration);
	}
	private void SweepVerticalGesture(KinematicsTracker tracker, float vel)
	{
		GameState state = GameFSM.Instance.CurrentState;
		if (state != null)
			state.OnSweepVertical(tracker, vel, GestureDuration);
	}
	private void SweepForwardGesture(KinematicsTracker tracker, float vel)
	{
		GameState state = GameFSM.Instance.CurrentState;
		if (state != null)
			state.OnSweepForward(tracker, vel, GestureDuration);
	}
}