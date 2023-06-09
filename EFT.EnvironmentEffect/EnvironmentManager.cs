using System.Collections;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Weather;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.EnvironmentEffect;

public class EnvironmentManager : EnvironmentManagerBase
{
	[Space]
	[SerializeField]
	private AudioSource OutdoorSource;

	[SerializeField]
	private AudioSource OutdoorMixSource;

	[SerializeField]
	private AudioSource BunkerSource;

	[SerializeField]
	private AudioSource IndoorSource;

	[Header("Rain")]
	[SerializeField]
	private AudioSource Rain1;

	[SerializeField]
	private AudioSource Rain2;

	[SerializeField]
	private AudioClip[] OutdoorRainClips;

	[SerializeField]
	private AudioClip[] IndoorRainClips;

	[SerializeField]
	private float NightBlendStart = 0.1f;

	[SerializeField]
	private float NightBlendEnd;

	[Header("Outdoor")]
	[SerializeField]
	private float OutdoorFadeTime = 0.25f;

	[SerializeField]
	private float OutdoorExposureSpeed = 2f;

	[SerializeField]
	private float OutdoorExposureOffset = 0.23f;

	[SerializeField]
	private float OutdoorRainVolume = 1f;

	[Header("Long Shadow Reduising")]
	public bool EnableLongShadowsCorrection = true;

	[_E2BD(0f, 24f, -1f)]
	[SerializeField]
	private Vector2 ShadowInterval1 = new Vector2(5f, 10f);

	[_E2BD(0f, 24f, -1f)]
	[SerializeField]
	private Vector2 ShadowInterval2 = new Vector2(17f, 23f);

	[SerializeField]
	private float ShadowDecreaseFactor = 3f;

	[SerializeField]
	private float ShadowMinDistance = 20f;

	[CompilerGenerated]
	private EnvironmentType m__E002;

	[CompilerGenerated]
	private bool m__E003;

	[CompilerGenerated]
	private float m__E004;

	[CompilerGenerated]
	private float m__E005;

	private AudioSource m__E006;

	private RainController.ERainIntensity _E007;

	private float _E008;

	private float _E009;

	private float _E00A = 1f;

	private Coroutine _E00B;

	private Coroutine _E00C;

	private float _E00D;

	private float _E00E = 0.25f;

	private float _E00F;

	private float _E010;

	private ThermalVision _E011;

	private int _E012;

