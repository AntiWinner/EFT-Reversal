using System;
using Audio.SpatialSystem;
using UnityEngine;
using UnityEngine.Audio;

public class SuperSource : BetterSource
{
	public AudioSource source2;

	private SpatialLowPassFilter _E004;

	private SpatialAudioSource _E005;

	public override bool Loop
	{
		get
		{
			return source1.loop;
		}
		set
		{
			AudioSource audioSource = source1;
			bool loop = (source2.loop = value);
			audioSource.loop = loop;
		}
	}

	public override AudioClip GetClip(int id)
	{
		if (id != 0)
		{
			return source2.clip;
		}
		return source1.clip;
	}

	private void Start()
	{
		Init();
	}

	public override void SetMixerGroup(AudioMixerGroup mixerGroup)
	{
		AudioSource audioSource = source1;
		AudioMixerGroup outputAudioMixerGroup = (source2.outputAudioMixerGroup = mixerGroup);
		audioSource.outputAudioMixerGroup = outputAudioMixerGroup;
	}

	public override bool VolumeFadeOut(float time, Action onDone = null)
	{
		throw new NotImplementedException();
	}

	public override bool VolumeFadeIn(float time, Action onDone = null)
	{
		throw new NotImplementedException();
	}

	public override void SetBaseVolume(float volume)
	{
		AudioSource audioSource = source1;
		float volume2 = (source2.volume = volume);
		audioSource.volume = volume2;
	}

	public override void SetSpatialSettings(SpatialAudioSettings settings)
	{
		base.SpatialSettings = settings;
		LowPassFilter.SetOcclusionCurves(settings.OcclusionLowPassFilterCurve, settings.PropagationLowPassFilterCurve);
		_E004.SetOcclusionCurves(settings.OcclusionLowPassFilterCurve, settings.PropagationLowPassFilterCurve);
	}

	public override void Init()
	{
		if (!(source1 != null))
		{
			source1 = GetComponent<AudioSource>();
			source2 = _E001();
			source2.spatialBlend = source1.spatialBlend;
			source2.SetCustomCurve(AudioSourceCurveType.Spread, source1.GetCustomCurve(AudioSourceCurveType.Spread));
			source2.rolloffMode = source1.rolloffMode;
			source2.SetCustomCurve(AudioSourceCurveType.CustomRolloff, source1.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
			source2.maxDistance = source1.maxDistance;
			source2.minDistance = source1.minDistance;
			source2.outputAudioMixerGroup = source1.outputAudioMixerGroup;
			_E005 = _E000();
			if (!(LowPassFilter != null))
			{
				LowPassFilter = GetComponent<SpatialLowPassFilter>();
				_E004 = _E002();
			}
		}
	}

	public override void SetLowPassFilterParameters(float value, ESoundOcclusionType occlusionType, float lowerFreq = 1600f, float highestFreq = 22000f, bool applyImmediately = false)
	{
		if (IncludedInOcclusionProcess && !(LowPassFilter == null))
		{
			value *= base.SpatialSettings.OcclusionIntensity;
			LowPassFilter.SetLowerFrequency(lowerFreq);
			LowPassFilter.SetHighestFrequency(highestFreq);
			LowPassFilter.SetFilterParams(value, applyImmediately, occlusionType);
			_E004.SetLowerFrequency(lowerFreq);
			_E004.SetHighestFrequency(highestFreq);
			_E004.SetFilterParams(value, applyImmediately, occlusionType);
		}
	}

	protected override void ResetFilters()
	{
		if ((object)LowPassFilter != null)
		{
			LowPassFilter.ResetFilter();
			_E004.ResetFilter();
		}
	}

	public override void PlayScheduled(double time)
	{
		base.PlayScheduled(time);
		EndPlaybackTime = CalculateEndPlaybackTime(source1.clip, source2.clip, (float)time);
		source1.volume = source1.volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		source2.volume = source2.volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		if (source1.clip != null)
		{
			source1.PlayScheduled(time);
		}
		if (source2.clip != null)
		{
			source2.PlayScheduled(time);
		}
	}

	public override void SetScheduledEndTime(double time)
	{
		source1.SetScheduledEndTime(time);
		source2.SetScheduledEndTime(time);
	}

	public override void SetPitch(float p)
	{
		AudioSource audioSource = source1;
		float pitch = (source2.pitch = p);
		audioSource.pitch = pitch;
	}

	public override void Play(AudioClip clip1, AudioClip clip2, float balance, float volume = 1f, bool forceStereo = false, bool oneShot = true)
	{
		base.Play(clip1, clip2, balance, volume, forceStereo);
		volume = volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		source1.clip = clip1;
		source1.volume = balance * volume;
		source1.Play();
		source2.clip = clip2;
		if (!(clip2 == null))
		{
			source2.clip = clip2;
			source2.volume = (1f - balance) * volume;
			source2.Play();
		}
	}

	public override void Clear(float spatial = 1f, float pitch = 1f)
	{
		Loop = false;
		AudioSource audioSource = source1;
		AudioClip clip = (source2.clip = null);
		audioSource.clip = clip;
		AudioSource audioSource2 = source1;
		float spatialBlend = (source2.spatialBlend = spatial);
		audioSource2.spatialBlend = spatialBlend;
		SetPitch(pitch);
	}

	public override void SetPriority(int priority)
	{
		base.SetPriority(priority);
		source2.priority = priority;
	}

	public override void Balance(float p)
	{
		source1.volume = p;
		source2.volume = 1f - p;
	}

	public override void Enable(bool p0)
	{
		AudioSource audioSource = source1;
		bool flag2 = (source2.enabled = p0);
		audioSource.enabled = flag2;
	}

	public override void SetRolloff(float distance)
	{
		AudioSource audioSource = source1;
		float maxDistance = (source2.maxDistance = distance);
		audioSource.maxDistance = maxDistance;
	}

	protected override void RefreshSpatialization(bool enabledSpat)
	{
		base.RefreshSpatialization(enabledSpat);
		source2.spatialize = enabledSpat;
		if (_E005 != null)
		{
			_E005.EnableSpatialization(enabledSpat);
		}
	}

	private SpatialAudioSource _E000()
	{
		return base.transform.GetChild(0).GetComponent<SpatialAudioSource>();
	}

	private AudioSource _E001()
	{
		return base.transform.GetChild(0).GetComponent<AudioSource>();
	}

	private SpatialLowPassFilter _E002()
	{
		return base.transform.GetChild(0).GetComponent<SpatialLowPassFilter>();
	}
}
