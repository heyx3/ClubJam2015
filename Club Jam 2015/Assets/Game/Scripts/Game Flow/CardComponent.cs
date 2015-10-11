using System;
using UnityEngine;


public class CardComponent : MonoBehaviour
{
	public Card MyCard
	{
		get { return myCard; }
		set
		{
			myCard.Owner = null;

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