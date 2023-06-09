using System;
using EFT.Animations;
using UnityEngine;

[Serializable]
public class WalkPreset : ScriptableObject
{
	public AnimVal[] Curves;

	public AnimVal this[int index] => Curves[index];
}
