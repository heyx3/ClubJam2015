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
	/// The first index is the player. The second is the element.
	/// The first element is the one currently out.
	/// </summary>
	public Elements[][] ElementsByPlayer = new Elements[2][] { new Elements[2], new Elements[2], };

	public Elements CurrentPlayerActiveElement { get { return ElementsByPlayer[CurrentPlayer][0]; } }
	public Elements CurrentPlayerPassiveElement { get { return ElementsByPlayer[CurrentPlayer][1]; } }
	public Elements OtherPlayerActiveElement { get { return ElementsByPlayer[OtherPlayer][0]; } }
	public Elements OtherPlayerPassiveElement { get { return ElementsByPlayer[OtherPlayer][1]; } }


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
}