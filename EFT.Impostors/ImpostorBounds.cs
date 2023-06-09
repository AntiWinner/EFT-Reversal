using System;
using UnityEngine;

namespace EFT.Impostors;

[Serializable]
public struct ImpostorBounds
{
	public Vector3 Center;

	public float Size;

	public float Near;

	public float Far;

	public Vector2 Padding;

	public static int Stride => 32;
}