	public EnvironmentType Environment
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public bool InBunker
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	public float PrismExposureSpeed
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E004 = value;
		}
	}

	public float PrismExposureOffset
	{
		[CompilerGenerated]
		get
		{
			return this.m__E005;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E005 = value;
		}
	}

	public new static EnvironmentManager Instance => EnvironmentManagerBase._instance as EnvironmentManager;

	private new RainController.ERainIntensity _E000
	{
		get
		{
			if (!InBunker)
			{
				return RainController.IntensityType;
			}
			return RainController.ERainIntensity.None;
		}
	}

	private float _E001
	{
		get
		{
			float b = 20f;
			if (Singleton<_E7DE>.Instantiated)
			{
				b = Singleton<_E7DE>.Instance.Graphics.Settings.ShadowDistance;
			}
			return Mathf.Max(ShadowMinDistance, b);
		}
	}

	private bool _E013
	{
		get
		{
			if (_E011 == null)
			{
				if (_E8A8.Instance == null)
				{
					return false;
				}
				_E011 = _E8A8.Instance.ThermalVision;
				if (_E011 == null)
				{
					return false;
				}
			}
			return _E011.On;
		}
	}

	protected override void Init()
	{
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.TransitToEnvironment(Environment);
		}
		this.m__E006 = Rain1;
		_E006(BunkerSource, isActive: true);
		_E00E = OutdoorFadeTime;
		_E00A = OutdoorRainVolume;
		_E008 = OutdoorExposureSpeed;
		_E009 = OutdoorExposureOffset;
		QualitySettings.shadowDistance = this._E001;
		base.Init();
	}

	public EnvironmentType GetEnvironmentByPos(Vector3 pos)
	{
		if (!(TryFindTriggerByPos(pos) == null))
		{
			return EnvironmentType.Indoor;
		}
		return EnvironmentType.Outdoor;
	}

	public void UpdateEnvironmentForPlayer(Player player)
	{
		IndoorTrigger trigger = TryFindTriggerByPos(player.Transform.position);
		SetTriggerForPlayer(player, trigger);
	}

	public bool IsPlayerAndTargetInDifferentEnvironments(Vector3 sourcePos)
	{
		EnvironmentType environment = Environment;
		EnvironmentType environmentByPos = GetEnvironmentByPos(sourcePos);
		return environment != environmentByPos;
	}

	public bool IsPlayerAndTargetOutdoor(Vector3 targetPos)
	{
		EnvironmentType environmentByPos = GetEnvironmentByPos(targetPos);
		if (Environment == EnvironmentType.Outdoor)
		{
			return environmentByPos == EnvironmentType.Outdoor;
		}
		return false;
	}

	protected override void SetTriggerForPlayer(Player player, IndoorTrigger trigger)
	{
		EnvironmentType environment = ((!(trigger == null)) ? EnvironmentType.Indoor : EnvironmentType.Outdoor);
		player.SetEnvironment(environment);
		player.AIData?.SetEnvironment(trigger);
		if (!player.IsYourPlayer)
		{
			return;
		}
		Environment = environment;
		if (trigger == null)
		{
			_E00E = OutdoorFadeTime;
			_E00A = OutdoorRainVolume;
			_E008 = OutdoorExposureSpeed;
			_E009 = OutdoorExposureOffset;
			_E010 = 0f;
			_E00F = 0f;
			bool inBunker = InBunker;
			InBunker = false;
			if (inBunker)
			{
				_E00B = StartCoroutine(_E000());
			}
		}
		else
		{
			_E00E = trigger.FadeTime;
			_E00A = trigger.RainVolume;
			_E008 = trigger.ExposureSpeed;
			_E009 = trigger.ExposureOffset;
			_E010 = 0f;
			_E00F = 0f;
			bool inBunker2 = InBunker;
			InBunker = trigger.IsBunker;
			if (inBunker2 || trigger.IsBunker)
			{
				this.TryStopCoroutine(ref _E00B);
				_E00B = StartCoroutine(InBunker ? _E001() : _E000());
			}
		}
		Singleton<BetterAudio>.Instance.TransitToEnvironment(Environment);
		_E002();
	}

	[CanBeNull]
	public IndoorTrigger TryFindTriggerByPos(Vector3 pos)
	{
		if (_rootTriggerGroup == null)
		{
			return null;
		}
		return _rootTriggerGroup.Check(pos);
	}

	private IEnumerator _E000()
	{
		float num = ((WeatherController.Instance == null) ? 1f : WeatherController.Instance.SunHeight);
		_E006(OutdoorSource, num > NightBlendEnd);
		_E006(OutdoorMixSource, num < NightBlendStart);
		_E006(IndoorSource, isActive: true);
		yield return new WaitForSeconds(0.25f);
		_E006(BunkerSource, isActive: false);
	}

	private IEnumerator _E001()
	{
		_E006(BunkerSource, isActive: true);
		yield return new WaitForSeconds(0.25f);
		_E006(OutdoorSource, isActive: false);
		_E006(OutdoorMixSource, isActive: false);
		_E006(IndoorSource, isActive: false);
	}

	private void _E002()
	{
		this.TryStopCoroutine(ref _E00C);
		_E00C = StartCoroutine(_E003(1f));
	}

	private IEnumerator _E003(float speed)
	{
		AudioSource audioSource = ((this.m__E006 == Rain1) ? Rain2 : Rain1);
		if (audioSource == null)
		{
			yield break;
		}
		if (this._E000 == RainController.ERainIntensity.None)
		{
			audioSource.clip = null;
		}
		else
		{
			int num = (int)(this._E000 - 1);
			audioSource.clip = ((Environment == EnvironmentType.Outdoor) ? OutdoorRainClips[num] : IndoorRainClips[num]);
		}
		if (audioSource.clip != null)
		{
			audioSource.Play();
		}
		float volume = this.m__E006.volume;
		for (float num2 = 0f; num2 < 1f; num2 += Time.deltaTime * speed)
		{
			if (this.m__E006.clip != null)
			{
				this.m__E006.volume = Mathf.Lerp(volume, 0f, num2);
			}
			if (audioSource.clip != null)
			{
				audioSource.volume = Mathf.Lerp(0f, _E00A, num2);
			}
			yield return null;
		}
		this.m__E006.Stop();
		this.m__E006 = audioSource;
	}

	private void Update()
	{
		if (EnableLongShadowsCorrection)
		{
			QualitySettings.shadowDistance = _E004() * this._E001;
		}
		PrismExposureOffset = (_E013 ? OutdoorExposureOffset : Mathf.SmoothDamp(PrismExposureOffset, _E009, ref _E00F, _E00E));
		PrismExposureSpeed = Mathf.SmoothDamp(PrismExposureSpeed, _E008, ref _E010, _E00E);
		_E005();
	}

	private float _E004()
	{
		if (Environment == EnvironmentType.Indoor)
		{
			return 1f;
		}
		if (TOD_Sky.Instance == null || !Singleton<_E7DE>.Instantiated)
		{
			return 1f;
		}
		float hour = TOD_Sky.Instance.Cycle.Hour;
		if (hour > ShadowInterval1.x && hour < ShadowInterval1.y)
		{
			float t = (hour - ShadowInterval1.x) / (ShadowInterval1.y - ShadowInterval1.x);
			return Mathf.Lerp(1f / ShadowDecreaseFactor, 1f, t);
		}
		if (hour > ShadowInterval2.x && hour < ShadowInterval2.y)
		{
			float num = (hour - ShadowInterval2.x) / (ShadowInterval2.y - ShadowInterval2.x);
			return Mathf.Lerp(1f / ShadowDecreaseFactor, 1f, 1f - num);
		}
		return 1f;
	}

	private void _E005()
	{
		if (!(Time.time - _E00D < 2f))
		{
			_E00D = Time.time;
			float num = ((WeatherController.Instance == null) ? 1f : WeatherController.Instance.SunHeight);
			_E006(OutdoorSource, num > NightBlendEnd);
			_E006(OutdoorMixSource, num < NightBlendStart);
			if (num > NightBlendEnd && num < NightBlendStart)
			{
				float num2 = Mathf.InverseLerp(NightBlendStart, NightBlendEnd, num);
				OutdoorSource.volume = 1f - num2;
				OutdoorMixSource.volume = num2;
			}
			RainController.ERainIntensity eRainIntensity = this._E000;
			if (eRainIntensity != _E007)
			{
				_E007 = eRainIntensity;
				this.TryStopCoroutine(ref _E00C);
				_E00C = StartCoroutine(_E003(0.33f));
			}
		}
	}

	private void _E006(AudioSource source, bool isActive)
	{
		if (!(source == null))
		{
			source.enabled = isActive;
		}
	}

	private void OnDestroy()
	{
		QualitySettings.shadowDistance = this._E001;
		EnvironmentManagerBase._instance = null;
	}
}
