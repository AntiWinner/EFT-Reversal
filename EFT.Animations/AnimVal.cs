using System;
using UnityEngine;

namespace EFT.Animations;

[Serializable]
public class AnimVal
{
	public AnimationCurve Curve;

	public float TimeShift;

	[_E2BE("To 160;Component 100;Intensity 100", false)]
	public Val[] Vals;
}
