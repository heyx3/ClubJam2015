using System;
using UnityEngine;


public class ChooseToDiscardState : GameState
{
	public override void OnEnteringState(GameState previous)
	{
		base.OnEnteringState(previous);

		foreach (CardButton butt in PrefabAndInstanceContainer.Instance.HandButtons)
			butt.Tweenee = FSM.Current.Hand[butt.HandIndex].Owner.MyTr;

		foreach (Transform t in PrefabAndInstanceContainer.Instance.HumanHandMarkers)
			t.gameObject.SetActive(true);
		PrefabAndInstanceContainer.Instance.HumanPlayerDeckMarker.gameObject.SetActive(true);
	}
	public override void OnLeavingState(GameState next)
	{
		base.OnLeavingState(next);

		foreach (CardButton butt in PrefabAndInstanceContainer.Instance.HandButtons)
			butt.Tweenee = null;
		PrefabAndInstanceContainer.Instance.HumanPlayerDeckMarker.gameObject.SetActive(false);
	}
}