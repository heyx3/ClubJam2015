using System;
using UnityEngine;


/// <summary>
/// The controller for the different states the game is in.
/// </summary>
public class GameFSM : Singleton<GameFSM>
{
	/// <summary>
	/// 0 for the first player, 1 for the second.
	/// </summary>
	public int CurrentPlayer = 0;


	public GameState CurrentState
	{
		get { return currentState; }
		set
		{
			GameState old = currentState;
			currentState = value;

			if (old != null)
			{
				old.OnLeavingState(value);
			}
			if (currentState != null)
			{
				currentState.OnEnteringState(old);
			}
		}
	}
	private GameState currentState = null;
}