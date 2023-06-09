using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using DeferredDecals;
using EFT;
using EFT.Ballistics;
using EFT.EnvironmentEffect;
using EFT.Hideout.ShootingRange;
using EFT.Particles;
using UnityEngine;

namespace Systems.Effects;

public class Effects : MonoBehaviour
{
	[Serializable]
	public class EmissionEffect
	{
		public string Key;

		public GrenadeEmission Instance;

		private List<GrenadeEmission> _cache;

		public GrenadeEmission GetEffect()
		{
			if (Instance == null)
			{
				return null;
			}
			if (_cache.Count < 1)
			{
				InstantiateNewEffect();
			}
			GrenadeEmission grenadeEmission = _cache[0];
			_cache.RemoveAt(0);
			grenadeEmission.gameObject.SetActive(value: true);
			return grenadeEmission;
		}

		public void Dispose(GrenadeEmission e)
		{
			_cache.Add(e);
			e.Clear();
			e.gameObject.SetActive(value: false);
		}

		public void InstantiateNewEffect()
		{
			if (_cache == null)
			{
				_cache = new List<GrenadeEmission>(3);
			}
			GrenadeEmission grenadeEmission = UnityEngine.Object.Instantiate(Instance);
			grenadeEmission.transform.parent = Singleton<Effects>.Instance.transform;
			grenadeEmission.gameObject.SetActive(value: false);
			grenadeEmission.PoolName = Key;
			_cache.Add(grenadeEmission);
		}
	}

	public struct _E000
	{
		public Effect Effect;

		public Vector3 Position;

		public Vector3 Normal;

		public BallisticCollider HitCollider;

		public bool WithDecal;

		public float Volume;

		public bool IsKnife;

		public bool IsHitPointVisible;

		public bool IsGrenade;

		public EPointOfView Pov;
	}

	[Serializable]
	public class Effect
	{
		[Serializable]
		public class ParticleSys
		{
			public enum Type
			{
				Forward,
				Cone,
				Hemisphere,
				Circle,
				ConeNormalized,
				Cone60
			}

			public Emitter Particle;

			public Vector2 Distance;

			public Type RandomType;

			public int MinCount;

			public int RandomCountRange;

			public bool UseRandomScale;

			public Vector3 RandomScale;

