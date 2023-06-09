using System;
using UnityEngine;

[Serializable]
public class TOD_CloudParameters
{
	[_E05A(0f, 1f)]
	[Tooltip("Density of the clouds.")]
	public float Density = 1f;

	[_E058(0f)]
	[Tooltip("Sharpness of the clouds.")]
	public float Sharpness = 3f;

	[_E058(0f)]
	[Tooltip("Brightness of the clouds.")]
	public float Brightness = 1f;

	[Tooltip("Number of billboard clouds to instantiate at start.\nBillboard clouds are not visible in edit mode.")]
	[_E058(0f)]
	public int Billboards;

	[Tooltip("Opacity of the cloud shadows.")]
	[_E05A(0f, 1f)]
	public float ShadowStrength;

	[Tooltip("Scale of the first cloud layer.")]
	public Vector2 Scale1 = new Vector2(3f, 3f);

	[Tooltip("Scale of the second cloud layer.")]
	public Vector2 Scale2 = new Vector2(7f, 7f);
}
