using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerTextDisplay : MonoBehaviour
{
	public enum Displays
	{
		Health,
		Energy,
		Fortitude,
		Force,
		Flow,
	}

	private static string EnergyTextFor(Elements el)
	{
		switch (el)
		{
			case Elements.Fire: return "F";
			case Elements.Air: return "A";
			case Elements.Water: return "Wa";
			case Elements.Earth: return "E";
			case Elements.Wood: return "Wo";
			case Elements.Metal: return "M";
			default: throw new InvalidOperationException();
		}
	}


	public TextMesh TMesh;
	public Displays Type;


	void Update()
	{
		Player p = GameFSM.Instance.HumanPlayer;
		if (p == null || p.Energy == null)
		{
			TMesh.text = "";
			return;
		}

		switch (Type)
		{
			case Displays.Health:
				TMesh.text = p.HP.ToString() + "HP";
				break;

			case Displays.Energy:
				string energy = "";
				foreach (Elements el in Enum.GetValues(typeof(Elements)).Cast<Elements>())
					energy += EnergyTextFor(el) + p.Energy[el].ToString() + "\n";
				TMesh.text = energy.Substring(0, energy.Length - 1);
				break;

			case Displays.Flow:
				TMesh.text = "Flow: " + p.CurrentFlow.ToString();
				break;
			case Displays.Force:
				TMesh.text = "Force: " + p.CurrentForce.ToString();
				break;
			case Displays.Fortitude:
				TMesh.text = "Fortitude: " + p.CurrentFortitude.ToString();
				break;

			default:
				Debug.LogError("Unknown display type " + Type);
				break;
		}
	}
}