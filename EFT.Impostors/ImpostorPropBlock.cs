using System;
using UnityEngine;

namespace EFT.Impostors;

[Serializable]
public struct ImpostorPropBlock
{
	public uint _Index;

	public float _Frames;

	public float _ImpostorSize;

	public float _TextureBias;

	public float _Parallax;

	public float _DepthSize;

	public float _ClipMask;

	public float _AI_ShadowBias;

	public float _AI_ShadowView;

	public float _Hemi;

	public Vector4 _Offset;

	public Vector4 _AI_SizeOffset;

	public Vector2 padding;

	public static int Stride => 80;
}
