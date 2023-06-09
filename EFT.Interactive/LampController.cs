using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Effects;
using Comfort.Common;
using EFT.Visual;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Interactive;

[ExecuteInEditMode]
[_E2E2(20500)]
public sealed class LampController : Turnable, _EBFD, _E3A7
{
	private sealed class _E000
	{
		private readonly int m__E000;

		private int _E001 = -1;

		private float _E002;

		public _E000(int seed)
		{
			m__E000 = seed;
		}

		public bool IsBlink(float time, float bandWidth, float randomness)
		{
			time *= bandWidth;
			int num = (int)time;
			float num2 = time - (float)num;
			if (_E001 != num)
			{
				_E001 = num;
				_E002 = (float)(_E8EE.RandomizeInt(num ^ m__E000) & 0xFF) * 0.003921569f;
				_E002 = 0.5f + (_E002 - 0.5f) * randomness;
			}
			if (num2 <= _E002)
			{
				return false;
			}
			_E002 = float.MaxValue;
			return true;
		}
	}

	private const float m__E000 = 30f;

	[SerializeField]
	private AnimationCurve _animationCurve;

	[SerializeField]
	private bool _useCurve;

	[SerializeField]
	private AudioSource AudioSource;

	[Space(32f)]
	public Light[] Lights = new Light[0];

	public BaseLight[] AreaAndTubeLights = new BaseLight[0];

	public AdvancedLight[] CustomLights = new AdvancedLight[0];

	[SerializeField]
	private MultiFlareLight[] MultiFlareLights = new MultiFlareLight[0];

	[SerializeField]
	private MaterialEmission[] _materialEmissions = new MaterialEmission[0];

	[SerializeField]
	private MaterialColor[] Materials;

	[SerializeField]
	private GameObject[] OnObjects;

	[SerializeField]
	private GameObject[] OffObjects;

	[SerializeField]
	private GameObject[] DestroyedObjects;

	[Header("Working Interval (hours)")]
	public bool isTimeOfDayDependant;

	public bool isCertainInterval;

	[_E2BD(0f, 12f, -1f)]
	public Vector2 MinMaxAmount1 = new Vector2(0f, 7f);

	[_E2BD(12f, 24f, -1f)]
	public Vector2 MinMaxAmount2 = new Vector2(22f, 24f);

	[Header("Audio")]
	public int Rolloff = 60;

	[SerializeField]
	private AudioClip TurnOffClip;

	[SerializeField]
	private AudioClip TurnedOnClipStart;

	[SerializeField]
	private AudioClip TurnedOnClipLoop;

	[SerializeField]
	private AudioClip FlickeringClipLoop;

	[SerializeField]
	private float PitchRandom;

	[Space(32f)]
	[SerializeField]
	private Vector2 TurnedOnLengthMinMax;

	[SerializeField]
	private Vector2 TurnedOffLengthMinMax;

	[SerializeField]
	private float FadeInPower;

	[SerializeField]
	private float FadeOutPower;

	[Space(32f)]
	[SerializeField]
	private bool Blinking;

	[SerializeField]
	private float BlinkingFreq;

	[SerializeField]
	private float SingleBlinkLength;

	[SerializeField]
	[Range(0f, 1f)]
	private float Randomness;

	[Space(32f)]
	public float FlickeringFreq;

	[SerializeField]
	private float TurningOnFlickering;

	[SerializeField]
	private float TurningOffFlickering;

	[SerializeField]
	private float TurnedOnFlickering;

	[Space(32f)]
	[Header("Flick Audio")]
	[SerializeField]
	private AudioClip[] TurnOnClips;

	[SerializeField]
	private float FlickHighTreshold = 0.8f;

	[SerializeField]
	private float FlickLowTreshold = 0.35f;

	[SerializeField]
	private Vector2 FlickVolume = new Vector2(0.3f, 0.7f);

	[Header("Damage")]
	[SerializeField]
	private string SparksEffect = _ED3E._E000(212744);

	[SerializeField]
	private Vector2 SparksFreq = new Vector2(2f, 3f);

	[SerializeField]
	private string DestroyEffect = _ED3E._E000(212742);

	[SerializeField]
	private float IntactTime;

