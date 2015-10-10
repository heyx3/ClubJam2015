using System;
using UnityEngine;


/// <summary>
/// A button for selecting a specific element deck to use in the match.
/// </summary>
public class ElementChoiceButton : Button
{
	public float TriggerTime = 1.0f;
	public Elements Choice = Elements.Air;

	public float SelectedScale = 1.3f;
	public float ScaleSpeedLerp = 0.1f;

	[NonSerialized]
	public float TouchTime = 0.0f;

	public bool IsSelected = false;
	
	private Vector3 normalScale;
	private Transform tr;


	protected override void Awake()
	{
		base.Awake();

		tr = transform;
		normalScale = tr.localScale;
	}
	protected override void Update()
	{
		base.Update();

		bool isSelected = IsSelected;

		//See if this button is being activated.
		if (touching.Count > 0)
		{
			TouchTime += Time.deltaTime;

			if (TouchTime >= TriggerTime)
			{
				ChooseElementsState state = (ChooseElementsState)GameFSM.Instance.CurrentState;
				state.OnChosen(this, Choice);
			}
			else
			{
				isSelected = true;
			}
		}

		//Smoothly the button based on whether it's selected.
		Vector3 targetScale = (isSelected ? (normalScale * SelectedScale) : normalScale);
		tr.localScale = Vector3.Lerp(tr.localScale, targetScale, ScaleSpeedLerp);
	}
}