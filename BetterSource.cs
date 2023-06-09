using System;
using System.Collections;
using System.Diagnostics;
using Audio.SpatialSystem;
using EFT;
using UnityEngine;
using UnityEngine.Audio;

public abstract class BetterSource : MonoBehaviour, _E3F7._E000
{
	public enum EPlayBackState
	{
		Playing,
		Stopped,
		Released
	}

	public interface _E000
	{
		void Release(BetterSource source);
	}

	private const float TOO_CLOSE_FOR_BINAURAL = 1.5f;

	public AudioSource source1;

	protected float EndPlaybackTime;

	protected AudioGroupPreset Preset;

	private Transform _transform;

	private bool _forceStereo;

	protected bool IncludedInOcclusionProcess = true;

	protected SpatialLowPassFilter LowPassFilter;

	protected SpatialAudioSource Spatializer;

	public bool checkEveryFrame;

	public EPlayBackState PlayBackState = EPlayBackState.Released;

	public _E000 ReleaseListener;

	public float LowPassFilterLevel => LowPassFilter.FilterLevel;

	public float LowPassFilterFrequency => LowPassFilter.CutoffFrequency;

	public float OcclusionVolumeFactor { get; protected set; } = 1f;


	public SpatialAudioSettings SpatialSettings { get; protected set; }

	public Vector3 Position
	{
		get
		{
			return _transform.position;
		}
		set
		{
			_transform.position = value;
		}
	}

	public float MaxDistance
	{
		get
		{
			if (!(source1 != null))
			{
				return float.MaxValue;
			}
			return source1.maxDistance;
		}
	}

	public abstract bool Loop { get; set; }

	public event Action<BetterSource> OnReleased = delegate
	{
	};

	public abstract AudioClip GetClip(int id);

	protected abstract void ResetFilters();

	public abstract void SetLowPassFilterParameters(float value, ESoundOcclusionType occlusionType, float lowerFreq = 1600f, float highestFreq = 22000f, bool applyImmediately = false);

	public void IncludeInOcclusionProcess(bool included)
	{
		IncludedInOcclusionProcess = included;
	}

	public virtual void SetOcclusionVolumeFactor(float value)
	{
		if (IncludedInOcclusionProcess)
		{
			OcclusionVolumeFactor = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public void ResetOcclusionWithDelay(float delay = 5f)
	{
		if (base.isActiveAndEnabled)
		{
			StartCoroutine(_E000(delay));
		}
	}

	private IEnumerator _E000(float delay = 1f)
	{
		yield return new WaitForSeconds(delay);
		ResetOcclusion();
	}

	public void ResetOcclusion()
	{
		SetOcclusionVolumeFactor(1f);
		ResetFilters();
	}

	public virtual void Release()
	{
		IncludeInOcclusionProcess(included: true);
		ResetOcclusionWithDelay();
		_forceStereo = false;
		PlayBackState = EPlayBackState.Released;
		ReleaseListener?.Release(this);
		this.OnReleased?.Invoke(this);
	}

	public void Awake()
	{
		PlayBackState = EPlayBackState.Stopped;
		_transform = base.transform;
	}

	public virtual void PlayScheduled(double time)
	{
		_E002();
		PlayBackState = EPlayBackState.Playing;
	}

	public abstract void SetScheduledEndTime(double time);

	public abstract void Init();

	public abstract void SetPitch(float p);

	public virtual void Play(AudioClip clip1, AudioClip clip2, float balance, float volume = 1f, bool forceStereo = false, bool oneShot = true)
	{
		EndPlaybackTime = CalculateEndPlaybackTime(clip1, clip2);
		PlayBackState = EPlayBackState.Playing;
		_forceStereo = forceStereo;
		_E002();
	}

	public abstract void Clear(float spatial = 1f, float pitch = 1f);

	public abstract void Balance(float p);

	public abstract void Enable(bool p0);

	public abstract void SetRolloff(float distance);

	public abstract void SetMixerGroup(AudioMixerGroup mixerGroup);

	public abstract bool VolumeFadeOut(float time, Action onDone = null);

	public abstract bool VolumeFadeIn(float time, Action onDone = null);

	public abstract void SetBaseVolume(float volume);

	public abstract void SetSpatialSettings(SpatialAudioSettings settings);

	public virtual void SetPriority(int priority)
	{
		source1.priority = priority;
	}

	public void SetPreset(AudioGroupPreset preset)
	{
		Preset = preset;
	}

	protected virtual void RefreshSpatialization(bool enabledSpat)
	{
		if (Spatializer != null)
		{
			Spatializer.EnableSpatialization(enabledSpat);
		}
		source1.spatialize = enabledSpat;
	}

	[Conditional("UNITY_EDITOR")]
	protected void CheckIfClipLoaded(AudioClip clip)
	{
		if (clip != null && clip.loadState != AudioDataLoadState.Loaded && clip.preloadAudioData)
		{
			UnityEngine.Debug.LogWarning(_ED3E._E000(35598) + clip.name + _ED3E._E000(35588) + clip.loadState);
		}
	}

	protected float CalculateEndPlaybackTime(AudioClip clip1, AudioClip clip2, float delay = 0f)
	{
		float num = Mathf.Max((clip1 != null) ? clip1.length : 0f, (clip2 != null) ? clip2.length : 0f);
		return Time.time + num + delay;
	}

	private void Update()
	{
		if (checkEveryFrame || PlayBackState == EPlayBackState.Playing)
		{
			_E002();
		}
		_E001();
	}

	private void _E001()
	{
		if (PlayBackState == EPlayBackState.Playing && Time.time > EndPlaybackTime && !source1.isPlaying)
		{
			PlayBackState = EPlayBackState.Stopped;
		}
	}

	private void _E002()
	{
		if (_forceStereo)
		{
			RefreshSpatialization(enabledSpat: false);
		}
		else if (Preset.DirectBinaural && Preset.DisableBinauralByDist)
		{
			bool enabledSpat = _E8A8.Instance.Distance(Position) > 1.5f;
			RefreshSpatialization(enabledSpat);
		}
	}

	private void OnDisable()
	{
		PlayBackState = EPlayBackState.Stopped;
	}
}
