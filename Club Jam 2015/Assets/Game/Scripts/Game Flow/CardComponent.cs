using System;
using UnityEngine;


public class CardComponent : MonoBehaviour
{
	public Card MyCard
	{
		get { return myCard; }
		set
		{
			if (myCard != null)
			{
				myCard.Owner = null;
			}

			myCard = value;

			if (myCard != null)
			{
				myCard.Owner = this;
				CardRend.material = myCard.Mat;
			}
		}
	}
	private Card myCard = null;
	
	public Transform MyTr { get; private set; }


	public bool IsSelected = false;
	public bool IsFaceDown
	{
		get
		{
			return (MyTr.localScale.z < 0.0f);
		}
		set
		{
			if (value != IsFaceDown)
				MyTr.localScale = new Vector3(MyTr.localScale.x, MyTr.localScale.y, -MyTr.localScale.z);
		}
	}

	public Renderer CardRend;

	
	void Awake()
	{
		MyTr = transform;
		if (myCard != null)
		{
			myCard.Owner = this;
			CardRend.material = myCard.Mat;
		}
		
		if (CardRend == null)
		{
			Debug.LogError("'CardRend' is null!");
		}
	}
}