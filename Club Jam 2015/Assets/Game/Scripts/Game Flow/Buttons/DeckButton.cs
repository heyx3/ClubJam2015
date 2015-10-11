using System;
using UnityEngine;


public class DeckButton : Button
{
	void Start()
	{
		InputController.Instance.OnCloseFist += OnGestured;
		InputController.Instance.OnSweepVertical += OnGestured;
	}

	private void OnGestured(KinematicsTracker tracker)
	{
		if (gameObject.activeSelf && touching.Contains(tracker) &&
			GameFSM.Instance.CurrentState is ChooseToDiscardState)
		{
			GameFSM.Instance.StartCoroutine(DrawCardsCoroutine(GameFSM.Instance.Current));
		}
	}

	private System.Collections.IEnumerator DrawCardsCoroutine(Player p)
	{
		yield return GameFSM.Instance.StartCoroutine(GameActions.DrawCardsToFull(p));

		GameFSM.Instance.CurrentState = new ChooseCharacerActionState();
	}
}