using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Audio.SpatialSystem;
using Comfort.Common;
using DG.Tweening;
using EFT;
using EFT.Interactive;
using EFT.ItemGameSounds;
using JetBrains.Annotations;
using JsonType;
using UnityEngine;
using UnityEngine.Audio;

public sealed class BetterAudio : MonoBehaviourSingleton<BetterAudio>
{
	public enum AudioSourceGroupType
	{
		Gunshots,
		Weaponry,
		Impacts,
		Character,
		Environment,
		Collisions,
		Speech,
		Distant,
		NonspatialBypass,
		Nonspatial,
		Voip,
		Grenades
	}

	public class _E000
	{
		private const float _E000 = 5f;

		private const float _E001 = 0.2f;

		public Vector3 Position;

		public Vector2 Radius;

		public float EndTime;

		public float Rolloff;

		public bool IsActive => Time.time < EndTime;

		public bool IsChoked(Vector3 newPosition, Vector3 listener, float distance)
		{
			Vector3 normalized = (Position - listener).normalized;
			float t = Mathf.InverseLerp(5f, Rolloff, distance);
			float num = Mathf.Lerp(Radius.x, Radius.y, t);
			return Vector3.Distance(Position + normalized * (num - 0.2f), newPosition) < num;
		}

		public void Display()
		{
			if (!(EndTime - Time.time < 0f))
			{
				Vector3 normalized = (Position - _E8A8.Instance.Camera.transform.position).normalized;
				float t = Mathf.InverseLerp(5f, Rolloff, _E8A8.Instance.Distance(Position));
				float num = Mathf.Lerp(Radius.x, Radius.y, t);
				Vector3 center = Position + normalized * (num - 0.2f);
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawSphere(center, num);
			}
		}
	}

	public interface _E001
	{
		AudioGroupPreset Preset { get; }

		float DeltaPitch { get; }

		AudioMixerGroup MixerGroup { get; set; }

		string DebugName { get; }

		[CanBeNull]
		BetterSource BorrowSource(bool activateSource = true);

		void RefreshSourceSettings();

		void SetSpatialize(bool val);

		void ForceChangePreset(SpatialAudioSettings preset);
	}

	public class _E002<_E077> : BetterSource._E000, _E001 where _E077 : BetterSource
	{
		private _E077 _E000;

		[CompilerGenerated]
		private AudioGroupPreset _E001;

		[CompilerGenerated]
		private AudioMixerGroup m__E002;

		public float DeltaPitch => Preset.DeltaPitch;

		public AudioGroupPreset Preset
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			protected set
			{
				_E001 = value;
			}
		}

