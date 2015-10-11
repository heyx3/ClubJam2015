using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;


[Serializable]
public class Player
{
	public Elements PrimaryChar, SecondChar;

	public int HP;
	public Dictionary<Elements, int> Energy;

	public List<Card> Hand = new List<Card>(),
					  Deck = new List<Card>(),
					  DiscardPile = new List<Card>();

	public bool IsAI;
	

	public int CurrentForce { get { return GameConsts.Instance.Characters[PrimaryChar].Force; } }
	public int CurrentFlow { get { return GameConsts.Instance.Characters[PrimaryChar].Flow; } }
	public int CurrentFortitude { get { return GameConsts.Instance.Characters[PrimaryChar].Fortitude; } }
	public int PassiveEnergyPerTurn { get { return GameConsts.Instance.Characters[SecondChar].PassiveEnergyPerTurn; } }


	public Player(Elements primary, Elements secondary, bool isAI)
	{
		PrimaryChar = primary;
		SecondChar = secondary;
		IsAI = isAI;

		//Generate energy.
		Energy = new Dictionary<Elements, int>();
		foreach (Elements el in Enum.GetValues(typeof(Elements)).Cast<Elements>())
		{
			Energy.Add(el, 0);
		}
		Energy[PrimaryChar] += GameConsts.Instance.StartingEnergyPerType;
		Energy[SecondChar] += GameConsts.Instance.StartingEnergyPerType;
		
		//Generate the deck.
		foreach (Card c in GameConsts.Instance.Decks[PrimaryChar].Concat(GameConsts.Instance.Decks[SecondChar]))
		{
			Card c2 = new Card(c);
			Deck.Add(c2);
		}

		//Shuffle the deck: http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
		System.Random rng = new System.Random();
		int n = Deck.Count;  
		while (n > 1)
		{  
			n--;  
			int k = rng.Next(n + 1);  
			Card value = Deck[k];  
			Deck[k] = Deck[n];  
			Deck[n] = value;  
		}
		
		//Create corresponding GameObjects.
		int count = 0;
		Transform container = (isAI ? PrefabAndInstanceContainer.Instance.AIPlayerDeckMarker :
									  PrefabAndInstanceContainer.Instance.HumanPlayerDeckMarker);
		foreach (Card c in Deck)
		{
			GameObject prefab = PrefabAndInstanceContainer.Instance.CardPrefabs[c.Type];
			Transform cT = GameObject.Instantiate<GameObject>(prefab).transform;

			cT.parent = container;
			cT.localPosition = new Vector3(0.0f, -count * GameConsts.Instance.DeckCardSeparation, 0.0f);
			cT.parent = null;
			cT.GetComponent<CardComponent>().MyCard = c;

			count += 1;
		}
	}
}