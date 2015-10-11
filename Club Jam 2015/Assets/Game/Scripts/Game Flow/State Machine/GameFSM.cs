using System;
using UnityEngine;


/// <summary>
/// The controller for the different states the game is in.
/// </summary>
public class GameFSM : Singleton<GameFSM>
{
	/// <summary>
	/// The player whose turn it currently is.
	/// 0 for the first player, 1 for the second.
	/// </summary>
	public int CurrentPlayer = 0;
	public int OtherPlayer { get { return (CurrentPlayer + 1) % 2; } }


	/// <summary>
	/// Two players: the current one, and his opponent.
	/// </summary>
	public Player Current, Opponent;
	public bool IsAITurn { get { return Current.IsAI; } }


	public bool IsGameOver = false;


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


	protected override void Awake()
	{
		base.Awake();

		CurrentState = new ChooseElementsState();
	}
	void Start()
	{
		InputController.Instance.OnSweepSideways += t =>
			{
				if (CurrentState != null)
				{
					CurrentState.OnSweepSideways(t);
				}
			};
		InputController.Instance.OnSweepForward += t =>
			{
				if (CurrentState != null)
				{
					CurrentState.OnSweepForward(t);
				}
			};
		InputController.Instance.OnSweepVertical += t =>
			{
				if (CurrentState != null)
				{
					CurrentState.OnSweepVertical(t);
				}
			};
		InputController.Instance.OnCloseFist += t =>
			{
				if (CurrentState != null)
				{
					CurrentState.OnCloseFist(t);
				}
			};
	}
}