	[Range(0f, 1f)]
	[SerializeField]
	private float JointBreakProbability;

	[SerializeField]
	private Transform SparksEmmiterTransform;

	[SerializeField]
	private Transform DestroyEffectPivot;

	[SerializeField]
	private Vector3 EffectDirection;

	private int m__E001;

	private bool m__E002;

	private bool m__E003 = true;

	private float m__E004;

	private float m__E005;

	private float m__E006;

	private float m__E007;

	private float m__E008;

	private float m__E009;

	private float m__E00A;

	private _E000 _E00B;

	private float _E00C = -1f;

	private float[] _E00D;

	private float[] _E00E;

	private Color[] _E00F;

	private Flicker[] _E010 = new Flicker[0];

	private List<CullingLightObject> _E011 = new List<CullingLightObject>();

	private List<CullingAdvancedLightObject> _E012 = new List<CullingAdvancedLightObject>();

	private List<VolumetricLight> _E013 = new List<VolumetricLight>();

	private float _E014;

	private float _E015;

	private RigidbodySpawner _E016;

	private ConfigurableJointSpawner _E017;

	private Rigidbody _E018;

	private bool _E019;

	private float _E01A;

	private float _E01B;

	private EState _E01C;

	private bool _E01D = true;

	private bool _E01E;

	public bool Enabled
	{
		get
		{
			return _E01D;
		}
		set
		{
			_E01D = value;
			Switch(_E01C);
		}
	}

	public void TryToBlowUp(IExplosiveItem grenade, in Vector3 grenadePosition)
	{
		Vector3 position = base.transform.position;
		float num = Vector3.Distance(position, grenadePosition);
		float strength = Mathf.InverseLerp(grenade.MaxExplosionDistance * 2f, grenade.MinExplosionDistance / 2f, num);
		_E005(strength, NetId ^ (NetId << (int)(num * 10f)));
		if (!(num > grenade.MinExplosionDistance * 2f) && (!Physics.Linecast(grenadePosition, position, out var hitInfo, _E37B.GrenadeObstaclesColliderMask) || hitInfo.transform.IsChildOf(base.transform)))
		{
			BallisticCollider.ApplyHit(new _EC23
			{
				DamageType = EDamageType.Explosion,
				HitPoint = grenadePosition
			}, _EC22.EMPTY_SHOT_ID);
		}
	}

