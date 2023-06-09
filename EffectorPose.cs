using EFT.Animations;
using UnityEngine;

public class EffectorPose : ScriptableObject
{
	[_E2BE("Curve 240;Component 80;Intensity 120;AltIntensity 120", false)]
	public AnimVal[] Pos;

	[_E2BE("Curve 240;Component 80;Intensity 120;AltIntensity 120", false)]
	public AnimVal[] Rot;
}
