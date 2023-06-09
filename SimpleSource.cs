using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Audio.SpatialSystem;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleSource : BetterSource
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public SimpleSource _003C_003E4__this;

		public Action onDone;

		internal void _E000()
		{
			_003C_003E4__this._E002 = false;
			if (onDone != null)
			{
				onDone();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public SimpleSource _003C_003E4__this;

		public Action onDone;

		internal void _E000()
		{
			_003C_003E4__this._E003 = false;
			if (onDone != null)
			{
				onDone();
			}
		}
	}

	private new float m__E000;

	private IEnumerator m__E001;

	private bool _E002;

	private bool _E003;

	public override bool Loop
	{
		get
		{
			return source1.loop;
		}
		set
		{
			source1.loop = value;
		}
	}

	public override void Init()
	{
		if ((object)LowPassFilter == null)
		{
			LowPassFilter = GetComponent<SpatialLowPassFilter>();
		}
		if ((object)Spatializer == null)
		{
			Spatializer = GetComponent<SpatialAudioSource>();
		}
		if (!(source1 != null))
		{
			source1 = GetComponent<AudioSource>();
		}
	}

	public override AudioClip GetClip(int id)
	{
		return source1.clip;
	}

	public override void SetLowPassFilterParameters(float value, ESoundOcclusionType occlusionType, float lowerFreq = 1600f, float highestFreq = 22000f, bool applyImmediately = false)
	{
		if (IncludedInOcclusionProcess && !(LowPassFilter == null))
		{
			value *= base.SpatialSettings.OcclusionIntensity;
			LowPassFilter.SetLowerFrequency(lowerFreq);
			LowPassFilter.SetHighestFrequency(highestFreq);
			LowPassFilter.SetFilterParams(value, applyImmediately, occlusionType);
		}
	}

	protected override void ResetFilters()
	{
		if ((object)LowPassFilter != null)
		{
			LowPassFilter.ResetFilter();
		}
	}

	public override void SetMixerGroup(AudioMixerGroup mixerGroup)
	{
		source1.outputAudioMixerGroup = mixerGroup;
	}

	public override void PlayScheduled(double time)
	{
		base.PlayScheduled(time);
		EndPlaybackTime = CalculateEndPlaybackTime(source1.clip, null, (float)time);
		float volume = source1.volume;
		volume = volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		source1.volume = volume;
		source1.PlayScheduled(time);
	}

	public override void SetScheduledEndTime(double time)
	{
		source1.SetScheduledEndTime(time);
	}

	public override void SetPitch(float p)
	{
		source1.pitch = p;
	}

	public override void Play(AudioClip clip1, AudioClip clip2, float balance, float volume = 1f, bool forceStereo = false, bool oneShot = true)
	{
		base.Play(clip1, clip2, balance, volume, forceStereo);
		volume = volume * Preset.OverallVolume * base.OcclusionVolumeFactor;
		if (!oneShot && clip1 != null)
		{
			source1.clip = clip1;
			source1.volume = volume;
			source1.Play();
		}
		else if (clip2 != null && clip1 != null)
		{
			source1.PlayOneShot(clip1, balance * volume);
			source1.PlayOneShot(clip2, (1f - balance) * volume);
		}
		else if (clip1 != null)
		{
			source1.PlayOneShot(clip1, volume);
		}
		else if (clip2 != null)
		{
			source1.PlayOneShot(clip2, volume);
		}
	}

	public override void Clear(float spatial = 1f, float pitch = 1f)
	{
		Loop = false;
		source1.clip = null;
		source1.spatialBlend = spatial;
		source1.pitch = pitch;
	}

	public override void Balance(float p)
	{
		Debug.LogWarning(string.Format(_ED3E._E000(46085)));
	}

	public override void Enable(bool p0)
	{
		source1.enabled = p0;
	}

	public override void SetRolloff(float distance)
	{
		source1.maxDistance = distance;
	}

	public override bool VolumeFadeOut(float time, Action onDone = null)
	{
		if (source1 == null)
		{
			Debug.LogError(_ED3E._E000(46167));
			return false;
		}
		if (this._E002 || Mathf.Approximately(source1.volume, 0f))
		{
			return false;
		}
		_E000();
		this._E002 = true;
		this.m__E001 = _E001(this.m__E000, 0f, time, delegate
		{
			this._E002 = false;
			if (onDone != null)
			{
				onDone();
			}
		});
		StartCoroutine(this.m__E001);
		return true;
	}

	public override bool VolumeFadeIn(float time, Action onDone = null)
	{
		if (_E003 || Mathf.Approximately(source1.volume, this.m__E000))
		{
			return false;
		}
		_E000();
		_E003 = true;
		this.m__E001 = _E001(0f, this.m__E000, time, delegate
		{
			_E003 = false;
			if (onDone != null)
			{
				onDone();
			}
		});
		StartCoroutine(this.m__E001);
		return true;
	}

	public override void SetBaseVolume(float volume)
	{
		this.m__E000 = volume;
		source1.volume = this.m__E000;
	}

	public override void SetSpatialSettings(SpatialAudioSettings settings)
	{
		base.SpatialSettings = settings;
		LowPassFilter.SetOcclusionCurves(settings.OcclusionLowPassFilterCurve, settings.PropagationLowPassFilterCurve);
	}

	private void _E000()
	{
		if (this.m__E001 != null)
		{
			StopCoroutine(this.m__E001);
		}
		this._E002 = false;
		_E003 = false;
	}

	private IEnumerator _E001(float from, float to, float time, Action onDone = null)
	{
		float num = 0f;
		float num2 = 0f;
		while (num2 <= 1f)
		{
			num2 = num / time;
			source1.volume = Mathf.Lerp(from, to, num2);
			yield return null;
			num += Time.deltaTime;
		}
		onDone?.Invoke();
		this.m__E001 = null;
	}
}
