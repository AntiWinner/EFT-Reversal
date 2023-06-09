using EFT;
using UnityEngine;

namespace Audio.SpatialSystem;

public sealed class SpatialAudioSettings
{
	public bool OcclusionEnabled;

	public float OcclusionIntensity;

	public EOcclusionTest OcclusionTest;

	public float OcclusionDistanceCoefficient;

	public float OcclusionRotationCoefficient;

	public float IndoorToOutdoorFactor;

	public float OutdoorToIndoorFactor;

	public bool IncludeReverbEffect;

	public AnimationCurve ObstructionVolumeCurve;

	public AnimationCurve PropagationVolumeCurve;

	public AnimationCurve OcclusionLowPassFilterCurve;

	public AnimationCurve PropagationLowPassFilterCurve;

	public AnimationCurve OcclusionReverbFilterCurve;

	public readonly AudioFilterFrequencySettings FrequencySettings;

	public SpatialAudioSettings(AudioGroupPreset preset)
	{
		OcclusionEnabled = preset.OcclusionEnabled;
		OcclusionIntensity = preset.OcclusionIntensity;
		OcclusionTest = preset.OcclusionTest;
		OcclusionDistanceCoefficient = preset.OcclusionDistanceCoefficient;
		OcclusionRotationCoefficient = preset.OcclusionRotationCoefficient;
		IndoorToOutdoorFactor = preset.IndoorToOutdoorFactor;
		OutdoorToIndoorFactor = preset.OutdoorToIndoorFactor;
		IncludeReverbEffect = preset.IncludeReverbEffect;
		ObstructionVolumeCurve = preset.OcclusionVolumeCurve;
		PropagationVolumeCurve = preset.PropagationVolumeCurve;
		OcclusionLowPassFilterCurve = preset.OcclusionLowPassFilterCurve;
		PropagationLowPassFilterCurve = preset.PropagationLowPassFilterCurve;
		OcclusionReverbFilterCurve = preset.OcclusionReverbFilterCurve;
		FrequencySettings = preset.FrequencySettings;
	}
}
