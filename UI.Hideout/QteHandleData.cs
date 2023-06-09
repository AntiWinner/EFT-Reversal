using System;
using System.Collections.Generic;
using EFT;
using EFT.Hideout;
using UnityEngine;

namespace UI.Hideout;

[Serializable]
public class QteHandleData : QteData
{
	public enum EPropsTarget
	{
		LeftPalm,
		RightPalm
	}

	[Serializable]
	public class PropsVariantData : PropsData
	{
		public int Trigger;
	}

	[Serializable]
	public class PropsData
	{
		public EPropsTarget Target;

		public GameObject Prefab;

		public Vector3 Position;

		public Vector3 Rotation;
	}

	[SerializeField]
	public Vector3 PlayerPosition;

	[SerializeField]
	public Vector3 PlayerRotation;

	[Space(10f)]
	[SerializeField]
	public List<PropsData> Props = new List<PropsData>();

	[SerializeField]
	[Space(10f)]
	public RuntimeAnimatorController HideoutAnimatorController;

	[Space(10f)]
	public Dictionary<string, SoundBank> Sounds = new Dictionary<string, SoundBank>();

	public QteHandleData(QteData backendData)
	{
		Update(backendData);
	}
}
