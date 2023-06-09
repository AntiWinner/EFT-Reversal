using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class TOD_ReflectionParameters
{
	[Tooltip("Reflection probe mode.")]
	public TOD_ReflectionType Mode;

	[Tooltip("Clear flags to use for the reflection.")]
	public ReflectionProbeClearFlags ClearFlags = ReflectionProbeClearFlags.Skybox;

	[Tooltip("Layers to include in the reflection.")]
	public LayerMask CullingMask = 0;

	[Tooltip("Time slicing behaviour to spread out rendering cost over multiple frames.")]
	public ReflectionProbeTimeSlicingMode TimeSlicing;

	[Tooltip("Refresh interval of the reflection cubemap in seconds.")]
	[_E058(0f)]
	public float UpdateInterval = 1f;
}
