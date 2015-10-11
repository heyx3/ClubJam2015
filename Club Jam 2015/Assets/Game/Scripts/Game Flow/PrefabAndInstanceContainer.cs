using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR;


public class PrefabAndInstanceContainer : Singleton<PrefabAndInstanceContainer>
{
	public static Transform InstantiateAt(GameObject prefab, Vector3 pos)
	{
		Transform t = Instantiate(prefab).transform;
		t.position = pos;
		return t;
	}


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

	public GameObject ParticlePrefab_ChooseElemental;

	public GameObject ElementSelectionContainer;

	public Transform HumanPlayerDeckMarker, AIPlayerDeckMarker;
	public Transform[] HumanHandMarkers = new Transform[5],
					   AIHandMarkers = new Transform[5];

	[NonSerialized]
	public CardButton[] HandButtons;

	[NonSerialized]
	public Transform PlayerLookTarget;
	public Transform MainCam;

	public Transform LookTarget_MyTable, LookTarget_Opponent,
					 LookTarget_DiscardLeft, LookTarget_DiscardRight;

	public GameObject ChooseCharacterActionContainer;


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
		Assert(LookTarget_DiscardLeft, "LookTarget_DiscardLeft");
		Assert(LookTarget_DiscardRight, "LookTarget_DiscardRight");
		Assert(LookTarget_Opponent, "LookTarget_Opponent");

		Assert(ParticlePrefab_ChooseElemental, "ParticlePrefab_ChooseElemental");

		Assert(ChooseCharacterActionContainer, "ChooseCharacterActionContainer");

		PlayerLookTarget = LookTarget_MyTable;

		for (int i = 0; i < HumanHandMarkers.Length; ++i)
		{
			Assert(HumanHandMarkers[i], "Human hand marker " + i);
			Assert(AIHandMarkers[i], "AI hand marker " + i);
		}
		HandButtons = HumanHandMarkers.Select(t => t.GetComponentInChildren<CardButton>()).ToArray();

		CardPrefabs = new Dictionary<Elements, GameObject>();
		foreach (ElementAndCardPrefab eacp in cardPrefabs)
			CardPrefabs.Add(eacp.Type, eacp.Prefab);
	}


	void FixedUpdate()
	{
		//Rotate the camera to face the look-at target.
		if (!VRDevice.isPresent)
		{
			Vector3 targPos = PlayerLookTarget.position;
			float dist = (targPos - MainCam.position).magnitude;
			MainCam.LookAt(Vector3.Lerp(MainCam.position + (MainCam.forward * dist),
										PlayerLookTarget.position,
										GameConsts.Instance.CameraLookLerp),
						   new Vector3(0.0f, 1.0f, 0.0f));
		}
	}
}