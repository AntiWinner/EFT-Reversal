using System;
using UnityEngine;

[Serializable]
public class TOD_LightParameters
{
	[Tooltip("Refresh interval of the light source position in seconds.")]
	[_E058(0f)]
	public float UpdateInterval;

	[Tooltip("Controls how low the light source is allowed to go.\n = -1 light source can go as low as it wants.\n = 0 light source will never go below the horizon.\n = +1 light source will never leave zenith.")]
	[_E05A(-1f, 1f)]
	public float MinimumHeight;
}
