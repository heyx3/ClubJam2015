using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChooseCharacerActionState : GameState
{
	public override void OnEnteringState(GameState previous)
	{
		base.OnEnteringState(previous);

		PrefabAndInstanceContainer.Instance.ChooseCharacterActionContainer.SetActive(true);
	}
	public override void OnLeavingState(GameState next)
	{
		base.OnLeavingState(next);
		
		PrefabAndInstanceContainer.Instance.ChooseCharacterActionContainer.SetActive(false);
	}
}