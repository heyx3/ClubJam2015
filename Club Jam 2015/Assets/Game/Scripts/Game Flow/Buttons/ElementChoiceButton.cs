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

	private bool started = false;
	void Start()
	{
		started = true;
		OnEnable();
	}
	void OnEnable()
	{
		if (started)
		{
			InputController.Instance.OnCloseFist += GestureResponse;
			InputController.Instance.OnSweepVertical += GestureResponse;
		}
	}
	void OnDestroy() { OnDisable(); }
	void OnDisable()
	{
		InputController.Instance.OnCloseFist -= GestureResponse;
		InputController.Instance.OnSweepVertical -= GestureResponse;
	}

	protected override void Awake()
	{
		base.Awake();

		tr = transform;
		normalScale = tr.localScale;
	}
	protected override void Update()
	{
		base.Update();

		//Smoothly scale the button based on whether it's selected.
		bool isSelected = (IsSelected || touching.Count > 0);
		Vector3 targetScale = (isSelected ? (normalScale * SelectedScale) : normalScale);
		tr.localScale = Vector3.Lerp(tr.localScale, targetScale, ScaleSpeedLerp);
	}

	private void GestureResponse(KinematicsTracker palm)
	{
		if (touching.Contains(palm))
		{
			ChooseElementsState state = (ChooseElementsState)GameFSM.Instance.CurrentState;
			state.OnChosen(this, Choice);
		}
	}
}