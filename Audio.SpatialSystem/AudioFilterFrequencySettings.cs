using System;
using EFT.EnvironmentEffect;
using UnityEngine;

namespace Audio.SpatialSystem;

[Serializable]
public class AudioFilterFrequencySettings
{
	[Range(100f, 22000f)]
	[SerializeField]
	private float _defaultIndoorOcclusionFrequency = 8000f;

	[Range(100f, 22000f)]
	[SerializeField]
	private float _defaultOutdoorOcclusionFrequency = 5000f;

	[SerializeField]
	[Range(100f, 22000f)]
	private float _lowerOcclusionFrequency = 450f;

	[Range(100f, 22000f)]
	[SerializeField]
	private float _upperOcclusionFrequency = 700f;

	[Range(100f, 22000f)]
	[SerializeField]
	private float _wallOcclusionFrequency = 1600f;

	[Range(100f, 22000f)]
	[SerializeField]
	private float _fullOcclusionFrequency = 100f;

	[Range(100f, 22000f)]
	[SerializeField]
	private float _distanceHighCutFrequency = 12000f;

	public float DistanceHighCutFrequency => _distanceHighCutFrequency;

	public float MaxHighFrequency => 22000f;

	public float GetLowerFrequency(Vector3 soundPosition, Vector3 listenerPosition, float obstruction, float propagation, bool isolated = false)
	{
		int num;
		float num2;
		if (EnvironmentManager.Instance != null)
		{
			num = (EnvironmentManager.Instance.IsPlayerAndTargetOutdoor(soundPosition) ? 1 : 0);
			if (num != 0)
			{
				num2 = _defaultOutdoorOcclusionFrequency;
				goto IL_002f;
			}
		}
		else
		{
			num = 0;
		}
		num2 = _defaultIndoorOcclusionFrequency;
		goto IL_002f;
		IL_002f:
		float num3 = num2;
		if (num != 0)
		{
			return num3;
		}
		if (propagation > 0f)
		{
			num3 = _wallOcclusionFrequency;
		}
		if (isolated)
		{
			float t = ((propagation > 0f) ? propagation : obstruction);
			return Mathf.Lerp(num3, _fullOcclusionFrequency, t);
		}
		if (propagation <= 0f || !_E486.IsNeedVerticalOcclusion(soundPosition, listenerPosition))
		{
			return num3;
		}
		return (_E486.CalculateVerticalDot(soundPosition, listenerPosition) > 0f) ? _upperOcclusionFrequency : _lowerOcclusionFrequency;
	}
}
