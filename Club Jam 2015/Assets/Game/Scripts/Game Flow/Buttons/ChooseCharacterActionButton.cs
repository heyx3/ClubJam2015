using System;
using UnityEngine;


public class ChooseCharacterActionButton : Button
{
	public enum Choices
	{
		ChangeCharacters,
		GainEnergy,
	}


	public Choices ThisChoice;


	private SizeTweener sizeTw;


	void Start()
	{
		InputController.Instance.OnCloseFist += OnGesture;
		InputController.Instance.OnSweepVertical += OnGesture;
		sizeTw = GetComponent<SizeTweener>();
	}
	protected override void OnEnterTrigger(KinematicsTracker tracker)
	{
		base.OnEnterTrigger(tracker);
		sizeTw.Target = 1.0f;
	}
	protected override void OnExitTrigger(KinematicsTracker tracker)
	{
		base.OnExitTrigger(tracker);
		if (touching.Count == 0)
			sizeTw.Target = 0.0f;
	}

	private void OnGesture(KinematicsTracker palm)
	{
		Player current = GameFSM.Instance.Current;
		if (touching.Contains(palm))
		{
			switch (ThisChoice)
			{
				case Choices.ChangeCharacters:
					//TODO: Handle.
					break;

				case Choices.GainEnergy:
					current.Energy[current.SecondChar] +=
						GameConsts.Instance.Characters[current.SecondChar].PassiveEnergyPerTurn;
					break;

				default: throw new NotImplementedException("Unknown choice " + ThisChoice);
			}

			//TODO: Change state.
			GameFSM.Instance.CurrentState = null;
		}
	}
}