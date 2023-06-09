using System;
using Audio.SpatialSystem;
using SteamAudio;
using UnityEngine;

namespace EFT;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable objects/AudioGroupPreset", fileName = "AudioGroupPreset")]
public class AudioGroupPreset : ScriptableObject
{
	[Range(0f, 1f)]
	public float OverallVolume = 1f;

	[Range(0f, 1f)]
	public float SpatialBlend = 1f;

	public bool SteamSpatialize = true;

	public bool SteamSpatializePostEffects;

	public bool DirectBinaural = true;

	public bool IndirectBinaural = true;

	public bool BilinearHRTF = true;

	public bool DisableBinauralByDist;

	public bool Reflections;

	public bool PhysicalBaseAttenuation;

	public bool AirAbsorption;

	public float EnableBinauralDist;

	public float HeightDiffToAllowBinaural;

	public float AngleToAllowBinaural;

	public OcclusionMode OcclusionMode;

	public OcclusionMethod OcclusionMethod = OcclusionMethod.Partial;

	[Range(0.1f, 10f)]
	public float OcclusionSourceRadius = 1f;

	[Range(0f, 1f)]
	public float DirectSoundMix = 1f;

	[Range(0f, 10f)]
	public float IndirectSoundMix = 1f;

	[Range(-1f, 1f)]
	public float DeltaPitch;

	public int AudioSourcePriority = 128;

	public int PreCachedSourcesCount = 2;

	public int SourcesCountLimit = 100;

	public float DefaultMaxDistance = 500f;

	[Header("----- Occlusion Settings -----")]
	public bool OcclusionEnabled = true;

	[Range(0f, 1f)]
	public float OcclusionIntensity;

	public EOcclusionTest OcclusionTest;

	[Range(0f, 0.5f)]
	public float OcclusionDistanceCoefficient = 0.5f;

	[Range(0f, 0.2f)]
	public float OcclusionRotationCoefficient = 0.2f;

	[Range(0.1f, 2f)]
	public float IndoorToOutdoorFactor = 1f;

	[Range(0.1f, 2f)]
	public float OutdoorToIndoorFactor = 1f;

	public bool IncludeReverbEffect = true;

	public AnimationCurve OcclusionVolumeCurve;

	public AnimationCurve OcclusionLowPassFilterCurve;

	public AnimationCurve PropagationVolumeCurve;

	public AnimationCurve PropagationLowPassFilterCurve;

	public AnimationCurve OcclusionReverbFilterCurve;

	public AudioFilterFrequencySettings FrequencySettings;
}
