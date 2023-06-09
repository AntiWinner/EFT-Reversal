using UnityEngine;

public class SmokeGrenadeSettings : GrenadeSettings
{
	[SerializeField]
	public SphereCollider _emissionArea;

	[SerializeField]
	public AnimationCurve _sizeOverTime;

	[SerializeField]
	public float _initialRadius;

	[SerializeField]
	public float _radiusMultiplier = 1f;

	[SerializeField]
	public Vector3 _pivot;

	[SerializeField]
	public Vector3 _torque;

	[SerializeField]
	public float _torqueDelta = 0.3f;

	[SerializeField]
	public float _areaStartPosNorm = 0.5f;

	private void OnValidate()
	{
		if (_emissionArea != null)
		{
			_initialRadius = _emissionArea.radius;
			_pivot = _emissionArea.transform.localPosition;
		}
	}
}
