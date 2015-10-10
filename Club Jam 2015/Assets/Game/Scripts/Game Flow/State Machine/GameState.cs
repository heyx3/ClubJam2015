using System;
using UnityEngine;


public abstract class GameState
{
	protected GameFSM FSM { get { return GameFSM.Instance; } }

	protected int CurrentPlayer { get { return FSM.CurrentPlayer; } set { FSM.CurrentPlayer = value; } }


	public virtual void Update() { }
	public virtual void OnEnteringState(GameState previous) { }
	public virtual void OnLeavingState(GameState next) { }
	
	/// <summary>
	/// Reacts to a sideways sweeping gesture.
	/// Negative velocity is "left"; positive is "right".
	/// Current position can be queried from the KinematicsTracker.
	/// </summary>
	/// <param name="duration">
	/// The gesture that caused this event started this many seconds ago.
	/// Will usually be a small fraction of a second.
	/// </param>
	public virtual void OnSweepSideways(KinematicsTracker moveHistory, float gestureVelocity, float duration) { }
	/// <summary>
	/// Reacts to a vertical sweeping gesture.
	/// Negative velocity is "down"; positive is "up".
	/// Current position can be queried from the KinematicsTracker.
	/// </summary>
	/// <param name="duration">
	/// The gesture that caused this event started this many seconds ago.
	/// Will usually be a small fraction of a second.
	/// </param>
	public virtual void OnSweepVertical(KinematicsTracker moveHistory, float gestureVelocity, float duration) { }
	/// <summary>
	/// Reacts to a forward/back sweeping gesture.
	/// Negative velocity is "backwards"; positive is "forwards".
	/// Current position can be queried from the KinematicsTracker.
	/// </summary>
	/// <param name="duration">
	/// The gesture that caused this event started this many seconds ago.
	/// Will usually be a small fraction of a second.
	/// </param>
	public virtual void OnSweepForward(KinematicsTracker moveHistory, float gestureVelocity, float duration) { }
}