			public void Emit(Vector3 position, Vector3 normal, float distance)
			{
				if (distance < Distance.x || distance > Distance.y)
				{
					return;
				}
				switch (RandomType)
				{
				case Type.Forward:
				{
					int num2 = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int j = 0; j < num2; j++)
					{
						Vector3 vector2 = normal;
						vector2 = (UseRandomScale ? Vector3.Scale(vector2, RandomScale) : vector2);
						Particle.Emit(position, vector2);
					}
					break;
				}
				case Type.Cone:
				{
					int num5 = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int m = 0; m < num5; m++)
					{
						Vector3 vector5 = normal + _E8EE.VectorNormalized();
						vector5 = (UseRandomScale ? Vector3.Scale(vector5, RandomScale) : vector5);
						Particle.Emit(position, vector5);
					}
					break;
				}
				case Type.ConeNormalized:
				{
					int num4 = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int l = 0; l < num4; l++)
					{
						Vector3 vector4 = Vector3.Normalize(normal + _E8EE.VectorHemisphere());
						vector4 = (UseRandomScale ? Vector3.Scale(vector4, RandomScale) : vector4);
						Particle.Emit(position, vector4);
					}
					break;
				}
				case Type.Hemisphere:
				{
					int num3 = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int k = 0; k < num3; k++)
					{
						Vector3 vector3 = _E8EE.VectorHemisphere();
						vector3 = (UseRandomScale ? Vector3.Scale(vector3, RandomScale) : vector3);
						Particle.Emit(position, vector3);
					}
					break;
				}
				case Type.Circle:
				{
					int num6 = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int n = 0; n < num6; n++)
					{
						Vector3 vector6 = _E8EE.VectorCircle();
						vector6 = (UseRandomScale ? Vector3.Scale(vector6, RandomScale) : vector6);
						Vector3 pos = position;
						pos.y += 0.3f;
						Particle.Emit(pos, vector6);
					}
					break;
				}
				case Type.Cone60:
				{
					int num = ((RandomCountRange < 1) ? MinCount : _E8EE.Int(MinCount, RandomCountRange));
					for (int i = 0; i < num; i++)
					{
						Vector3 vector = _E8EE.Cone60();
						vector = (UseRandomScale ? Vector3.Scale(vector, RandomScale) : vector);
						Particle.Emit(position, vector);
					}
					break;
				}
				}
			}
		}

		public string Name;

		public MaterialType[] MaterialTypes;

		public BasicParticleSystemMediator BasicParticleSystemMediator;

		public ParticleSys[] Particles = new ParticleSys[0];

		public SoundBank Sound;

		public SoundBank SoundFP;

		public DecalSystem Decal;

		public bool Flash;

		public int FlareID;

		public float FlashMaxDist = 50f;

		public float FlashTime = 0.07f;

		public bool Light;

		public Color LightColor = new Color(1f, 0.83f, 0.45f, 1f);

		public float LightMaxDist = 50f;

		public float LightRange = 1.5f;

		public float LightIntensity = 5f;

		public float LightTime = 0.1f;

		public float ParticlesShift;

		public bool WithShadows;

		public bool Wind;

		public float WindIntensity = 1f;

		public float WindRadius = 20f;

		public float WindTime = 1f;

		public float WindMaxDist = 200f;

		public const string CHOKE_IMPACT_KEY = "Impact";

		public const string CHOKE_GRENADE_KEY = "Grenade";

		public bool UseDeferredDecals;

		[HideInInspector]
		public DeferredDecalRenderer DeferredDecals;

		public _E43F LightPool;

		private Vector2 _impactsGagRadius = new Vector2(1f, 3f);

		public void Emit(Vector3 position, Vector3 normal, BallisticCollider hitCollider, bool withDecal = true, float volume = 1f, bool isKnife = false, bool isHitPointVisible = true, bool isGrenade = false, EPointOfView pov = EPointOfView.ThirdPerson)
		{
			if (withDecal && !isKnife && !(hitCollider is HideoutTargetBallisticCollider))
			{
				if ((!UseDeferredDecals || (UseDeferredDecals && !EFTHardSettings.Instance.DEFERRED_DECALS_ENABLED)) && Decal != null)
				{
					Decal.Add(position, normal);
				}
				if (UseDeferredDecals && DeferredDecals != null && (MaterialTypes.Length != 0 || isGrenade) && EFTHardSettings.Instance.DEFERRED_DECALS_ENABLED)
				{
					DeferredDecals.DrawDecal(position, normal, hitCollider, isGrenade);
				}
			}
			float num = _E8A8.Instance.Distance(position);
			EnvironmentManager instance = EnvironmentManager.Instance;
			if (volume > 0f)
			{
				if (pov == EPointOfView.FirstPerson && SoundFP != null)
				{
					AudioClip clip = SoundFP.PickSingleClip(0);
					Singleton<BetterAudio>.Instance.PlayNonspatial(clip, BetterAudio.AudioSourceGroupType.Nonspatial);
				}
				else if (Sound != null && instance != null)
				{
					EnvironmentType environmentByPos = instance.GetEnvironmentByPos(position);
					EOcclusionTest eOcclusionTest = EOcclusionTest.Fast;
					if (Sound.Physical)
					{
						eOcclusionTest = ((Sound.SourceType == BetterAudio.AudioSourceGroupType.Grenades || Sound.SourceType == BetterAudio.AudioSourceGroupType.Gunshots) ? EOcclusionTest.Continuous : eOcclusionTest);
						Singleton<BetterAudio>.Instance.PlayAtPointDistant(position, Sound, num, volume, Mathf.Clamp(0.2f, 0.9f, num / 30f), environmentByPos, eOcclusionTest);
					}
					else
					{
						string key = _ED3E._E000(92217);
						if (isGrenade)
						{
							eOcclusionTest = EOcclusionTest.Continuous;
							key = _ED3E._E000(92208);
						}
						Singleton<BetterAudio>.Instance.LimitedPlay(position, Sound, num, _impactsGagRadius, 0.3f, volume, -1f, environmentByPos, eOcclusionTest, key);
					}
				}
			}
			if (isHitPointVisible && pov == EPointOfView.ThirdPerson)
			{
				if (BasicParticleSystemMediator != null)
				{
					BasicParticleSystemMediator.Emit(position, Quaternion.LookRotation(normal));
				}
				for (int i = 0; i < Particles.Length; i++)
				{
					Particles[i].Emit(position + normal * ParticlesShift, normal, num);
				}
				position += normal * 0.05f;
				if (Light && num < LightMaxDist)
				{
					LightPool.Add(position, LightColor, LightRange, LightIntensity, LightTime, WithShadows);
				}
				if (Flash && num < FlashMaxDist)
				{
					FlareForEffects.Add(position, FlareID, FlashTime);
				}
			}
		}

		public void ClearDecal()
		{
			Decal.Clear();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string key;

		internal bool _003CGetEmissionEffect_003Eb__0(EmissionEffect x)
		{
			return x.Key == key;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GrenadeEmission emission;

		internal bool _003CDisposeEmissionEffect_003Eb__0(EmissionEffect x)
		{
			return x.Key == emission.PoolName;
		}
	}

	private static readonly int m__E000 = 10;

	public DeferredDecalRenderer DeferredDecals;

	public bool UseDecalPainter;

	public TextureDecalsPainter TexDecals;

	public ParticleSystem MuzzleFumeParticleSystem;

	public SimpleSparksRenderer MuzzleSparkParticleSystem;

	public ParticleSystem MuzzleHeatParticleSystem;

	public ParticleSystem MuzzleHeatHazeParticleSystem;

	public SoundBank[] AdditionalSoundEffects;

	public Effect[] EffectsArray;

	public EmissionEffect[] EmissionEffects;

	private EffectsCommutator m__E001;

	private _E43F m__E002;

	private Dictionary<MaterialType, Effect> _E003;

	private Dictionary<string, Effect> _E004;

	private List<_E000> _E005 = new List<_E000>(Effects.m__E000);

	public EffectsCommutator EffectsCommutator => this.m__E001;

	public void OnValidate()
	{
		if (Application.isPlaying)
		{
			InitDictionaryAndNames();
		}
	}

	private void Awake()
	{
		this.m__E001 = GetComponent<EffectsCommutator>();
		_E005 = new List<_E000>(Effects.m__E000);
		this.m__E002 = new _E43F();
		this.m__E002.Init();
		InitDictionaryAndNames();
		Dictionary<Emitter, List<Effect>> dictionary = new Dictionary<Emitter, List<Effect>>();
		Effect[] effectsArray = EffectsArray;
		foreach (Effect effect in effectsArray)
		{
			Effect.ParticleSys[] particles = effect.Particles;
			foreach (Effect.ParticleSys particleSys in particles)
			{
				if (!dictionary.TryGetValue(particleSys.Particle, out var value))
				{
					dictionary.Add(particleSys.Particle, value = new List<Effect>());
				}
				value.Add(effect);
			}
		}
		Singleton<BetterAudio>.Instance.PrecacheGag(_ED3E._E000(92217));
		Singleton<BetterAudio>.Instance.PrecacheGag(_ED3E._E000(92208));
	}

	private void InitDictionaryAndNames()
	{
		_E003 = new Dictionary<MaterialType, Effect>(EffectsArray.Length, _E3A5<MaterialType>.EqualityComparer);
		_E004 = new Dictionary<string, Effect>(EffectsArray.Length);
		Effect[] effectsArray = EffectsArray;
		foreach (Effect effect in effectsArray)
		{
			effect.DeferredDecals = DeferredDecals;
			effect.LightPool = this.m__E002;
			MaterialType[] materialTypes = effect.MaterialTypes;
			foreach (MaterialType key in materialTypes)
			{
				if (!_E003.ContainsKey(key))
				{
					_E003.Add(key, effect);
				}
			}
			_E004.Add(effect.Name, effect);
		}
	}

	private void OnDestroy()
	{
		this.m__E002.Destroy();
	}

	private void Update()
	{
		this.m__E002.Update();
		if (_E005.Count != 0)
		{
			int num = Mathf.Min(Effects.m__E000, _E005.Count);
			for (int i = 0; i < num; i++)
			{
				_E000 obj = _E005[i];
				obj.Effect.Emit(obj.Position, obj.Normal, obj.HitCollider, obj.WithDecal, obj.Volume, obj.IsKnife, obj.IsHitPointVisible, obj.IsGrenade, obj.Pov);
			}
			_E005.Clear();
		}
	}

	private void AddEffectEmit(Effect effect, Vector3 position, Vector3 normal, BallisticCollider hitCollider, bool withDecal = true, float volume = 1f, bool isKnife = false, bool isHitPointVisible = true, bool isGrenade = false, EPointOfView pov = EPointOfView.ThirdPerson)
	{
		_E000 obj = default(_E000);
		obj.Effect = effect;
		obj.Position = position;
		obj.Normal = normal;
		obj.HitCollider = hitCollider;
		obj.WithDecal = withDecal;
		obj.Volume = volume;
		obj.IsKnife = isKnife;
		obj.IsHitPointVisible = isHitPointVisible;
		obj.IsGrenade = isGrenade;
		obj.Pov = pov;
		_E000 item = obj;
		_E005.Add(item);
	}

	public void PlayerMeshesHit(List<_E3D2> renderers, Vector3 point, Vector3 direction)
	{
		if (UseDecalPainter && TexDecals != null)
		{
			TexDecals.DrawDecal(renderers, point, direction);
		}
	}

	public void EmitBloodOnEnvironment(Vector3 position, Vector3 normal)
	{
		if (DeferredDecals != null)
		{
			DeferredDecals.EmitBloodOnEnvironment(position, normal);
		}
	}

	public void EmitBleeding(Vector3 position, Vector3 normal)
	{
		if (DeferredDecals != null)
		{
			DeferredDecals.EmitBleeding(position, normal);
		}
	}

	public void Emit(MaterialType material, BallisticCollider hitCollider, Vector3 position, Vector3 normal, float volume = 1f, bool isKnife = false, bool isHitPointVisible = true, EPointOfView pov = EPointOfView.ThirdPerson)
	{
		if (_E003.TryGetValue(material, out var value))
		{
			AddEffectEmit(value, position, normal, hitCollider, withDecal: true, volume, isKnife, isHitPointVisible, isGrenade: false, pov);
		}
	}

	public void PrewarmEmit(MaterialType material, BallisticCollider hitCollider, Vector3 position, Vector3 normal, float volume = 1f, bool isKnife = false, bool isHitPointVisible = true, EPointOfView pov = EPointOfView.ThirdPerson)
	{
		if (_E003.TryGetValue(material, out var value))
		{
			value.Emit(position, normal, hitCollider, withDecal: true, volume, isKnife, isHitPointVisible, isGrenade: false, pov);
		}
	}

	public void TestEmit(int id, Vector3 position, Vector3 normal)
	{
		EffectsArray[id].Emit(position, normal, null);
	}

	public void Emit(string ename, Vector3 position, Vector3 normal)
	{
		if (_E004.ContainsKey(ename))
		{
			AddEffectEmit(_E004[ename], position, normal, null);
		}
	}

	public GrenadeEmission GetEmissionEffect(string key)
	{
		return EmissionEffects.FirstOrDefault((EmissionEffect x) => x.Key == key)?.GetEffect();
	}

	public void DisposeEmissionEffect(GrenadeEmission emission)
	{
		EmissionEffect emissionEffect = EmissionEffects.FirstOrDefault((EmissionEffect x) => x.Key == emission.PoolName);
		if (emissionEffect != null)
		{
			emissionEffect.Dispose(emission);
		}
		else
		{
			UnityEngine.Object.Destroy(emission.gameObject);
		}
	}

	public void EmitSoundOnly(MaterialType material, Vector3 position, EPointOfView pov = EPointOfView.ThirdPerson, float volume = 1f)
	{
		if (_E003.TryGetValue(material, out var value))
		{
			if (pov == EPointOfView.FirstPerson && value.SoundFP != null)
			{
				AudioClip clip = value.SoundFP.PickSingleClip(0);
				Singleton<BetterAudio>.Instance.PlayNonspatial(clip, BetterAudio.AudioSourceGroupType.NonspatialBypass);
			}
			else if (value.Sound != null)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(position, value.Sound, _E8A8.Instance.Distance(position), volume, -1f, EnvironmentManager.Instance.Environment, EOcclusionTest.Fast);
			}
		}
	}

	public void EmitGrenade(string ename, Vector3 position, Vector3 normal, float volume = 1f)
	{
		if (_E004.ContainsKey(ename))
		{
			AddEffectEmit(_E004[ename], position, normal, null, withDecal: true, volume, isKnife: false, isHitPointVisible: true, isGrenade: true);
		}
	}

	public void ClearEffects(MaterialType materialType)
	{
		if (_E003.TryGetValue(materialType, out var value))
		{
			value.ClearDecal();
		}
	}

	public Effect Get(MaterialType id)
	{
		if (!_E003.TryGetValue(id, out var value))
		{
			return null;
		}
		return value;
	}

	public void CacheEffects()
	{
		EmissionEffect[] emissionEffects = EmissionEffects;
		for (int i = 0; i < emissionEffects.Length; i++)
		{
			emissionEffects[i].InstantiateNewEffect();
		}
	}
}