		public AudioMixerGroup MixerGroup
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
			[CompilerGenerated]
			set
			{
				_E002 = value;
			}
		}

		public string DebugName => Preset.name;

		public _E002(AudioMixerGroup mixerGroup, GameObject prefab, AudioGroupPreset preset)
		{
			MixerGroup = mixerGroup;
			Preset = preset;
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
			gameObject.name = DebugName;
			AudioSource component = gameObject.GetComponent<AudioSource>();
			component.rolloffMode = AudioRolloffMode.Custom;
			component.SetCustomCurve(AudioSourceCurveType.CustomRolloff, EFTHardSettings.Instance.SoundRolloff);
			component.spatialBlend = 0f;
			component.reverbZoneMix = 0f;
			component.SetCustomCurve(AudioSourceCurveType.Spread, EFTHardSettings.Instance.SpreadCurve);
			component.outputAudioMixerGroup = MixerGroup;
			_E077 val = gameObject.AddComponent<_E077>();
			val.Init();
			val.SetPreset(Preset);
			val.SetSpatialSettings(new SpatialAudioSettings(Preset));
			val.transform.parent = Singleton<BetterAudio>.Instance.transform;
			val.SetPitch(1f);
			val.SetPriority(Preset.AudioSourcePriority);
			val.source1.loop = false;
			val.gameObject.SetActive(value: false);
			_E000 = val;
		}

		public BetterSource BorrowSource(bool activateSource = true)
		{
			_E000.gameObject.SetActive(activateSource);
			_E000.ResetOcclusion();
			_E000.ReleaseListener = this;
			return _E000;
		}

		public void Release(BetterSource source)
		{
			_E000.ReleaseListener = null;
			if (!_E000.source1.isPlaying)
			{
				_E000.gameObject.SetActive(value: false);
			}
		}

		public void RefreshSourceSettings()
		{
		}

		public void SetSpatialize(bool val)
		{
		}

		public void ForceChangePreset(SpatialAudioSettings preset)
		{
		}
	}

	public class _E003<_E077> : BetterSource._E000, _E001 where _E077 : BetterSource
	{
		protected Stack<_E077> _E000 = new Stack<_E077>();

		private int _E001;

		private GameObject _E002;

		[CompilerGenerated]
		private AudioGroupPreset m__E003;

		[CompilerGenerated]
		private AudioMixerGroup _E004;

		public AudioGroupPreset Preset
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			protected set
			{
				_E003 = value;
			}
		}

		public float DeltaPitch => Preset.DeltaPitch;

		public AudioMixerGroup MixerGroup
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
			[CompilerGenerated]
			set
			{
				_E004 = value;
			}
		}

		public string DebugName => Preset.name;

		protected float _E005 => Preset.SpatialBlend;

		public _E003(AudioMixerGroup mixerGroup, GameObject prefab, AudioGroupPreset preset, bool spatialization = false)
		{
			MixerGroup = mixerGroup;
			Preset = preset;
			_E002 = prefab;
			for (int i = 0; i < preset.PreCachedSourcesCount; i++)
			{
				_E077 item = InstantiateNewSource(spatialization);
				_E000.Push(item);
			}
		}

		public virtual _E077 InstantiateNewSource(bool spatialization = false)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_E002);
			gameObject.name = DebugName + ++_E001;
			AudioSource component = gameObject.GetComponent<AudioSource>();
			component.rolloffMode = AudioRolloffMode.Custom;
			component.SetCustomCurve(AudioSourceCurveType.CustomRolloff, EFTHardSettings.Instance.SoundRolloff);
			component.spatialBlend = _E005;
			component.SetCustomCurve(AudioSourceCurveType.Spread, EFTHardSettings.Instance.SpreadCurve);
			component.outputAudioMixerGroup = MixerGroup;
			_E077 val = gameObject.AddComponent<_E077>();
			val.Init();
			val.SetPreset(Preset);
			SpatialAudioSettings spatialSettings = new SpatialAudioSettings(Preset);
			val.SetSpatialSettings(spatialSettings);
			val.transform.parent = Singleton<BetterAudio>.Instance.transform;
			val.SetPitch(1f);
			val.SetPriority(Preset.AudioSourcePriority);
			val.SetRolloff(Preset.DefaultMaxDistance);
			component.spatialize = spatialization;
			val.gameObject.SetActive(value: false);
			return val;
		}

		public BetterSource BorrowSource(bool activateSource = true)
		{
			BetterSource betterSource = null;
			betterSource = ((_E000.Count <= 0) ? InstantiateNewSource() : _E000.Pop());
			betterSource.ReleaseListener = this;
			betterSource.gameObject.SetActive(activateSource);
			betterSource.ResetOcclusion();
			betterSource.SetBaseVolume(1f);
			return betterSource;
		}

		public virtual void RefreshSourceSettings()
		{
		}

		public virtual void ForceChangePreset(SpatialAudioSettings preset)
		{
			RefreshSourceSettings();
			foreach (_E077 item in _E000)
			{
				BetterSource component = item.GetComponent<BetterSource>();
				if (!(component == null))
				{
					component.SetSpatialSettings(preset);
				}
			}
		}

		public virtual void SetSpatialize(bool val)
		{
		}

		public virtual void Release(BetterSource source)
		{
			if (source == null)
			{
				Debug.Log(_ED3E._E000(35444));
				return;
			}
			if (!Singleton<BetterAudio>.Instantiated)
			{
				UnityEngine.Object.Destroy(source.gameObject);
				return;
			}
			source.ReleaseListener = null;
			source.Clear(_E005);
			source.transform.parent = Singleton<BetterAudio>.Instance.transform;
			source.gameObject.SetActive(value: false);
			_E000.Push(source as _E077);
		}
	}

	private sealed class _E004<_E077> : _E003<_E077> where _E077 : BetterSource
	{
		private new readonly List<SpatialAudioSource> m__E000 = new List<SpatialAudioSource>();

		public _E004(AudioMixerGroup mixerGroup, GameObject prefab, AudioGroupPreset preset)
			: base(mixerGroup, prefab, preset, spatialization: true)
		{
		}

		public override _E077 InstantiateNewSource(bool spatialization = false)
		{
			_E077 val = base.InstantiateNewSource(spatialization: true);
			SpatialAudioSource[] componentsInChildren = val.GetComponentsInChildren<SpatialAudioSource>();
			foreach (SpatialAudioSource spatialAudioSource in componentsInChildren)
			{
				AudioSource component = spatialAudioSource.GetComponent<AudioSource>();
				component.dopplerLevel = 0f;
				_E000(spatialAudioSource, component);
				this._E000.Add(spatialAudioSource);
			}
			return val;
		}

		public override void RefreshSourceSettings()
		{
			base.RefreshSourceSettings();
			foreach (SpatialAudioSource item in this._E000)
			{
				if (!(item == null))
				{
					_E000(item, item.GetComponent<AudioSource>());
				}
			}
		}

		public override void ForceChangePreset(SpatialAudioSettings preset)
		{
			RefreshSourceSettings();
			foreach (SpatialAudioSource item in this._E000)
			{
				BetterSource component = item.GetComponent<BetterSource>();
				if (!(component == null))
				{
					component.SetSpatialSettings(preset);
				}
			}
		}

		public override void SetSpatialize(bool val)
		{
			foreach (SpatialAudioSource item in this._E000)
			{
				if (!(item == null))
				{
					item.GetComponent<AudioSource>().spatialize = val;
				}
			}
		}

		private new void _E000(SpatialAudioSource spatialAudioSource, AudioSource audioSource)
		{
			audioSource.spatializePostEffects = base.Preset.SteamSpatializePostEffects;
			audioSource.spatialBlend = base.Preset.SpatialBlend;
			spatialAudioSource.SetParameters(base.Preset);
		}
	}

	public static _EA53 LowMute = new _EA53
	{
		CompressorVolume = -3f,
		DryVolume = -50f,
		AmbientVolume = -2f,
		CompressorAttack = 0f,
		CompressorGain = 0f,
		CompressorRelease = 0f,
		CompressorTreshold = 0f,
		Distortion = 0f,
		Resonance = 0f,
		CutoffFreq = 20,
		LowpassFreq = 21000
	};

	public static _EA53 StrongMute = new _EA53
	{
		CompressorVolume = -5f,
		DryVolume = -50f,
		AmbientVolume = -5f,
		CompressorAttack = 0f,
		CompressorGain = 0f,
		CompressorRelease = 0f,
		CompressorTreshold = 0f,
		Distortion = 0f,
		Resonance = 0f,
		CutoffFreq = 20,
		LowpassFreq = 18000
	};

	[CompilerGenerated]
	private Action m__E000 = delegate
	{
	};

	public const float TRANSITION_TIME = 0.25f;

	public const float SOUND_SPEED = 340f;

	[CompilerGenerated]
	private AudioListener m__E001;

	public WeaponSounds MiscCollisionSounds;

	public ItemDropSounds ItemDropSounds;

	private _E74F._E004 m__E002 = new _E74F._E004
	{
		Value = 0f
	};

	[CompilerGenerated]
	private Action m__E003;

	[CompilerGenerated]
	private Action<Player> m__E004 = delegate
	{
	};

	private _E751 m__E005;

	private float m__E006;

	private GameObject m__E007;

	private GameObject m__E008;

	private GameObject m__E009;

	private GameObject m__E00A;

	private Coroutine m__E00B;

	private bool m__E00C;

	private Action _E00D;

	private readonly _E3F7 _E00E = new _E3F7();

	private int _E00F;

	public _E001[] SourceGroups;

	public AudioMixer Master;

	public AudioMixerSnapshot[] Snapshots;

	public AudioMixerGroup MasterMixerGroup;

	public AudioMixerGroup GunshotOccludedMixerGroup;

	public AudioMixerGroup SimpleOccludedMixerGroup;

	public AudioMixerGroup MutedGroup;

	public AudioMixerGroup UpperOccluded;

	public AudioMixerGroup LowerOccluded;

	public AudioMixerGroup GunshotMixerGroup;

	public AudioMixerGroup VeryStandartMixerGroup;

	public AudioMixerGroup SelfSpeechReverb;

	public AudioMixerGroup OutEnvironment;

	public AudioMixerGroup VoipMixer;

	private float _E010;

	private float _E011;

	private float _E012;

	public int OcclusionMask;

	public int OcclusionHighPolyMask;

	private CancellationTokenSource _E013;

	private readonly List<_E3B9> _E014 = new List<_E3B9>(15);

	private readonly _E3F1<_E3BD> _E015 = new _E3F1<_E3BD>(15, 3);

	private Action _E016;

	private RaycastHit _E017;

	[CompilerGenerated]
	private bool _E018 = true;

	[CompilerGenerated]
	private Player _E019;

	public Dictionary<string, List<_E000>> Gags = new Dictionary<string, List<_E000>>();

	private _E3F1<_E000> _E01A = new _E3F1<_E000>(10, 5);

	private static Tweener _E01B = null;

	public AudioListener AudioListener
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public float ProtagonistHearing => this.m__E002;

	private int _E01C
	{
		set
		{
			if (_E00F != value)
			{
				_E00F = value;
				Master.SetFloat(_ED3E._E000(34977), EFTHardSettings.Instance.UPPER_LOWPASS.Evaluate(_E00F));
				Master.SetFloat(_ED3E._E000(35030), EFTHardSettings.Instance.LOWER_LOWPASS.Evaluate(_E00F));
			}
		}
	}

	public bool UseNewOcclusionSystem
	{
		[CompilerGenerated]
		get
		{
			return _E018;
		}
		[CompilerGenerated]
		private set
		{
			_E018 = value;
		}
	}

	public Player ListenerPlayer
	{
		[CompilerGenerated]
		get
		{
			return _E019;
		}
		[CompilerGenerated]
		private set
		{
			_E019 = value;
		}
	}

	public event Action AudioControllerInitialized
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action ProtagonistHearingChanged
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Player> ListenerSpawned
	{
		[CompilerGenerated]
		add
		{
			Action<Player> action = this.m__E004;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player> action = this.m__E004;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SetCompressor(_EA53 template)
	{
		bool flag = template != null;
		Master.SetFloat(_ED3E._E000(36808), flag ? template.CompressorVolume : (-80f));
		Master.SetFloat(_ED3E._E000(36803), flag ? template.DryVolume : 0f);
		Master.SetFloat(_ED3E._E000(36851), flag ? template.DryVolume : 0f);
		Master.SetFloat(_ED3E._E000(34845), flag ? template.AmbientVolume : 0f);
		Master.SetFloat(_ED3E._E000(34835), flag ? (template.AmbientVolume - 15f) : (-5f));
		this.m__E006 = (flag ? template.DryVolume : 0f);
		Master.SetFloat(_ED3E._E000(34819), this.m__E006);
		if (flag)
		{
			Master.SetFloat(_ED3E._E000(34870), template.CompressorAttack);
			Master.SetFloat(_ED3E._E000(34855), template.CompressorGain);
			Master.SetFloat(_ED3E._E000(34896), template.CompressorRelease);
			Master.SetFloat(_ED3E._E000(34882), template.CompressorTreshold);
			Master.SetFloat(_ED3E._E000(34925), template.Distortion);
			Master.SetFloat(_ED3E._E000(34970), template.Resonance);
			Master.SetFloat(_ED3E._E000(34950), template.CutoffFreq);
			Master.SetFloat(_ED3E._E000(34999), template.LowpassFreq);
		}
	}

	public override void Awake()
	{
	}

	public void Update()
	{
		_E00E.Update();
		for (int i = 0; i < _E014.Count; i++)
		{
			_E014[i].Update();
		}
		if (ListenerPlayer != null && Time.time > _E012)
		{
			_E01C = (int)ListenerPlayer.MovementContext.Pitch;
			_E012 = Time.time + 0.3f;
		}
	}

	public override void OnDestroy()
	{
		if (SourceGroups != null)
		{
			_E001[] sourceGroups = SourceGroups;
			for (int i = 0; i < sourceGroups.Length; i++)
			{
				sourceGroups[i]?.SetSpatialize(val: false);
			}
		}
		if (_E8A8.Exist)
		{
			_E8A8.Instance.OnCameraChanged -= _E005;
		}
		Singleton<_E482>.Release(Singleton<_E482>.Instance);
		_E013?.Cancel();
		MiscCollisionSounds = null;
		ItemDropSounds = null;
		if (this.m__E00B != null)
		{
			StopCoroutine(this.m__E00B);
			this.m__E00B = null;
		}
		_E00D = null;
		base.OnDestroy();
	}

	public async void Preload()
	{
		await PreloadCoroutine();
	}

	public async Task PreloadCoroutine()
	{
		_E013 = new CancellationTokenSource();
		ResourceRequest resourceRequest = Resources.LoadAsync<AudioMixer>(_ED3E._E000(35435));
		await resourceRequest.Await();
		Master = (AudioMixer)resourceRequest.asset;
		Snapshots = (from x in Enum.GetNames(typeof(EnvironmentType))
			select Master.FindSnapshot(x)).ToArray();
		_E000();
		if (_E013.IsCancellationRequested)
		{
			return;
		}
		_E8A8.Instance.OnCameraChanged += _E005;
		ResourceRequest resourceRequest2 = Resources.LoadAsync<GameObject>(_ED3E._E000(35477));
		await resourceRequest2.Await();
		this.m__E007 = (GameObject)resourceRequest2.asset;
		if (_E013.IsCancellationRequested)
		{
			return;
		}
		resourceRequest2 = Resources.LoadAsync<GameObject>(_ED3E._E000(35517));
		await resourceRequest2.Await();
		this.m__E008 = (GameObject)resourceRequest2.asset;
		if (_E013.IsCancellationRequested)
		{
			return;
		}
		resourceRequest2 = Resources.LoadAsync<GameObject>(_ED3E._E000(35548));
		await resourceRequest2.Await();
		this.m__E009 = (GameObject)resourceRequest2.asset;
		if (!_E013.IsCancellationRequested)
		{
			resourceRequest2 = Resources.LoadAsync<GameObject>(_ED3E._E000(35576));
			await resourceRequest2.Await();
			this.m__E00A = (GameObject)resourceRequest2.asset;
			if (!_E013.IsCancellationRequested)
			{
				_E013 = null;
				OcclusionMask = LayerMask.GetMask(_ED3E._E000(60734), _ED3E._E000(60795));
				OcclusionHighPolyMask = LayerMask.GetMask(_ED3E._E000(25428), _ED3E._E000(60852));
				_E001();
				_E002();
				_E003();
			}
		}
	}

	private void _E000()
	{
		if (!Singleton<_E482>.Instantiated)
		{
			Singleton<_E482>.Create(new _E482());
		}
		this.m__E000();
	}

	private void _E001()
	{
		Master.SetFloat(_ED3E._E000(34819), 0f);
		Master.SetFloat(_ED3E._E000(35019), (!_E7A3.InHideOut) ? (-80) : 0);
		Master.SetFloat(_ED3E._E000(35070), -80f);
		Master.SetFloat(_ED3E._E000(35056), -80f);
	}

	private void _E002()
	{
		MasterMixerGroup = FindMixerGroup(_ED3E._E000(35050));
		GunshotOccludedMixerGroup = FindMixerGroup(_ED3E._E000(35041));
		GunshotMixerGroup = FindMixerGroup(_ED3E._E000(35095));
		MutedGroup = FindMixerGroup(_ED3E._E000(35077));
		VeryStandartMixerGroup = FindMixerGroup(_ED3E._E000(35135));
		SimpleOccludedMixerGroup = FindMixerGroup(_ED3E._E000(35112));
		UpperOccluded = FindMixerGroup(_ED3E._E000(35106));
		LowerOccluded = FindMixerGroup(_ED3E._E000(35152));
		SelfSpeechReverb = FindMixerGroup(_ED3E._E000(35142));
		OutEnvironment = FindMixerGroup(_ED3E._E000(35197));
		VoipMixer = FindMixerGroup(_ED3E._E000(35180));
	}

	private void _E003()
	{
		AudioGroupPreset preset = _E004(_ED3E._E000(35177));
		AudioGroupPreset preset2 = _E004(_ED3E._E000(35171));
		AudioGroupPreset preset3 = _E004(_ED3E._E000(35222));
		AudioGroupPreset preset4 = _E004(_ED3E._E000(35214));
		AudioGroupPreset preset5 = _E004(_ED3E._E000(35202));
		AudioGroupPreset preset6 = _E004(_ED3E._E000(35259));
		AudioGroupPreset preset7 = _E004(_ED3E._E000(35244));
		AudioGroupPreset preset8 = _E004(_ED3E._E000(35142));
		AudioGroupPreset preset9 = _E004(_ED3E._E000(35236));
		AudioGroupPreset preset10 = _E004(_ED3E._E000(35294));
		AudioGroupPreset preset11 = _E004(_ED3E._E000(35279));
		AudioGroupPreset preset12 = _E004(_ED3E._E000(35180));
		SourceGroups = new _E001[Enum.GetValues(typeof(AudioSourceGroupType)).Length];
		AudioMixerGroup veryStandartMixerGroup = VeryStandartMixerGroup;
		SourceGroups[0] = _E007<SuperSource>(GunshotMixerGroup, this.m__E009, this.m__E00A, preset5);
		SourceGroups[11] = _E007<SuperSourceDistant>(GunshotMixerGroup, this.m__E009, this.m__E00A, preset6);
		SourceGroups[1] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset9);
		SourceGroups[2] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset7);
		SourceGroups[3] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset);
		SourceGroups[4] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset4);
		SourceGroups[5] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset2);
		SourceGroups[6] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset8);
		SourceGroups[7] = _E007<SuperSourceDistant>(veryStandartMixerGroup, this.m__E009, this.m__E00A, preset3);
		SourceGroups[8] = new _E002<SimpleSource>(FindMixerGroup(_ED3E._E000(35294)), this.m__E007, preset10);
		SourceGroups[9] = new _E003<SimpleSource>(veryStandartMixerGroup, this.m__E007, preset11);
		SourceGroups[10] = _E007<SimpleSource>(veryStandartMixerGroup, this.m__E008, this.m__E007, preset12);
	}

	private static AudioGroupPreset _E004(string groupName)
	{
		AudioGroupPreset audioGroupPreset = _E3A2.Load<AudioGroupPreset>(_ED3E._E000(35266) + groupName);
		audioGroupPreset.DirectBinaural = Singleton<_E7DE>.Instance.Sound.Settings.BinauralSound;
		return audioGroupPreset;
	}

	private void _E005()
	{
		_E006();
	}

	private void _E006()
	{
		AudioListener = _E8A8.Instance.Camera.GetComponent<AudioListener>();
	}

	private _E001 _E007<_E077>(AudioMixerGroup mixerGroup, GameObject steamPrefab, GameObject origPrefab, AudioGroupPreset preset) where _E077 : BetterSource
	{
		_E7DE instance = Singleton<_E7DE>.Instance;
		if ((bool)instance.Sound.Settings.BinauralSound && !instance.Sound.Controller.BinauralSoundRestartRequired)
		{
			return new _E004<_E077>(mixerGroup, steamPrefab, preset);
		}
		return new _E003<_E077>(mixerGroup, origPrefab, preset);
	}

	public AudioMixerGroup FindMixerGroup(string groupName)
	{
		return Master.FindMatchingGroups(groupName).First();
	}

	public void EnableAllSpatialization(bool val)
	{
		if (SourceGroups == null)
		{
			return;
		}
		_E001[] sourceGroups = SourceGroups;
		foreach (_E001 obj in sourceGroups)
		{
			if (obj == null || obj.Preset == null)
			{
				break;
			}
			obj.Preset.SteamSpatialize = val;
			obj.RefreshSourceSettings();
		}
	}

	public void EnableAllBinaural(bool val)
	{
		_E7DE instance = Singleton<_E7DE>.Instance;
		if (SourceGroups == null || instance.Sound.Controller.BinauralSoundRestartRequired)
		{
			return;
		}
		_E001[] sourceGroups = SourceGroups;
		foreach (_E001 obj in sourceGroups)
		{
			if (obj == null || obj.Preset == null)
			{
				break;
			}
			if (!obj.Preset.DirectBinaural || val)
			{
				obj.Preset.DirectBinaural = val;
				obj.RefreshSourceSettings();
			}
		}
	}

	public void LoadSoundBundles()
	{
		MiscCollisionSounds = Singleton<_ED0A>.Instance.InstantiateAsset<WeaponSounds>(_ED3E._E000(35307));
		ItemDropSounds = Singleton<_ED0A>.Instance.InstantiateAsset<ItemDropSounds>(_ED3E._E000(35379));
	}

	public void SetProtagonist(Player player)
	{
		ListenerPlayer = player;
		this.m__E002 = player.Skills.PerceptionHearing;
		this.m__E005 = player.Skills.Perception;
		_E016 = this.m__E005.SkillLevelChanged.Subscribe(InvokeProtagonistAction);
		this.m__E004?.Invoke(player);
	}

	public void UnsubscribeProtagonist()
	{
		if (this.m__E005 != null)
		{
			_E016();
			this.m__E005 = null;
			this.m__E003 = null;
			ListenerPlayer = null;
		}
	}

	public void InvokeProtagonistAction()
	{
		this.m__E003?.Invoke();
	}

	[CanBeNull]
	public _E3BD BorrowWeaponAudioQueue(AudioSourceGroupType groupType)
	{
		_E003<SuperSource> obj = SourceGroups[(int)groupType] as _E003<SuperSource>;
		BetterSource betterSource = obj.BorrowSource();
		if (betterSource == null)
		{
			return null;
		}
		BetterSource betterSource2 = obj.BorrowSource();
		if (betterSource2 == null)
		{
			betterSource.Release();
			return null;
		}
		_E3BD @object = _E015.GetObject();
		@object.Initialize(betterSource as SuperSource, betterSource2 as SuperSource);
		_E014.Add(@object);
		return @object;
	}

	public void ReleaseQueue(_E3B9 queue)
	{
		BetterSource[] audioSources = queue.AudioSources;
		foreach (BetterSource betterSource in audioSources)
		{
			if (betterSource != null)
			{
				betterSource.Release();
			}
		}
		if (queue is _E3BD)
		{
			_E015.PutObject(queue as _E3BD);
		}
		_E014.Remove(queue);
	}

	public void ReleaseQueueDelayed(_E3B9 queue, float delay)
	{
		if (delay <= 0f)
		{
			ReleaseQueue(queue);
		}
		else
		{
			StartCoroutine(_E008(queue, delay));
		}
	}

	private IEnumerator _E008(_E3B9 queue, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (queue != null)
		{
			ReleaseQueue(queue);
		}
	}

	public void TransitToEnvironment(EnvironmentType environment)
	{
		Snapshots[(environment != 0) ? 1 : 0].TransitionTo(0.25f);
	}

	public void RegisterVolume(BetterPropagationVolume volume)
	{
		Singleton<_E482>.Instance?.RegisterVolume(volume);
	}

	public void RemoveVolume(BetterPropagationVolume volume)
	{
		Singleton<_E482>.Instance?.RemoveVolume(volume);
	}

	public void RegisterGroup(BetterPropagationGroups group)
	{
		Singleton<_E482>.Instance?.RegisterGroup(group);
	}

	[ContextMenu("погнали ёбанарот")]
	public void RegisterAllVolumes()
	{
		BetterPropagationVolume[] array = UnityEngine.Object.FindObjectsOfType<BetterPropagationVolume>();
		foreach (BetterPropagationVolume volume in array)
		{
			Singleton<_E482>.Instance?.RegisterVolume(volume);
		}
	}

	public List<BetterPropagationVolume> GetVolumesByPosition(Vector3 position)
	{
		return Singleton<_E482>.Instance?.GetVolumesByPosition(position);
	}

	public BetterPropagationVolume GetVolumeByPosition(Vector3 position)
	{
		return Singleton<_E482>.Instance?.GetVolumeByPosition(position);
	}

	public List<BetterPropagationVolume> GetAdjustedAndIsolatedVolumes(Vector3 position, List<BetterPropagationVolume> volumesBuffer)
	{
		return Singleton<_E482>.Instance?.GetAdjustedAndIsolatedVolumes(position, volumesBuffer);
	}

	public bool IsSourcesInDifferentVolume(Vector3 firstSourcePos, Vector3 secondSourcePos)
	{
		return Singleton<_E482>.Instance.IsPositionsInDifferentVolume(firstSourcePos, secondSourcePos);
	}

	public void StartTinnitusEffect(float time, AudioClip clip = null)
	{
		bool num = _E011 > Time.time;
		_E010 = Mathf.Max(15f, time * 2f);
		_E011 = _E010 + Time.time;
		if (!num)
		{
			StartCoroutine(_E009(clip));
		}
	}

	public void TogglePauseTinnitusEffect(bool pause)
	{
		this.m__E00C = pause;
		if (!pause && _E00D != null)
		{
			_E00D();
			_E00D = null;
		}
	}

	private IEnumerator _E009(AudioClip clip)
	{
		BetterSource source = GetSource(AudioSourceGroupType.Speech);
		AudioMixerGroup outputAudioMixerGroup = source.source1.outputAudioMixerGroup;
		source.source1.outputAudioMixerGroup = MasterMixerGroup;
		source.source1.spatialBlend = 0f;
		source.source1.clip = clip;
		source.source1.loop = true;
		source.source1.Play();
		while (Time.time < _E011)
		{
			float num = 1f - (_E011 - Time.time) / _E010;
			float value = EFTHardSettings.Instance.TinnitusLowpas.Evaluate(3f * num);
			float value2 = this.m__E006 + EFTHardSettings.Instance.MainChannelLevel.Evaluate(3f * num);
			source.source1.volume = EFTHardSettings.Instance.TinnitusSound.Evaluate(num);
			Master.SetFloat(_ED3E._E000(34819), value2);
			Master.SetFloat(_ED3E._E000(35070), value);
			Master.SetFloat(_ED3E._E000(35056), value);
			Master.SetFloat(_ED3E._E000(35019), value2);
			yield return null;
		}
		source.source1.loop = false;
		Master.SetFloat(_ED3E._E000(34819), this.m__E006);
		Master.SetFloat(_ED3E._E000(35019), 0f);
		Master.SetFloat(_ED3E._E000(35070), -80f);
		Master.SetFloat(_ED3E._E000(35056), -80f);
		source.source1.outputAudioMixerGroup = outputAudioMixerGroup;
		source.source1.Stop();
		source.Release();
		yield return null;
	}

	public AudioMixerGroup GetOcclusionGroupSimple(Vector3 soundPosition, float distance = 0f)
	{
		if (_E8A8.Instance.Camera == null)
		{
			return SimpleOccludedMixerGroup;
		}
		if (!_E486.SimpleOcclusionTest(soundPosition, _E8A8.Instance.Camera.transform.position, (distance > 0f) ? distance : _E8A8.Instance.Distance(soundPosition), OcclusionMask))
		{
			return VeryStandartMixerGroup;
		}
		return SimpleOccludedMixerGroup;
	}

	public AudioMixerGroup GetOcclusionGroupSimple(Vector3 soundPosition, ref float volume, float distance = 0f)
	{
		if (UseNewOcclusionSystem)
		{
			return VeryStandartMixerGroup;
		}
		if (_E8A8.Instance.Camera == null)
		{
			return SimpleOccludedMixerGroup;
		}
		if (!_E486.SimpleOcclusionTest(soundPosition, _E8A8.Instance.Camera.transform.position, (distance > 0f) ? distance : _E8A8.Instance.Distance(soundPosition), OcclusionMask))
		{
			return VeryStandartMixerGroup;
		}
		if ((Singleton<_E482>.Instance?.GetIsolatedVolumes()).Count < 1)
		{
			return SimpleOccludedMixerGroup;
		}
		_EC11 relation = BetterPropagationVolume.GetRelation(ListenerPlayer, null, _E8A8.Instance.Camera.transform.position, soundPosition);
		if (relation.HasSelfFlag)
		{
			return SimpleOccludedMixerGroup;
		}
		if (relation.HasIsolatedFlag)
		{
			if (relation.HasConnectedFlag || relation.HasVerticalFlag)
			{
				volume = relation.Audibility;
				return SimpleOccludedMixerGroup;
			}
			volume = 0f;
			return MutedGroup;
		}
		return SimpleOccludedMixerGroup;
	}

	public void PlayNonspatial(AudioClip clip, AudioSourceGroupType sourceGroup, float panStereo = 0f, float volume = 1f)
	{
		_E001 obj = SourceGroups[(int)sourceGroup];
		BetterSource betterSource = obj.BorrowSource();
		if (!(betterSource == null))
		{
			if (obj.DeltaPitch > 0f)
			{
				betterSource.SetPitch(1f + UnityEngine.Random.Range(0f - obj.DeltaPitch, obj.DeltaPitch));
			}
			if (_E8A8.Instance.Camera != null)
			{
				betterSource.transform.position = _E8A8.Instance.Camera.transform.position;
			}
			betterSource.SetRolloff(obj.Preset.DefaultMaxDistance);
			betterSource.source1.panStereo = panStereo;
			betterSource.Play(clip, null, 1f, volume, forceStereo: true);
			float time = Time.time;
			_E00E.Add(time, time + clip.length, betterSource);
		}
	}

	public BetterSource PlayAtPoint(Vector3 position, AudioClip clip, float distance, AudioSourceGroupType sourceGroup, int rolloff, float volume = 1f, EOcclusionTest occlusionTest = EOcclusionTest.None, AudioMixerGroup forceMixerGroup = null, bool forceStereo = false)
	{
		if (distance > (float)rolloff)
		{
			return null;
		}
		_E001 obj = SourceGroups[(int)sourceGroup];
		BetterSource betterSource = obj.BorrowSource();
		if (betterSource == null)
		{
			return null;
		}
		if (obj.DeltaPitch > 0f)
		{
			betterSource.SetPitch(1f + UnityEngine.Random.Range(0f - obj.DeltaPitch, obj.DeltaPitch));
		}
		betterSource.SetRolloff(rolloff);
		betterSource.transform.position = position;
		betterSource.SetPriority(Mathf.Clamp((int)distance * 2, 64, 128));
		betterSource.ResetOcclusion();
		float volume2 = 1f;
		AudioMixerGroup mixerGroup = ((forceMixerGroup != null) ? forceMixerGroup : (occlusionTest switch
		{
			EOcclusionTest.Regular => _E487.VolumeDependentOcclusion(position, ref volume2, distance), 
			EOcclusionTest.None => VeryStandartMixerGroup, 
			_ => GetOcclusionGroupSimple(position, ref volume2, distance), 
		}));
		if (occlusionTest != 0 && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
		{
			MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(betterSource, occlusionTest);
		}
		if (volume2 <= 0f)
		{
			betterSource.Release();
			return betterSource;
		}
		betterSource.SetMixerGroup(mixerGroup);
		float volume3 = volume * volume2;
		betterSource.Play(clip, null, 1f, volume3, forceStereo);
		float time = Time.time;
		_E00E.Add(time, time + clip.length, betterSource);
		return betterSource;
	}

	public BetterSource PlayAtPoint(Vector3 position, SoundBank bank, int outputGroup, float distance, float volume = 1f, float bankBlendValue = -1f, EnvironmentType env = EnvironmentType.Outdoor, EOcclusionTest occlusionTest = EOcclusionTest.None, Player sourcePlayer = null)
	{
		if (distance > bank.Rolloff)
		{
			return null;
		}
		_E001 obj = SourceGroups[outputGroup];
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		float num = bank.PickClips((bankBlendValue < 0f) ? distance : bankBlendValue, ref clip, ref clip2, ref proportions, env);
		BetterSource betterSource = null;
		if (num > 0f)
		{
			betterSource = obj.BorrowSource();
			if (betterSource == null)
			{
				return null;
			}
			betterSource.Position = position;
			betterSource.SetRolloff(bank.Rolloff);
			betterSource.SetPriority(Mathf.Clamp((int)distance * 2, 64, 128));
			betterSource.ResetOcclusion();
			if (obj.DeltaPitch > 0f)
			{
				betterSource.SetPitch(1f + UnityEngine.Random.Range(0f - obj.DeltaPitch, obj.DeltaPitch));
			}
			if (bank.IgnoreOcclusion)
			{
				occlusionTest = EOcclusionTest.None;
			}
			AudioMixerGroup mixerGroup = occlusionTest switch
			{
				EOcclusionTest.Regular => _E487.VolumeDependentOcclusion(position, ref volume, distance), 
				EOcclusionTest.None => VeryStandartMixerGroup, 
				_ => sourcePlayer ? _E487.VolumeDependentOcclusion(position, ref volume, distance, sourcePlayer, sourcePlayer.Environment) : GetOcclusionGroupSimple(position, ref volume, distance), 
			};
			if (volume <= 0f)
			{
				betterSource.Release();
				return betterSource;
			}
			if (!bank.IgnoreOcclusion && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(betterSource, occlusionTest);
			}
			betterSource.SetMixerGroup(mixerGroup);
			float volume2 = volume * bank.RandomVolume;
			betterSource.Play(clip, clip2, proportions, volume2);
			float time = Time.time;
			_E00E.Add(time, time + num, betterSource);
		}
		return betterSource;
	}

	public void PlayAtPoint(Vector3 position, SoundBank bank, float distance, float volume = 1f, float bankBlendValue = -1f, EnvironmentType env = EnvironmentType.Outdoor, EOcclusionTest occlusionTest = EOcclusionTest.None)
	{
		PlayAtPoint(position, bank, (int)bank.SourceType, distance, volume, bankBlendValue, env, occlusionTest);
	}

	public void PlayAtPointDistant(Vector3 position, SoundBank bank, float distance, float volume = 1f, float spatialBlend = 1f, EnvironmentType env = EnvironmentType.Outdoor, EOcclusionTest occlusionTest = EOcclusionTest.None, AudioMixerGroup forceGroup = null)
	{
		if (distance > bank.Rolloff)
		{
			return;
		}
		_E001 obj = SourceGroups[7];
		AudioClip clip = null;
		AudioClip clip2 = null;
		float proportions = 1f;
		float num = bank.PickClips(distance, ref clip, ref clip2, ref proportions, env);
		if (num <= 0f)
		{
			return;
		}
		SuperSourceDistant superSourceDistant = (SuperSourceDistant)obj.BorrowSource();
		if (!(superSourceDistant == null))
		{
			superSourceDistant.ResetOcclusion();
			AudioMixerGroup mixerGroup = ((forceGroup != null) ? forceGroup : ((occlusionTest == EOcclusionTest.None) ? SourceGroups[(int)bank.SourceType].MixerGroup : GetOcclusionGroupSimple(position, ref volume, distance)));
			if (occlusionTest != 0 && MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ProcessSourceOcclusion(superSourceDistant, occlusionTest);
			}
			if (volume <= 0f)
			{
				superSourceDistant.Release();
				return;
			}
			superSourceDistant.SetMixerGroup(mixerGroup);
			superSourceDistant.SetRolloff(bank.Rolloff);
			superSourceDistant.transform.position = position;
			float num2 = (superSourceDistant.Delay = distance / 340f);
			superSourceDistant.SpatialBlend = spatialBlend;
			float volume2 = volume * bank.RandomVolume;
			superSourceDistant.Play(clip, clip2, proportions, volume2);
			float time = Time.time;
			_E00E.Add(time, time + num + num2, superSourceDistant);
		}
	}

	public void LimitedPlay(Vector3 position, SoundBank bank, float distance, Vector3 gagRadius, float chokeTime, float volume = 1f, float bankBlendValue = -1f, EnvironmentType env = EnvironmentType.Outdoor, EOcclusionTest occlusionTest = EOcclusionTest.None, string key = "")
	{
		if (distance > bank.Rolloff)
		{
			return;
		}
		key = (string.IsNullOrEmpty(key) ? bank.name : key);
		if (Gags.ContainsKey(key))
		{
			List<_E000> list = Gags[key];
			for (int num = list.Count - 1; num > -1; num--)
			{
				_E000 item = list[num];
				if (!list[num].IsActive)
				{
					_E01A.PutObject(item);
					list.RemoveAt(num);
				}
				else if (list[num].IsChoked(position, _E8A8.Instance.Camera.transform.position, distance))
				{
					return;
				}
			}
		}
		else
		{
			Gags.Add(key, new List<_E000>(10));
		}
		_E000 @object = _E01A.GetObject();
		@object.EndTime = Time.time + bank.ClipLength * chokeTime;
		@object.Radius = gagRadius;
		@object.Position = position;
		@object.Rolloff = bank.Rolloff;
		Gags[key].Add(@object);
		PlayAtPoint(position, bank, distance, volume, bankBlendValue, env, occlusionTest);
	}

	public void PlayDropItem(BaseBallistic.ESurfaceSound surfaceSound, EItemDropSoundType dropSoundType, Vector3 position, float energy)
	{
		SoundBank soundBank = ItemDropSounds.GetSoundBank(surfaceSound, dropSoundType);
		if (soundBank != null)
		{
			float volume = (float)Math.Round(ItemDropSounds.EnergyToVolumeCurve.Evaluate(energy), 2);
			float distance = _E8A8.Instance.Distance(position);
			PlayAtPoint(position, soundBank, distance, volume, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		}
	}

	public void PlayDropItem(SoundBank bank, Vector3 position, float energy)
	{
		if (bank != null)
		{
			float volume = (float)Math.Round(ItemDropSounds.EnergyToVolumeCurve.Evaluate(energy), 2);
			float distance = _E8A8.Instance.Distance(position);
			PlayAtPoint(position, bank, distance, volume, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		}
	}

	public void PrecacheGag(string key)
	{
		if (!Gags.ContainsKey(key))
		{
			Gags.Add(key, new List<_E000>(10));
		}
	}

	private void OnDrawGizmos()
	{
		foreach (KeyValuePair<string, List<_E000>> gag in Gags)
		{
			for (int i = 0; i < gag.Value.Count; i++)
			{
				gag.Value[i].Display();
			}
		}
	}

	public BetterSource GetSource(SoundBank bank, bool activateSource = true)
	{
		return GetSource(bank.SourceType, activateSource);
	}

	public void AddToAudioSourceQueue(_E3F7._E000 queueItem, float endTime)
	{
		_E00E.Add(Time.time, endTime, queueItem);
	}

	[CanBeNull]
	public BetterSource GetSource(AudioSourceGroupType sourceType, bool activateSource = true)
	{
		return SourceGroups[(int)sourceType].BorrowSource(activateSource);
	}

	public void TweenAmbientVolume(float endValDb, float seconds)
	{
		_E01B?.Kill();
		_E01B = DOTween.To(delegate
		{
			Master.GetFloat(_ED3E._E000(35019), out var value);
			return value;
		}, delegate(float x)
		{
			Master.SetFloat(_ED3E._E000(35019), x);
		}, endValDb, seconds);
	}

	public void ForceSetAmbientVolume(float valDb)
	{
		_E01B?.Kill();
		Master.SetFloat(_ED3E._E000(35019), valDb);
	}

	[CompilerGenerated]
	private AudioMixerSnapshot _E00A(string x)
	{
		return Master.FindSnapshot(x);
	}

	[CompilerGenerated]
	private float _E00B()
	{
		Master.GetFloat(_ED3E._E000(35019), out var value);
		return value;
	}

	[CompilerGenerated]
	private void _E00C(float x)
	{
		Master.SetFloat(_ED3E._E000(35019), x);
	}
}
