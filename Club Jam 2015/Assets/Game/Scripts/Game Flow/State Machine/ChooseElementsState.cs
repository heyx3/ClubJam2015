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

		PrefabAndInstanceContainer.Instance.PlayerLookTarget = PrefabAndInstanceContainer.Instance.LookTarget_MyTable;

		foreach (Transform t in PrefabAndInstanceContainer.Instance.HumanHandMarkers)
			t.gameObject.SetActive(false);
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
		PrefabAndInstanceContainer.InstantiateAt(PrefabAndInstanceContainer.Instance.ParticlePrefab_ChooseElemental,
												 button.MyTransform.position);

		if (chosenElements.Count == 2)
		{
			FSM.Current = new Player(chosenElements[0], chosenElements[1], false);
			Elements[] aiEls = AI.ChooseElements(chosenElements);
			FSM.Opponent = new Player(aiEls[0], aiEls[1], true);

			FSM.StartCoroutine(DealOutCoroutine());
		}
		else if (chosenElements.Count > 2)
		{
			Debug.LogError("More than two elements somehow! " + chosenElements.Count);
		}
	}
	private static System.Collections.IEnumerator DealOutCoroutine()
	{
		yield return new WaitForSeconds(1.0f);

		FSM.CurrentState = null;

		yield return FSM.StartCoroutine(GameActions.DrawCardsToFull(FSM.Current));
		yield return FSM.StartCoroutine(GameActions.DrawCardsToFull(FSM.Opponent));

		FSM.CurrentState = new ChooseToDiscardState();
	}
}