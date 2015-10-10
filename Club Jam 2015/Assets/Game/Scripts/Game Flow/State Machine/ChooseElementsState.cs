using System;
using System.Collections.Generic;
using UnityEngine;


public class ChooseElementsState : GameState
{


	public override void OnEnteringState(GameState previous)
	{
		base.OnEnteringState(previous);


	}
	public override void OnLeavingState(GameState next)
	{
		base.OnLeavingState(next);


	}


	public ChooseElementsState(GameFSM owner) : base(owner) { }
}