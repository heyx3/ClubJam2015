using System;
using System.Collections.Generic;
using UnityEngine;



public enum CardEffectTypes
{
	Attack,
	Buff,
	Debuff,
	Counter,
	Void,
}
[Serializable]
public class ActiveEffects
{
	public CardEffectTypes Type;

	/// <summary>
	/// Only useful for Attack cards.
	/// </summary>
	public int Attack_Health,
			   Attack_Energy;
	/// <summary>
	/// Only useful for Buff or Debuff cards.
	/// </summary>
	public int Buff_Health,
			   Buff_Energy,
			   Buff_Force,
			   Buff_Fortitude;
}


[Serializable]
public class Card
{
	public string Name;
	public Material Mat = null;

	public Elements Type;
	public int TypeCost,
			   UntypeCost;
	ActiveEffects Effects;


	public CardComponent Owner = null;


	public Card(Card c) { Name = c.Name; Mat = c.Mat; Type = c.Type; TypeCost = c.TypeCost; UntypeCost = c.UntypeCost; }
}