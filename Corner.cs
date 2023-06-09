using System;
using UnityEngine;

[Serializable]
public class Corner
{
	public Vector3 Pos;

	[NonSerialized]
	public CustomNavigationPoint PointsGroup;
}
