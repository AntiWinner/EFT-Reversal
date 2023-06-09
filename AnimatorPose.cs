using UnityEngine;

[CreateAssetMenu]
public class AnimatorPose : ScriptableObject
{
	public Vector3 Position;

	public Vector3 Rotation;

	public Vector3 CameraRotation;

	public Vector3 CameraPosition;

	public AnimationCurve Blend;
}
