using System;
using System.Collections.Generic;
using UnityEngine;


public class ChooseElementsState : GameState
{
	private List<Elements> chosenElements = new List<Elements>();


	public override void OnEnteringState(GameState previous)
	{
		base.OnEnteringState(previous);

		chosenElements.Clear();
		PrefabAndInstanceContainer.Instance.ElementSelectionContainer.SetActive(true);
	}
	public override void OnLeavingState(GameState next)
	{
		base.OnLeavingState(next);
		
		PrefabAndInstanceContainer.Instance.ElementSelectionContainer.SetActive(false);
	}


	public void OnChosen(ElementChoiceButton button, Elements element)
	{
		if (button.IsSelected)
		{
			return;
		}


		chosenElements.Add(element);
		button.IsSelected = true;

		if (chosenElements.Count == 2)
		{
			FSM.ElementsByPlayer[0] = chosenElements.ToArray();
			FSM.ElementsByPlayer[1] = AI.ChooseElements(FSM.ElementsByPlayer[0]);

			//TODO: Jump to next state.
			FSM.CurrentState = null;
		}
		else if (chosenElements.Count > 2)
		{
			Debug.LogError("More than two elements somehow! " + chosenElements.Count);
		}
	}
}