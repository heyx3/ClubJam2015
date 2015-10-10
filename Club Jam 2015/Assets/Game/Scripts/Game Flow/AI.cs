using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Handles the behavior for the computer enemy.
/// </summary>
public static class AI
{
	public static Elements[] ChooseElements(Elements[] playerElements)
	{
		Elements[] allElements = Enum.GetValues(typeof(Elements)).Cast<Elements>().ToArray();

		int rand1 = UnityEngine.Random.Range(0, allElements.Length),
			rand2 = rand1;
		while (rand2 == rand1)
		{
			rand2 = UnityEngine.Random.Range(0, allElements.Length);
		}

		return new Elements[2]
		{
			allElements[rand1],
			allElements[rand2],
		};
	}
}