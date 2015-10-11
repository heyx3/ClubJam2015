using System;
using System.Collections.Generic;
using UnityEngine;


public class GameConsts : Singleton<GameConsts>
{
	#region Character Data definitions

	[Serializable]
	public class CharacterData
	{
		public int PassiveEnergyPerTurn = 3;
		public int Force, Flow, Fortitude;
	}
	[Serializable]
	public class CharDataForElement
	{
		public Elements CharElement;
		public CharacterData Data;
	}
	[SerializeField]
	private List<CharDataForElement> characters;

	#endregion

	#region Deck/Card definitions

	[Serializable]
	public class ElementalDeck
	{
		[Serializable]
		public class CardData
		{
			public Card Value;
			public int NCopies;
		}

		public List<CardData> Cards = new List<CardData>();
		public Elements Type;

		public ElementalDeck(Elements type) { Type = type; }
	}
	[SerializeField]
	private List<ElementalDeck> decks = new List<ElementalDeck>();

	#endregion


	public Dictionary<Elements, CharacterData> Characters;
	public Dictionary<Elements, List<Card>> Decks = new Dictionary<Elements, List<Card>>();

	public int StartingHP = 20,
			   StartingEnergyPerType = 2;
	public int MaxHand = 5;

	public float DeckCardSeparation = 0.01f;

	public float DiscardForce = 1.0f;

	public float ShuffleOutSpeed = 1.0f,
				 ShuffleInSpeed = 0.25f;
	public float ShuffleOutDist = 0.5f;

	public float DrawCardStartHeight = 3.0f,
				 DrawCardStartHeightVariance = 0.5f;
	public float DrawCardMoveSpeed = 1.0f;

	public float CameraLookLerp = 0.05f;


	protected override void Awake()
	{
		base.Awake();

		//Generate character data.
		Characters = new Dictionary<Elements, CharacterData>();
		foreach (CharDataForElement dat in characters)
			Characters.Add(dat.CharElement, dat.Data);

		//Generate the decks.
		foreach (ElementalDeck d in decks)
		{
			Decks.Add(d.Type, new List<Card>());
			foreach (ElementalDeck.CardData dat in d.Cards)
			{
				Card c = dat.Value;
				for (int i = 0; i < dat.NCopies; ++i)
					Decks[d.Type].Add(new Card(c));
			}
		}
	}
}