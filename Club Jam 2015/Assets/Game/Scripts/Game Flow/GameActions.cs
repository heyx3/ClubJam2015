using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


/// <summary>
/// Runs actions that require coroutines and extended amounts of time to complete.
/// </summary>
public class GameActions : Singleton<GameActions>
{
	private static System.Random RNG = new System.Random();


	private static GameFSM FSM { get { return GameFSM.Instance; } }
	private static GameConsts Consts { get { return GameConsts.Instance; } }
	private static PrefabAndInstanceContainer PFI { get { return PrefabAndInstanceContainer.Instance; } }


	private static Transform GetDeckMarker(Player p)
	{
		return (p.IsAI ? PFI.AIPlayerDeckMarker : PFI.HumanPlayerDeckMarker);
	}
	private static Transform[] GetHandMarkers(Player p)
	{
		return (p.IsAI ? PFI.AIHandMarkers : PFI.HumanHandMarkers);
	}


	public static IEnumerator DrawCard(Player p)
	{
		if (p.Hand.Count > Consts.MaxHand)
		{
			Debug.LogError("Trying to draw more than the max number of cards!");
		}

		if (p.Deck.Count > 0)
		{
			p.Hand.Add(p.Deck[p.Deck.Count - 1]);
			p.Deck.RemoveAt(p.Deck.Count - 1);

			int index = p.Hand.Count - 1;
			Transform handMarker = GetHandMarkers(p)[index];
			CardComponent cmp = p.Hand[index].Owner;

			//Move upwards, then move towards the free spot on the table.
			MoveTowards mt = cmp.gameObject.AddComponent<MoveTowards>();
			mt.SetTarget(handMarker.position);
			mt.OnMovementFinished += mt2 =>
			{
				GameObject.Destroy(mt2);
			};

			//Wait for all that movement to finish.
			while (mt != null)
				yield return null;
		}
		else if (p.DiscardPile.Count > 0)
		{
			//No cards in deck, so get cards from discard pile.
			//First shuffle the discard pile: http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
			System.Random rng = new System.Random();
			int n = p.DiscardPile.Count;  
			while (n > 1)
			{  
				n--;  
				int k = rng.Next(n + 1);  
				Card value = p.DiscardPile[k];  
				p.DiscardPile[k] = p.DiscardPile[n];  
				p.DiscardPile[n] = value;  
			}
			
			//Move all the discarded cards into the deck.
			List<MoveTowards> movements = new List<MoveTowards>();
			foreach (Card c in p.DiscardPile)
			{
				movements.Add(MoveCardToDeck(c, p));
			}
			
			//Wait for all that movement to finish.
			while (movements.Count > 0)
			{
				yield return null;
				movements.RemoveAll(mt => mt == null);
			}
		}
		else
		{
			FSM.IsGameOver = true;
			//TODO: End game.
		}

		yield break;
	}
	private static MoveTowards MoveCardToDeck(Card c, Player p)
	{
		p.DiscardPile.Remove(c);
		p.Deck.Add(c);

		Transform deckMarker = GetDeckMarker(p);
		float heightOffset = deckMarker.transform.position.y +
							 Consts.DrawCardStartHeight + (Mathf.Lerp(-1.0f, 1.0f,
																	  (float)RNG.Next()) *
														   Consts.DrawCardStartHeightVariance);

		CardComponent cmp = c.Owner;
		MoveTowards mt = cmp.gameObject.AddComponent<MoveTowards>();

		//First move upwards.
		mt.SetTarget(cmp.MyTr.position + new Vector3(0.0f, heightOffset, 0.0f));
		//Then move to the deck position.
		mt.OnMovementFinished += mt2 =>
			{
				mt2.SetTarget(deckMarker.position + new Vector3(0.0f, p.Deck.IndexOf(c) * Consts.DeckCardSeparation, 0.0f));
				
				mt2.OnMovementFinished += mt3 => GameObject.Destroy(mt3);
			};
		
		return mt;
	}
	
	/// <summary>
	/// Draws cards until the given player's hand is full.
	/// This may end the game under certain conditions.
	/// </summary>
	public static IEnumerable DrawCardsToFull(Player p)
	{
		while (!FSM.IsGameOver && p.Hand.Count < Consts.MaxHand)
			yield return DrawCard(p);
	}
}