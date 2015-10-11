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
	/// </summary>
	public virtual void OnSweepSideways(KinematicsTracker palmMovement) { }
	/// <summary>
	/// Reacts to a vertical sweeping gesture.
	/// </summary>
	public virtual void OnSweepVertical(KinematicsTracker palmMovement) { }
	/// <summary>
	/// Reacts to a forward/back sweeping gesture.
	/// </summary>
	public virtual void OnSweepForward(KinematicsTracker palmMovement) { }
	/// <summary>
	/// Reacts to a closing fist gesture.
	/// </summary>
	public virtual void OnCloseFist(KinematicsTracker palmMovement) { }
}