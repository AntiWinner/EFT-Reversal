using System;
using UnityEngine;

namespace EFT.Interactive;

[Serializable]
public class VolumeConnection
{
	public BetterPropagationVolume Volume;

	public EVolumeRelations Relations;

	[Range(0f, 1f)]
	public float ConnectionAudibility = 1f;

	public bool UseManualAudibilitySettings;

	[HideInInspector]
	public EVolumeRelationsMask RelationsMask;
}
