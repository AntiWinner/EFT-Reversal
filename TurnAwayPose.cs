using UnityEngine;

public class TurnAwayPose : ScriptableObject
{
	[_E2BE("Curve 240;Component 40;Intensity 90;Intensity1 90;AltIntensity 90;AltIntensity1 90", false)]
	[Header("Order: Intensity FP, Intensity TP")]
	public TurnAwayEffector.AnimVal[] Pos;

	[_E2BE("Curve 240;Component 40;Intensity 90;Intensity1 90;AltIntensity 90;AltIntensity1 90", false)]
	public TurnAwayEffector.AnimVal[] Rot;
}
