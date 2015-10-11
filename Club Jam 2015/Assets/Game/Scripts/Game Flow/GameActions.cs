using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using RNG = UnityEngine.Random;


/// <summary>
/// Runs actions that require coroutines and extended amounts of time to complete.
/// </summary>
public class GameActions : Singleton<GameActions>
{
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
			cmp.IsFaceDown = false;

			//Move sideways, then down onto the table.
			MoveTowards mt = cmp.gameObject.AddComponent<MoveTowards>();
			mt.Speed = Consts.DrawCardMoveSpeed;
			mt.SetTarget(new Vector3(handMarker.position.x, cmp.MyTr.position.y, handMarker.position.z));
			mt.OnMovementFinished += mt2 =>
			{
				mt2.SetTarget(handMarker.position);
				mt2.OnMovementFinished += mt3 =>
				{
					GameObject.Destroy(mt3);
				};
			};

			//Wait for all that movement to finish.
			while (mt != null)
			{
				yield return null;
			}
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

			yield return GameFSM.Instance.StartCoroutine(DrawCard(p));
		}
		else
		{
			FSM.IsGameOver = true;
			//TODO: End game.
		}
		
	}
	private static MoveTowards MoveCardToDeck(Card c, Player p)
	{
		p.DiscardPile.Remove(c);
		p.Deck.Add(c);

		Transform deckMarker = GetDeckMarker(p);
		float heightOffset = deckMarker.transform.position.y +
							 Consts.DrawCardStartHeight + (Mathf.Lerp(-1.0f, 1.0f, RNG.value) *
														   Consts.DrawCardStartHeightVariance);

		CardComponent cmp = c.Owner;
		MoveTowards mt = cmp.gameObject.AddComponent<MoveTowards>();
		mt.Speed = Consts.DrawCardMoveSpeed;

		//First move upwards.
		mt.SetTarget(cmp.MyTr.position + new Vector3(0.0f, heightOffset, 0.0f));
		//Then move to the deck position.
		mt.OnMovementFinished += mt2 =>
			{
				mt2.SetTarget(deckMarker.position + new Vector3(0.0f, p.Deck.IndexOf(c) * Consts.DeckCardSeparation, 0.0f));
				
				mt2.OnMovementFinished += mt3 => GameObject.Destroy(mt3);
				
				cmp.IsFaceDown = true;
			};
		
		return mt;
	}
	
	/// <summary>
	/// Draws cards until the given player's hand is full.
	/// This may end the game under certain conditions.
	/// </summary>
	public static IEnumerator DrawCardsToFull(Player p)
	{
		while (!FSM.IsGameOver && p.Hand.Count < Consts.MaxHand)
			yield return GameFSM.Instance.StartCoroutine(DrawCard(p));
	}

	public static IEnumerator DiscardCard(CardComponent cmp, int handIndex, float dir)
	{
		InputController.Instance.DisableAllGestures = true;

		//Update the card lists.
		cmp.IsSelected = false;
		List<Card> hand = FSM.Current.Hand;
		Card c = cmp.MyCard;
		UnityEngine.Assertions.Assert.IsTrue(c == hand[handIndex]);
		FSM.Current.DiscardPile.Add(c);
		hand.RemoveAt(handIndex);

		//Push the card out of the way.

		PFI.PlayerLookTarget = (dir < 0.0f ? PFI.LookTarget_DiscardLeft : PFI.LookTarget_DiscardRight);

		Rigidbody rgd = cmp.gameObject.GetComponent<Rigidbody>();
		rgd.isKinematic = false;

		yield return null;
		
		Vector3 pushDir = PFI.MainCam.right;
		rgd.velocity = pushDir * Consts.DiscardForce * dir * 0.5f;

		yield return new WaitForSeconds(2.0f);

		PFI.PlayerLookTarget = PFI.LookTarget_MyTable;
		rgd.isKinematic = true;

		yield return new WaitForSeconds(1.0f);

		//Move all later cards in the hand down by one.
		for (int i = handIndex; i < hand.Count; ++i)
		{
			MoveTowards mt = hand[i].Owner.gameObject.AddComponent<MoveTowards>();
			mt.Speed = 1.0f;
			mt.SetTarget(GetHandMarkers(FSM.Current)[i].position);
			mt.OnMovementFinished += mt2 =>
				{
					GameObject.Destroy(mt2);
				};
		}

		while (hand.Any(c2 => c2.Owner.GetComponent<MoveTowards>() != null))
			yield return null;

		InputController.Instance.DisableAllGestures = false;

		for (int i = handIndex; i < Consts.MaxHand; ++i)
		{
			PFI.HandButtons[i].Reset(i >= hand.Count ? null : hand[i].Owner);
		}
	}

	public static IEnumerator ShuffleDeck(Player p)
	{
		List<MoveTowards> movers = new List<MoveTowards>();

		//Move all the cards outwards randomly.
		foreach (CardComponent cmp in p.Deck.Select(c => c.Owner))
		{
			movers.Add(cmp.gameObject.AddComponent<MoveTowards>());
			movers[movers.Count - 1].Speed = Consts.ShuffleOutSpeed;
			movers[movers.Count - 1].SetTarget(cmp.MyTr.position + (RNG.insideUnitSphere * Consts.ShuffleOutDist));
			movers[movers.Count - 1].OnMovementFinished += mt =>
				{
					movers.Remove(mt);
				};
		}
		while (movers.Count > 0)
			yield return null;

		//Shuffle the cards: http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
		int n = p.Deck.Count;  
		while (n > 1)
		{  
			n--;
			int k = RNG.Range(0, n + 1);
			Card value = p.Deck[k];  
			p.Deck[k] = p.Deck[n];  
			p.Deck[n] = value;  
		}

		//Push all the cards back in in their new order.
		int count = 0;
		foreach (CardComponent cmp in p.Deck.Select(c => c.Owner))
		{
			Vector3 deckBase = (p.IsAI ? PFI.AIPlayerDeckMarker : PFI.HumanPlayerDeckMarker).position;

			movers.Add(cmp.gameObject.GetComponent<MoveTowards>());
			movers[movers.Count - 1].Speed = Consts.ShuffleInSpeed;
			movers[movers.Count - 1].SetTarget(deckBase + new Vector3(0.0f, count * Consts.DeckCardSeparation, 0.0f));
			movers[movers.Count - 1].OnMovementFinished += mt =>
				{
					movers.Remove(mt);
					GameObject.Destroy(mt);
				};

			count += 1;
		}
		while (movers.Count > 0)
			yield return null;
	}

	public static IEnumerable BurnCardFromDiscard(Card c, Player p)
	{
		//TODO: Implement.
		yield break;
	}
}