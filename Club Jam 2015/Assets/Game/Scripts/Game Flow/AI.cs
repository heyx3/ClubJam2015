using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RNG = UnityEngine.Random;


/// <summary>
/// Handles the behavior for the computer enemy.
/// </summary>
public static class AI
{
	public static Elements[] ChooseElements(List<Elements> playerElements)
	{
		Elements[] allElements = Enum.GetValues(typeof(Elements)).Cast<Elements>().ToArray();

		int rand1 = RNG.Range(0, allElements.Length),
			rand2 = rand1;
		while (rand2 == rand1)
		{
			rand2 = RNG.Range(0, allElements.Length);
		}

		return new Elements[2]
		{
			allElements[rand1],
			allElements[rand2],
		};
	}
	public static List<int> ChooseToDiscardHand(Player p)
	{
		List<int> vals = new List<int>();

		for (int i = 0; i < GameConsts.Instance.MaxHand; ++i)
		{
			if (RNG.Range(0.0f, 1.0f) > 0.77777f)
			{
				vals.Add(i);
			}
		}

		return vals;
	}
}