	private void Awake()
	{
		_materialEmissions = _materialEmissions.Where((MaterialEmission mat) => mat != null).ToArray();
		MaterialEmission[] materialEmissions = _materialEmissions;
		for (int i = 0; i < materialEmissions.Length; i++)
		{
			materialEmissions[i].Init();
		}
		MaterialColor[] materials = Materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].Init();
		}
		if (!Application.isPlaying)
		{
			return;
		}
		_E01E = true;
		RegisterForNetwork();
		if (AudioSource != null)
		{
			AudioSource.maxDistance = Rolloff;
		}
		_E017 = GetComponentInChildren<ConfigurableJointSpawner>();
		_E016 = GetComponentInChildren<RigidbodySpawner>();
		Vector3 position = base.transform.position;
		this.m__E001 = (int)Mathf.Repeat(position.x + position.y * 10f + position.z * 100f, 1000f);
		_E00B = new _E000(this.m__E001);
		if (AudioSource != null)
		{
			AudioSource.pitch = 1f + ((float)this.m__E001 / 1000f - 0.5f) * PitchRandom;
		}
		_E00D = new float[Lights.Length];
		_E00E = new float[AreaAndTubeLights.Length];
		_E011 = new List<CullingLightObject>();
		_E012 = new List<CullingAdvancedLightObject>();
		_E013 = new List<VolumetricLight>();
		_E010 = base.gameObject.GetComponentsInChildren<Flicker>();
		for (int j = 0; j < Lights.Length; j++)
		{
			_E00D[j] = Lights[j].intensity;
			CullingLightObject component = Lights[j].GetComponent<CullingLightObject>();
			if (component != null)
			{
				_E011.Add(component);
			}
			VolumetricLight component2 = Lights[j].GetComponent<VolumetricLight>();
			if (component2 != null)
			{
				_E013.Add(component2);
			}
		}
		for (int k = 0; k < AreaAndTubeLights.Length; k++)
		{
			_E00E[k] = AreaAndTubeLights[k].m_Intensity;
			CullingAdvancedLightObject component3 = AreaAndTubeLights[k].GetComponent<CullingAdvancedLightObject>();
			if (component3 != null)
			{
				_E012.Add(component3);
			}
		}
		_E00F = new Color[CustomLights.Length];
		for (int l = 0; l < CustomLights.Length; l++)
		{
			_E00F[l] = CustomLights[l].Color;
		}
		_E006(LampState == EState.TurningOn || LampState == EState.On);
	}

	private void OnDisable()
	{
		_E3A3.UnregisterInSystem(this);
		MultiFlareLight[] multiFlareLights = MultiFlareLights;
		foreach (MultiFlareLight multiFlareLight in multiFlareLights)
		{
			if (multiFlareLight != null)
			{
				multiFlareLight.enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		_E3A3.RegisterInSystem(this);
		if (Application.isPlaying)
		{
			_E009(this.m__E002, force: true);
			return;
		}
		MultiFlareLight[] multiFlareLights = MultiFlareLights;
		foreach (MultiFlareLight multiFlareLight in multiFlareLights)
		{
			if (multiFlareLight != null)
			{
				multiFlareLight.enabled = LampState == EState.TurningOn || LampState == EState.On || LampState == EState.ConstantFlickering;
			}
		}
	}

	private void _E000(bool turnOn)
	{
		if (Enabled)
		{
			if (!turnOn && LampState == EState.On)
			{
				Switch(EState.Off);
			}
			else if (turnOn && LampState == EState.Off)
			{
				Switch(EState.On);
			}
		}
	}

	private void _E001()
	{
		if (!isTimeOfDayDependant || TOD_Sky.Instance == null)
		{
			return;
		}
		if (isCertainInterval)
		{
			float hour = TOD_Sky.Instance.Cycle.Hour;
			if ((hour <= Mathf.Abs(MinMaxAmount1.y) && hour >= Mathf.Abs(MinMaxAmount1.x)) || (hour <= Mathf.Abs(MinMaxAmount2.y) && hour >= Mathf.Abs(MinMaxAmount2.x)))
			{
				_E000(turnOn: true);
			}
			else
			{
				_E000(turnOn: false);
			}
		}
		else
		{
			_E000(!TOD_Sky.Instance.IsDay);
		}
	}

	private void _E002()
	{
		if (!(_E017 == null) && !(_E016 == null))
		{
			_E018 = _E016.Create();
			_E320._E002.SupportRigidbody(_E018, 0f);
			_E018.maxDepenetrationVelocity = 10f;
			_E018.isKinematic = false;
			ConfigurableJoint obj = _E017.Create() as ConfigurableJoint;
			if (obj == null)
			{
				throw new Exception(_ED3E._E000(212720));
			}
			obj.projectionMode = JointProjectionMode.PositionAndRotation;
			obj.enablePreprocessing = false;
			_E01B = 30f;
		}
	}

	private void _E003()
	{
		_E017.Remove();
		_E016.Remove();
		_E018 = null;
	}

	private void _E004()
	{
		if (!(_E018 == null))
		{
			_E01B -= Time.deltaTime;
			if (_E01B <= 0f || _E018.IsSleeping())
			{
				_E003();
			}
		}
	}

	private void _E005(float strength, int randomSeed)
	{
		StartFlickering(strength);
		float num = JointBreakProbability * strength;
		if (!_E019 && num > 0f && _E8EE.Range(randomSeed, 0f, 1f) <= num)
		{
			_E002();
			_E019 = true;
		}
	}

	public void StartFlickering(float strength)
	{
		this.m__E006 = TurnedOnFlickering;
		this.m__E007 = strength * IntactTime / 2f + IntactTime / 2f;
		this.m__E008 = 0f;
		this.m__E004 = (TurnedOnFlickering = 8f * (0.5f + strength));
	}

	public override void Switch(EState switchTo)
	{
		_E01C = switchTo;
		if (!_E01D && (switchTo == EState.On || switchTo == EState.TurningOn || switchTo == EState.ConstantFlickering))
		{
			switchTo = EState.Off;
		}
		switch (switchTo)
		{
		case EState.On:
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SmartEnable();
			}
			_E014 = UnityEngine.Random.Range(TurnedOnLengthMinMax.x, TurnedOnLengthMinMax.y);
			this.m__E009 = Time.time + _E014;
			LampState = EState.TurningOn;
			_E008(TurnedOnClipStart);
			break;
		case EState.Off:
			_E009(on: false, force: true);
			_E015 = UnityEngine.Random.Range(TurnedOffLengthMinMax.x, TurnedOffLengthMinMax.y);
			this.m__E009 = Time.time + _E015;
			LampState = EState.TurningOff;
			break;
		case EState.SmoothOff:
			_E015 = UnityEngine.Random.Range(TurnedOffLengthMinMax.x, TurnedOffLengthMinMax.y);
			this.m__E009 = Time.time + _E015;
			LampState = EState.TurningOff;
			break;
		case EState.Destroyed:
			LampState = EState.Destroyed;
			_E009(on: false, force: true);
			if (Singleton<Effects>.Instantiated)
			{
				Singleton<Effects>.Instance.Emit(DestroyEffect, (DestroyEffectPivot == null) ? base.transform.position : DestroyEffectPivot.position, (DestroyEffectPivot == null) ? base.transform.TransformDirection(EffectDirection) : DestroyEffectPivot.TransformDirection(EffectDirection));
			}
			if (AudioSource != null)
			{
				AudioSource.Stop();
			}
			_E007();
			break;
		case EState.ConstantFlickering:
			_E009(on: true);
			LampState = EState.ConstantFlickering;
			if (FlickeringClipLoop != null)
			{
				_E008(FlickeringClipLoop, loop: true);
			}
			break;
		default:
			_E015 = 0f;
			this.m__E009 = Time.time;
			break;
		case EState.TurningOn:
		case EState.TurningOff:
			break;
		}
	}

	private void _E006(bool on)
	{
		this.m__E009 = float.MaxValue;
		LampState = (on ? EState.On : EState.Off);
		Switch(LampState);
		this.m__E009 = Time.time;
	}

	private void _E007()
	{
		Flicker[] array = _E010;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		_E010 = new Flicker[0];
		foreach (VolumetricLight item in _E013)
		{
			UnityEngine.Object.Destroy(item);
		}
		_E013.Clear();
		foreach (CullingLightObject item2 in _E011)
		{
			UnityEngine.Object.Destroy(item2);
		}
		_E011.Clear();
		foreach (CullingAdvancedLightObject item3 in _E012)
		{
			UnityEngine.Object.Destroy(item3);
		}
		_E012.Clear();
		Light[] lights = Lights;
		for (int i = 0; i < lights.Length; i++)
		{
			UnityEngine.Object.Destroy(lights[i]);
		}
		Lights = new Light[0];
		BaseLight[] areaAndTubeLights = AreaAndTubeLights;
		for (int i = 0; i < areaAndTubeLights.Length; i++)
		{
			UnityEngine.Object.Destroy(areaAndTubeLights[i]);
		}
		AreaAndTubeLights = new BaseLight[0];
		AdvancedLight[] customLights = CustomLights;
		for (int i = 0; i < customLights.Length; i++)
		{
			UnityEngine.Object.Destroy(customLights[i]);
		}
		CustomLights = new AdvancedLight[0];
		_E00F = new Color[0];
	}

	public void ManualUpdate()
	{
		float num = 0f;
		float num2 = 1f;
		bool flag = this.m__E002;
		_E004();
		_E001();
		float time = Time.time;
		switch (LampState)
		{
		case EState.TurningOn:
		{
			if (Blinking)
			{
				if (_E00B.IsBlink(time, BlinkingFreq, Randomness))
				{
					flag = true;
					this.m__E00A = time + SingleBlinkLength;
				}
				else if (time > this.m__E00A)
				{
					flag = false;
					this.m__E00A = float.MaxValue;
				}
			}
			else
			{
				flag = true;
			}
			num = TurningOnFlickering;
			float num3 = ((_E014 > 0f) ? Mathf.Clamp01((this.m__E009 - time) / _E014) : 0f);
			num2 = Mathf.Pow(1f - num3, FadeInPower);
			if (this.m__E009 < time)
			{
				LampState = EState.On;
				this.m__E009 = float.MaxValue;
				_E008(TurnedOnClipLoop, loop: true);
				flag = true;
			}
			break;
		}
		case EState.TurningOff:
			num = TurningOffFlickering;
			num2 = Mathf.Pow((this.m__E009 - time) / _E015, FadeOutPower);
			if (this.m__E009 < time)
			{
				LampState = EState.Off;
				this.m__E009 = float.MaxValue;
				if (TurnOffClip != null)
				{
					_E008(TurnOffClip);
				}
				else if (AudioSource != null)
				{
					AudioSource.Stop();
				}
				flag = false;
			}
			break;
		case EState.On:
			if (this.m__E008 < 1f)
			{
				TurnedOnFlickering = Mathf.Lerp(this.m__E004, this.m__E006, this.m__E008);
				this.m__E008 += Time.deltaTime / this.m__E007;
			}
			else
			{
				TurnedOnFlickering = this.m__E006;
			}
			if (Application.isPlaying && AudioSource != null && !AudioSource.isPlaying && AudioSource.loop && AudioSource.enabled)
			{
				AudioSource.Play();
			}
			num = TurnedOnFlickering;
			break;
		case EState.Destroyed:
			return;
		default:
			throw new ArgumentOutOfRangeException();
		case EState.Off:
		case EState.ConstantFlickering:
			break;
		}
		if (LampState == EState.ConstantFlickering)
		{
			num2 = (_useCurve ? _animationCurve.Evaluate(time * FlickeringFreq) : Mathf.PerlinNoise(time * FlickeringFreq, this.m__E001));
			num2 /= 1.5f;
		}
		else if (num > 0f)
		{
			num2 += (Mathf.PerlinNoise(time * FlickeringFreq, this.m__E001) - 0.5f) * num;
		}
		_E009(flag);
		if (flag)
		{
			_E00A(num2);
		}
		if (AudioSource != null && AudioSource.isPlaying && this.m__E005 < Time.time)
		{
			float num4 = _E8A8.Instance.Distance(base.transform.position);
			this.m__E005 = Time.time + 0.5f;
			if (num4 < (float)Rolloff && Singleton<BetterAudio>.Instantiated)
			{
				AudioSource.outputAudioMixerGroup = Singleton<BetterAudio>.Instance.GetOcclusionGroupSimple(base.transform.position, num4);
			}
		}
		if (this.m__E002 && _E019 && _E01A < Time.time)
		{
			if (Singleton<Effects>.Instantiated)
			{
				Singleton<Effects>.Instance.Emit(SparksEffect, (SparksEmmiterTransform == null) ? base.transform.position : SparksEmmiterTransform.position, Vector3.down);
			}
			_E01A = Time.time + UnityEngine.Random.Range(SparksFreq.x, SparksFreq.y);
		}
	}

	private void _E008([CanBeNull] AudioClip clip, bool loop = false)
	{
		if (clip == null)
		{
			return;
		}
		if (Singleton<BetterAudio>.Instantiated && !loop)
		{
			Vector3 position = base.transform.position;
			Singleton<BetterAudio>.Instance.PlayAtPoint(position, clip, _E8A8.Instance.Distance(position), BetterAudio.AudioSourceGroupType.Impacts, Rolloff, UnityEngine.Random.Range(FlickVolume.x, FlickVolume.y), EOcclusionTest.Fast);
		}
		else if (!(AudioSource == null))
		{
			AudioSource.loop = loop;
			if (!(AudioSource.clip == clip) || !AudioSource.isPlaying)
			{
				AudioSource.clip = clip;
				AudioSource.Play();
			}
		}
	}

	private void _E009(bool on, bool force = false)
	{
		if (on == this.m__E002 && !force)
		{
			return;
		}
		this.m__E002 = on;
		MultiFlareLight[] multiFlareLights = MultiFlareLights;
		for (int i = 0; i < multiFlareLights.Length; i++)
		{
			multiFlareLights[i].enabled = on;
		}
		Light[] lights = Lights;
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].enabled = on;
		}
		foreach (VolumetricLight item in _E013)
		{
			item.enabled = on;
		}
		foreach (CullingLightObject item2 in _E011)
		{
			if (!(item2 == null))
			{
				item2.Switch(on);
			}
		}
		BaseLight[] areaAndTubeLights = AreaAndTubeLights;
		for (int i = 0; i < areaAndTubeLights.Length; i++)
		{
			areaAndTubeLights[i].enabled = on;
		}
		AdvancedLight[] customLights = CustomLights;
		for (int i = 0; i < customLights.Length; i++)
		{
			customLights[i].enabled = on;
		}
		if (Application.isPlaying)
		{
			MaterialEmission[] materialEmissions = _materialEmissions;
			for (int i = 0; i < materialEmissions.Length; i++)
			{
				materialEmissions[i].TurnLights(on);
			}
			MaterialColor[] materials = Materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].TurnLights(on);
			}
		}
		GameObject[] onObjects = OnObjects;
		for (int i = 0; i < onObjects.Length; i++)
		{
			onObjects[i].SetActive(value: false);
		}
		onObjects = OffObjects;
		for (int i = 0; i < onObjects.Length; i++)
		{
			onObjects[i].SetActive(value: false);
		}
		onObjects = DestroyedObjects;
		for (int i = 0; i < onObjects.Length; i++)
		{
			onObjects[i].SetActive(value: false);
		}
		onObjects = ((LampState == EState.Destroyed) ? DestroyedObjects : (this.m__E002 ? OnObjects : OffObjects));
		for (int i = 0; i < onObjects.Length; i++)
		{
			onObjects[i].SetActive(value: true);
		}
	}

	private void _E00A(float intensity)
	{
		if (Math.Abs(intensity - _E00C) < Mathf.Epsilon)
		{
			return;
		}
		if (!this.m__E003)
		{
			this.m__E003 = intensity < FlickLowTreshold;
		}
		else if (intensity > FlickHighTreshold)
		{
			this.m__E003 = false;
			if (TurnOnClips.Length != 0)
			{
				if (Singleton<BetterAudio>.Instantiated)
				{
					Vector3 position = base.transform.position;
					Singleton<BetterAudio>.Instance.PlayAtPoint(position, TurnOnClips[UnityEngine.Random.Range(0, TurnOnClips.Length)], _E8A8.Instance.Distance(position), BetterAudio.AudioSourceGroupType.Impacts, Rolloff, UnityEngine.Random.Range(FlickVolume.x, FlickVolume.y), EOcclusionTest.Fast);
				}
				else
				{
					AudioSource.pitch = 1f + UnityEngine.Random.Range(0f - PitchRandom, PitchRandom);
					AudioSource.PlayOneShot(TurnOnClips[UnityEngine.Random.Range(0, TurnOnClips.Length)], UnityEngine.Random.Range(FlickVolume.x, FlickVolume.y));
				}
			}
		}
		if (!_E01E)
		{
			return;
		}
		_E00C = intensity;
		for (int i = 0; i < Lights.Length; i++)
		{
			Lights[i].intensity = _E00D[i] * intensity;
		}
		for (int j = 0; j < AreaAndTubeLights.Length; j++)
		{
			AreaAndTubeLights[j].m_Intensity = _E00E[j] * intensity;
		}
		foreach (CullingLightObject item in _E011)
		{
			item.IntensityMultiplier = intensity;
		}
		foreach (CullingAdvancedLightObject item2 in _E012)
		{
			item2.IntensityMultiplier = intensity;
		}
		for (int k = 0; k < CustomLights.Length; k++)
		{
			CustomLights[k].Color = _E00F[k] * intensity;
		}
		if (Application.isPlaying)
		{
			MaterialEmission[] materialEmissions = _materialEmissions;
			for (int l = 0; l < materialEmissions.Length; l++)
			{
				materialEmissions[l].SetIntensity(intensity);
			}
			MaterialColor[] materials = Materials;
			for (int l = 0; l < materials.Length; l++)
			{
				materials[l].SetIntensity(intensity);
			}
		}
	}
}
