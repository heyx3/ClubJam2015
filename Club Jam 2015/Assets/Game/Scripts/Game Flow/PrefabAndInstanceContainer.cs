using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR;


public class PrefabAndInstanceContainer : Singleton<PrefabAndInstanceContainer>
{
	#region Card Prefabs

	[Serializable]
	public class ElementAndCardPrefab
	{
		public Elements Type;
		public GameObject Prefab;
	}
	[SerializeField]
	private List<ElementAndCardPrefab> cardPrefabs = new List<ElementAndCardPrefab>();

	#endregion

	public Dictionary<Elements, GameObject> CardPrefabs;

	public GameObject ElementSelectionContainer;

	public Transform HumanPlayerDeckMarker, AIPlayerDeckMarker;
	public Transform[] HumanHandMarkers = new Transform[5],
					   AIHandMarkers = new Transform[5];

	[NonSerialized]
	public Transform PlayerLookTarget;
	public Transform MainCam;

	public Transform LookTarget_MyTable, LookTarget_Opponent, LookTarget_DiscardPile;


	private void Assert(object g, string objName)
	{
		if (g == null)
		{
			Debug.LogError("'" + objName + "' in 'PrefabAndInstanceContainer' component is null");
		}
	}
	protected override void Awake()
	{
		base.Awake();

		Assert(ElementSelectionContainer, "ElementSelectionContainer");
		Assert(HumanPlayerDeckMarker, "HumanPlayerDeckMarker");
		Assert(AIPlayerDeckMarker, "AIPlayerDeckMarker");

		Assert(LookTarget_MyTable, "LookTarget_MyTable");
		Assert(LookTarget_DiscardPile, "LookTarget_DiscardPile");
		Assert(LookTarget_Opponent, "LookTarget_Opponent");

		PlayerLookTarget = LookTarget_MyTable;

		for (int i = 0; i < HumanHandMarkers.Length; ++i)
		{
			Assert(HumanHandMarkers[i], "Human hand marker " + i);
			Assert(AIHandMarkers[i], "AI hand marker " + i);
		}

		CardPrefabs = new Dictionary<Elements, GameObject>();
		foreach (ElementAndCardPrefab eacp in cardPrefabs)
			CardPrefabs.Add(eacp.Type, eacp.Prefab);
	}


	void FixedUpdate()
	{
		//Rotate the camera to face the look-at target.
		if (!VRDevice.isPresent)
		{
			MainCam.transform.LookAt(Vector3.Lerp(MainCam.transform.forward,
												  (PlayerLookTarget.position - MainCam.transform.position).normalized,
												  GameConsts.Instance.CameraLookLerp).normalized,
									 new Vector3(0.0f, 1.0f, 0.0f));
		}
	}
}