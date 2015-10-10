using System;
using System.Collections.Generic;
using UnityEngine;


public class PrefabAndInstanceContainer : Singleton<PrefabAndInstanceContainer>
{
	public GameObject ElementSelectionContainer;


	protected override void Awake()
	{
		base.Awake();

		if (ElementSelectionContainer == null)
		{
			Debug.LogError("'ElementSelectionContainer' is null");
		}
	}
}