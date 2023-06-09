using UnityEngine;

[CreateAssetMenu]
public class BreathParameters : ScriptableObject
{
	public AnimationCurve AmplitudeCurve;

	public AnimationCurve Delay;

	public AnimationCurve Hardness;
}
