using UnityEngine;

namespace Koenigz.PerfectCulling;

[CreateAssetMenu(menuName = "Perfect Culling/PerfectCullingSettings")]
public class PerfectCullingSettings : ScriptableObject
{
	[Range(16f, 2048f)]
	[Tooltip("Resolution for the actual rendering of a single camera perspective (1 out of 6). Increase this if you are experiencing distant object popping. Decrease this if you are memory constrained on your baking system.")]
	public int bakeCameraResolution = 1024;

	public bool debugBakeMode;

	public Material proxyTransparentMaterial;

	public Material sharedOccluderMaterial;

	public float minCullingOccluderSize;

	public int numActivationsPerVolume;

	public bool saveRawBakeData;

	public static PerfectCullingSettings Instance => PerfectCullingResourcesLocator.Instance.Settings;

	public int bakeCameraResolutionWidth => bakeCameraResolution;

	public int bakeCameraResolutionHeight => bakeCameraResolution;
}
