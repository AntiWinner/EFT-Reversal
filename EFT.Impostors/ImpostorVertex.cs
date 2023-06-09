using System;
using UnityEngine;

namespace EFT.Impostors;

[Serializable]
public struct ImpostorVertex
{
	public Vector4 Vertex;

	public static int Stride => 16;
}
