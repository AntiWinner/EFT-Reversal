using System;
using System.Collections.Generic;
using Audio.SpatialSystem;
using UnityEngine;
using UnityEngine.Audio;

public class SuperSourceDistant : BetterSource
{
	public float Delay;

	public float SpatialBlend = 1f;

	private Queue<_E3BE> _E006 = new Queue<_E3BE>();

	private AudioSource _E007;

	private SpatialAudioSource _E005;

	private SpatialLowPassFilter _E004;

	private double _E008;

	public override bool Loop
	{
		get
		{
			return source1.loop;
		}
		set
		{
			AudioSource audioSource = source1;
			bool loop = (_E007.loop = value);
			audioSource.loop = loop;
		}
	}

	public override void PlayScheduled(double time)
	{
		throw new NotImplementedException();
	}

	public override void SetScheduledEndTime(double time)
	{
		throw new NotImplementedException();
	}

	public override AudioClip GetClip(int id)
	{
		if (id != 0)
		{
			return _E007.clip;
		}
		return source1.clip;
	}

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		if (!(source1 != null))
		{
			source1 = GetComponent<AudioSource>();
			_E007 = _E001();
			_E007.spatialBlend = source1.spatialBlend;
			_E007.spread = source1.spread;
			_E007.rolloffMode = source1.rolloffMode;
			_E007.maxDistance = source1.maxDistance;
			_E007.minDistance = source1.minDistance;
			_E007.outputAudioMixerGroup = source1.outputAudioMixerGroup;
			_E005 = _E000();
			if (!(LowPassFilter != null))
			{
				LowPassFilter = GetComponent<SpatialLowPassFilter>();
				_E004 = _E002();
			}
		}
	}

	public override void SetPitch(float p)
	{
		AudioSource audioSource = source1;
		float pitch = (_E007.pitch = p);
		audioSource.pitch = pitch;
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

	public override void Play(AudioClip clip1, AudioClip clip2, float balance, float volume = 1f, bool forceStereo = false, bool oneShot = true)
	{
		base.Play(clip1, clip2, balance, volume, forceStereo);
		volume = volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		source1.clip = clip1;
		source1.volume = balance * volume;
		source1.spatialBlend = SpatialBlend;
		source1.PlayDelayed(Delay);
		_E007.clip = clip2;
		if (!(clip2 == null))
		{
			_E007.enabled = true;
			_E007.clip = clip2;
			_E007.volume = (1f - balance) * volume;
			_E007.spatialBlend = SpatialBlend;
			_E007.PlayDelayed(Delay);
		}
	}

	public override void Clear(float spatial = 1f, float pitch = 1f)
	{
		Loop = false;
		AudioSource audioSource = source1;
		AudioClip clip = (_E007.clip = null);
		audioSource.clip = clip;
		SetPitch(pitch);
		AudioSource audioSource2 = source1;
		float spatialBlend = (_E007.spatialBlend = spatial);
		audioSource2.spatialBlend = spatialBlend;
	}

	public override void Balance(float p)
	{
		source1.volume = p;
		_E007.volume = 1f - p;
	}

	public override void Enable(bool p0)
	{
		AudioSource audioSource = source1;
		bool flag2 = (_E007.enabled = p0);
		audioSource.enabled = flag2;
	}

	public override void SetRolloff(float distance)
	{
		AudioSource audioSource = source1;
		float maxDistance = (_E007.maxDistance = distance);
		audioSource.maxDistance = maxDistance;
	}

	public override void SetMixerGroup(AudioMixerGroup mixerGroup)
	{
		AudioSource audioSource = source1;
		AudioMixerGroup outputAudioMixerGroup = (_E007.outputAudioMixerGroup = mixerGroup);
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
		float volume2 = (_E007.volume = volume);
		audioSource.volume = volume2;
	}

	public override void SetSpatialSettings(SpatialAudioSettings settings)
	{
		base.SpatialSettings = settings;
		LowPassFilter.SetOcclusionCurves(settings.OcclusionLowPassFilterCurve, settings.PropagationLowPassFilterCurve);
		_E004.SetOcclusionCurves(settings.OcclusionLowPassFilterCurve, settings.PropagationLowPassFilterCurve);
	}

	public void EnqueueImportant(AudioClip clip1, AudioClip clip2, float balance, float delay, float clipTime)
	{
		_E006.Clear();
		_ = clip1 != source1.clip;
	}

	public void ProlongCurrent(float clipTime)
	{
		_E008 += clipTime;
		source1.SetScheduledEndTime(_E008);
		_E007.SetScheduledEndTime(_E008);
	}

	protected override void RefreshSpatialization(bool enabledSpat)
	{
		base.RefreshSpatialization(enabledSpat);
		_E007.spatialize = enabledSpat;
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
