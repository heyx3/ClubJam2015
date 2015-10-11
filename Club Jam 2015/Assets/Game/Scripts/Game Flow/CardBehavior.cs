using System;
using System.Collections.Generic;
using UnityEngine;


public static class CardBehavior
{
	private static GameFSM FSM { get { return GameFSM.Instance; } }
	private static Player Current { get { return FSM.Current; } }
	private static Player Opponent { get { return FSM.Opponent; } }


	public delegate void CardAction();
	public delegate void CardReaction();
	public delegate bool CardPredicate(Card c);


	public static Dictionary<string, CardAction> PrimaryActions = new Dictionary<string, CardAction>()
	{

	};
	public static Dictionary<string, CardAction> SecondaryActions = new Dictionary<string, CardAction>()
	{

	};
	public static Dictionary<string, CardPredicate> CanCounter = new Dictionary<string, CardPredicate>()
	{

	};
	public static Dictionary<string, CardReaction> CounterActions = new Dictionary<string, CardReaction>()
	{

	};
}