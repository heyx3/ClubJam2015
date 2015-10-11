using UnityEngine;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Handles Leap Motion input parsing.
/// </summary>
public class InputController : Singleton<InputController>
{
	private static Vector2 Horz(Vector3 v) { return new Vector2(v.x, v.z); }


	public bool DisableAllGestures = false;

	public KinematicsTracker.DebugData Debugging = new KinematicsTracker.DebugData();

	public KinematicsTracker[] PalmTrackers = new KinematicsTracker[2] { null, null };
	public KinematicsTracker[] FingerTrackers = new KinematicsTracker[2] { null, null };
	public HandController HandControls;
	public GameObject LaserPointerHandPrefab;

	public float GestureDuration = 0.1f;
	public float VelThreshold = 0.9f;

	public float CalmTime = 0.6f;

	public float FistCloseDistThreshold = 0.07f;
	public float CloseFistTime = 0.65f;


	public delegate void GestureReaction(KinematicsTracker palmTracker);
	public event GestureReaction OnSweepSideways, OnSweepVertical, OnSweepForward, OnCloseFist;


	private float[] fistCloseTime = new float[2] { 0.0f, 0.0f };
	private float[] trackerCalmTime = new float[2] { 0.0f, 0.0f };


	protected override void Awake()
	{
		base.Awake();

		if (HandControls == null)
		{
			Debug.LogError("'HandControls' in 'InputController' component is null!");
		}
		if (LaserPointerHandPrefab == null)
		{
			Debug.LogError("'LaserPointerHandPrefab' is null!");
		}

		HandControls.onCreateHand += (handModel =>
			{
				if (handModel.gameObject.GetComponent<RigidHand>() != null)
				{
					return;
				}

				Instantiate<GameObject>(LaserPointerHandPrefab).transform.parent = handModel.palm;

				KinematicsTracker palmTrack = handModel.palm.gameObject.AddComponent<KinematicsTracker>(),
								  fingerTrack = handModel.fingers[2].bones[FingerModel.NUM_BONES - 1].gameObject.AddComponent<KinematicsTracker>();
				if (PalmTrackers[0] == null)
				{
					PalmTrackers[0] = palmTrack;
					FingerTrackers[0] = fingerTrack;
					trackerCalmTime[0] = CalmTime;
					fistCloseTime[0] = 0.0f;
				}
				else
				{
					if (PalmTrackers[1] != null)
					{
						Debug.LogError("More than two hands!");
					}
					else
					{
						PalmTrackers[1] = palmTrack;
						FingerTrackers[1] = fingerTrack;
						trackerCalmTime[1] = CalmTime;
						fistCloseTime[1] = 0.0f;
					}
				}
			});
	}

	void Update()
	{
		if (DisableAllGestures)
			return;

		for (int i = 0; i < PalmTrackers.Length; ++i)
		{
			KinematicsTracker palmT = PalmTrackers[i],
							  fingerT = FingerTrackers[i];
			if (palmT != null)
			{
				palmT.Debugging = Debugging;

				if (trackerCalmTime[i] <= 0.0f)
				{
					Vector3 avgVel = palmT.GetAverageVelocity(GestureDuration);
					Vector3 palmPos = palmT.PositionLogs[palmT.GetLogIndex(0)],
							fingerPos = fingerT.PositionLogs[fingerT.GetLogIndex(0)];

					if (Mathf.Abs(avgVel.x) >= VelThreshold)
					{
						SweepSideGesture(palmT);
						trackerCalmTime[i] = CalmTime;
					}
					else if (Mathf.Abs(avgVel.y) >= VelThreshold)
					{
						SweepVerticalGesture(palmT);
						trackerCalmTime[i] = CalmTime;
					}
					else if (Mathf.Abs(avgVel.z) >= VelThreshold)
					{
						SweepForwardGesture(palmT);
						trackerCalmTime[i] = CalmTime;
					}
					else if ((Horz(palmPos) - Horz(fingerPos)).sqrMagnitude <= (FistCloseDistThreshold * FistCloseDistThreshold))
					{
						fistCloseTime[i] += Time.deltaTime;
						if (fistCloseTime[i] >= CloseFistTime)
						{
							fistCloseTime[i] = 0.0f;
							trackerCalmTime[i] = CalmTime;
							FistCloseGesture(palmT);
						}
					}
				}
				else
				{
					trackerCalmTime[i] -= Time.deltaTime;
				}
			}
		}
	}
	private void SweepSideGesture(KinematicsTracker tracker)
	{
		if (OnSweepSideways != null)
			OnSweepSideways(tracker);
	}
	private void SweepVerticalGesture(KinematicsTracker tracker)
	{
		if (OnSweepVertical != null)
			OnSweepVertical(tracker);
	}
	private void SweepForwardGesture(KinematicsTracker tracker)
	{
		if (OnSweepForward != null)
			OnSweepForward(tracker);
	}
	private void FistCloseGesture(KinematicsTracker tracker)
	{
		if (OnCloseFist != null)
			OnCloseFist(tracker);
	}
}