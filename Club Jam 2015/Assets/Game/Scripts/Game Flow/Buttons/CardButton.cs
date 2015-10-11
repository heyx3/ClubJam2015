using System;
using UnityEngine;


public class CardButton : Button
{
	private float TweenTarget
	{
		get { return SizeTween.Target; }
		set
		{
			SizeTween.Target = value;
			PosTween.Target = value;
			RotTween.Target = value;
		}
	}

	private CardComponent Cmp { get { return GameFSM.Instance.Current.Hand[HandIndex].Owner; } }


	public int HandIndex;
	public bool WasDiscarded = false;

	[NonSerialized]
	public SizeTweener SizeTween;
	[NonSerialized]
	public PosTweener PosTween;
	[NonSerialized]
	public RotTweener RotTween;

	public Transform Tweenee
	{
		get { return SizeTween.Tweenee; }
		set { SizeTween.Tweenee = value; PosTween.Tweenee = value; RotTween.Tweenee = value; }
	}
	public bool AreTweensEnabled
	{
		get { return SizeTween.enabled; }
		set { SizeTween.enabled = value; PosTween.enabled = value; RotTween.enabled = value; }
	}
	public float Lerp
	{
		get { return SizeTween.Lerp; }
		set { SizeTween.Lerp = value; PosTween.Lerp = value; RotTween.Lerp = value; }
	}


	public void Reset(CardComponent cmp = null)
	{
		if (GameFSM.Instance.Current.Hand.Count > HandIndex)
		{
			Cmp.MyTr.position = PosTween.Min;
			Cmp.MyTr.localScale = SizeTween.Min;
			Cmp.MyTr.rotation = RotTween.Min;
		}

		Lerp = 0.0f;
		TweenTarget = 0.0f;
		WasDiscarded = false;
		if (cmp == null)
		{
			Tweenee = null;
			AreTweensEnabled = false;
		}
		else
		{
			Tweenee = cmp.MyTr;
			AreTweensEnabled = true;
			cmp.IsSelected = false;
		}

		PosTween.Start();
		RotTween.Start();
		SizeTween.Start();

		if (cmp == null)
			Tweenee = null;
	}


	protected override void Awake()
	{
		base.Awake();

		PosTween = GetComponent<PosTweener>();
		SizeTween = GetComponent<SizeTweener>();
		RotTween = GetComponent<RotTweener>();

		WasDiscarded = false;
	}
	void Start()
	{
		InputController.Instance.OnSweepVertical += OnSelected;
		InputController.Instance.OnSweepSideways += OnDiscarded;
	}

	private void OnSelected(KinematicsTracker palm)
	{
		if (touching.Contains(palm) && gameObject.activeInHierarchy && Tweenee != MyTransform &&
			GameFSM.Instance.Current.Hand.Count > HandIndex)
		{
			if (Cmp.IsSelected)
			{
				TweenTarget = 0.0f;
				Cmp.IsSelected = false;
			}
			else
			{
				TweenTarget = 1.0f;
				Cmp.IsSelected = true;
			}
		}
	}
	private void OnDiscarded(KinematicsTracker palm)
	{
		if (GameFSM.Instance.Current.Hand.Count > HandIndex && Cmp.IsSelected)
		{
			GameFSM.Instance.StartCoroutine(DiscardCoroutine(palm.GetAverageVelocity(InputController.Instance.GestureDuration).x));
		}
	}

	private System.Collections.IEnumerator DiscardCoroutine(float dir)
	{
		SizeTween.Size = SizeTween.Min;

		WasDiscarded = true;
		Cmp.IsSelected = false;
		TweenTarget = 0.0f;
		PosTween.enabled = false;
		RotTween.enabled = false;

		yield return GameFSM.Instance.StartCoroutine(GameActions.DiscardCard(Cmp, HandIndex, dir));
	}
}