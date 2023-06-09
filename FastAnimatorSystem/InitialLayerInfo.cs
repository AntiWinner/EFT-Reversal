using System;
using UnityEngine;

namespace FastAnimatorSystem;

[Serializable]
public struct InitialLayerInfo
{
	public bool isAdditive;

	public float weight;

	public AvatarMask avatarMask;
}
