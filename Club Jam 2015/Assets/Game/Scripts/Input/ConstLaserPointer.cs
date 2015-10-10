using System;
using UnityEngine;


public class ConstLaserPointer : MonoBehaviour
{
	public Transform MyTransform { get; private set; }
	private Quaternion startRot;


	void Awake()
	{
		MyTransform = transform;
		startRot = MyTransform.rotation;
	}
	void Update()
	{
		MyTransform.rotation = startRot;
	}
}