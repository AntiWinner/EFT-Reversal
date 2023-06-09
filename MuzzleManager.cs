using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Systems.Effects;
using Comfort.Common;
using UnityEngine;

public class MuzzleManager : BaseSystemComponent<MuzzleManager>, _E413, _E412, _E40C
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GameObject[] launcherGo;

		internal bool _E000(MuzzleEffect x)
		{
			return !launcherGo.Contains(x.gameObject);
		}
	}

	private const string m__E000 = "Muzzle Light";

	public Material JetMaterial;

	public int AtlasXCount;

	public int AtlasYCount;

	public AnimationCurve MoveCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	public AnimationCurve JetLightCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	public float ShotLength = 0.1f;

	public bool TestPlay;

	public bool TestShoot;

	public bool TestHold;

	public float TestDebugPosition;

	public float TestDelay;

	public MuzzleLight Light;

	public string MeshParentName = _ED3E._E000(88458);

	private MuzzleJet[] m__E001;

	private MuzzleSparks[] m__E002;

	private MuzzleFume[] m__E003;

	private MuzzleFume[] m__E004;

	private MuzzleSmoke[] _E005;

	private HeatEmitter[] _E006;

	private HeatHazeEmitter[] _E007;

	private Vector2 _E008;

	private float _E009;

	public Transform Hierarchy;

	private float _E00A;

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(88449));

	private float _E00C;

	private float _E00D;

	private bool _E00E;

	private bool _E00F;

	[CompilerGenerated]
	private GameObject[] _E010;

	public GameObject[] MuzzleJets
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
		[CompilerGenerated]
		private set
		{
			_E010 = value;
		}
	}

	private void Awake()
	{
		if (JetLightCurve.postWrapMode == WrapMode.ClampForever && MoveCurve.postWrapMode == WrapMode.ClampForever)
		{
			_E00C = Mathf.Max(JetLightCurve.GetDuration(), MoveCurve.GetDuration());
		}
		else
		{
			_E00C = float.MaxValue;
		}
		_E00D = ((Light != null && Light.LightIntensityCurve.postWrapMode == WrapMode.ClampForever) ? Light.LightIntensityCurve.GetDuration() : float.MaxValue);
	}

	public void ManualUpdate()
	{
		float num = 1f - (_E009 - Time.time) / ShotLength;
		if (num <= _E00C || !_E00E)
		{
			SetT(num);
			_E00E = num > _E00C;
		}
		_E001();
	}

	public void ManualLateUpdate()
	{
		float num = 1f - (_E009 - Time.time) / ShotLength;
		if (num <= _E00D || !_E00F)
		{
			_E002(num);
			_E00F = num > _E00D;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_E003();
	}

	public void SetT(float t)
	{
		if (JetMaterial != null)
		{
			JetMaterial.SetVector(_E00B, new Vector4(1f - JetLightCurve.Evaluate(t), MoveCurve.Evaluate(t), 0f, 0f));
		}
	}

	public void LauncherShot()
	{
		if (this.m__E004 != null)
		{
			for (int i = 0; i < this.m__E004.Length; i++)
			{
				this.m__E004[i].Emit(this);
			}
		}
	}

	public void Shot(bool isVisible = true, float sqrCameraDistance = 0f)
	{
		_E009 = Time.time + ShotLength;
		if (JetMaterial != null)
		{
			MuzzleJet.RandomizeMaterial(JetMaterial, _E008);
		}
		if (this.m__E002 != null && (isVisible || (!isVisible && sqrCameraDistance < 4f)))
		{
			for (int i = 0; i < this.m__E002.Length; i++)
			{
				this.m__E002[i].Emit(this);
			}
		}
		if (this.m__E003 != null && ((isVisible && sqrCameraDistance < 100f) || (!isVisible && sqrCameraDistance < 4f)))
		{
			for (int j = 0; j < this.m__E003.Length; j++)
			{
				this.m__E003[j].Emit(this);
			}
		}
		if (_E005 != null && ((isVisible && sqrCameraDistance < 100f) || (!isVisible && sqrCameraDistance < 4f)))
		{
			for (int k = 0; k < _E005.Length; k++)
			{
				_E005[k].Shot();
			}
		}
		if (_E00A > 0f && (isVisible || (!isVisible && sqrCameraDistance < 400f)))
		{
			Light._E000();
		}
		if (_E006 != null && (isVisible || (!isVisible && sqrCameraDistance < 400f)))
		{
			for (int l = 0; l < _E006.Length; l++)
			{
				_E006[l].OnShot();
			}
		}
		if (_E007 != null && (isVisible || (!isVisible && sqrCameraDistance < 400f)))
		{
			for (int m = 0; m < _E007.Length; m++)
			{
				_E007[m].OnShot(this);
			}
		}
	}

	void _E413.Emit(Vector3 position, Vector3 velocity, float time, float gravity, float drag, float lifeTime, byte emission, byte size, byte turbulence, byte frequency)
	{
		Singleton<Effects>.Instance.MuzzleSparkParticleSystem.EmitSeg(position, velocity, time, gravity, drag, lifeTime, emission, size, turbulence, frequency);
	}

	void _E412.Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
	{
		Singleton<Effects>.Instance.MuzzleFumeParticleSystem.Emit(new ParticleSystem.EmitParams
		{
			position = position,
			startLifetime = lifetime,
			velocity = velocity,
			randomSeed = _E8EE.Uint(),
			startColor = color,
			startSize = size,
			rotation = _E8EE.FloatRotation()
		}, 1);
	}

	void _E40C.Emit(Vector3 position, float rotation, Vector3 velocity, float size, float lifetime, Color32 color)
	{
		Singleton<Effects>.Instance.MuzzleHeatHazeParticleSystem.Emit(new ParticleSystem.EmitParams
		{
			position = position,
			rotation = rotation,
			velocity = velocity,
			startSize = size,
			startLifetime = lifetime,
			startColor = color
		}, 1);
	}

	public void UpdateJetsAndFumes()
	{
		Transform transform = Hierarchy.FindTransform(_ED3E._E000(88432));
		this.m__E004 = (transform ? transform.GetComponentsInChildrenActiveIgnoreFirstLevel<MuzzleFume>().ToArray() : new MuzzleFume[0]);
		GameObject[] launcherGo = this.m__E004.Select((MuzzleFume f) => f.gameObject).ToArray();
		MuzzleJets = (from x in Hierarchy.GetComponentsInChildren<MuzzleEffect>(includeInactive: true)
			where !launcherGo.Contains(x.gameObject)
			select x.gameObject).Distinct().ToArray();
		List<GameObject> list = new List<GameObject>();
		int num = 0;
		GameObject gameObject = null;
		for (int i = 0; i < MuzzleJets.Length; i++)
		{
			if (MuzzleJets[i].name == _ED3E._E000(88421))
			{
				gameObject = MuzzleJets[i];
				continue;
			}
			int num2 = _E38B.NumParents(MuzzleJets[i].transform, Hierarchy);
			if (num < num2)
			{
				list.Clear();
				list.Add(MuzzleJets[i]);
				num = num2;
			}
			else if (num == num2)
			{
				list.Add(MuzzleJets[i]);
			}
		}
		if (gameObject != null)
		{
			list.Add(gameObject);
		}
		MuzzleJets = list.ToArray();
		this.m__E003 = _E000<MuzzleFume>();
		_E005 = _E000<MuzzleSmoke>();
		this.m__E002 = _E000<MuzzleSparks>();
		this.m__E001 = _E000<MuzzleJet>();
		_E00A = this.m__E001.Sum((MuzzleJet x) => x.Chance);
		_E008 = new Vector2(1f / (float)AtlasXCount, 1f / (float)AtlasYCount);
		if (JetMaterial != null)
		{
			MuzzleJet.UpdateOrCreateMesh(this.m__E001, Hierarchy.FindTransform(MeshParentName) ?? Hierarchy, JetMaterial, _E008);
		}
		_E007 = _E000<HeatHazeEmitter>();
	}

	private _E077[] _E000<_E077>() where _E077 : MonoBehaviour
	{
		List<_E077> list = new List<_E077>();
		for (int i = 0; i < MuzzleJets.Length; i++)
		{
			_E077[] components = MuzzleJets[i].GetComponents<_E077>();
			if (components.Length != 0)
			{
				list.AddRange(components);
			}
		}
		return list.ToArray();
	}

	private void _E001()
	{
		if (this.m__E003 == null)
		{
			return;
		}
		for (int i = 0; i < this.m__E003.Length; i++)
		{
			if (this.m__E003[i] != null)
			{
				this.m__E003[i].UpdateValues();
			}
		}
	}

	public void LateUpdateMuzzleEffectsValues(Camera cam)
	{
		if (_E005 == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (deltaTime < float.Epsilon)
		{
			return;
		}
		for (int i = 0; i < _E005.Length; i++)
		{
			MuzzleSmoke muzzleSmoke = _E005[i];
			if (!(muzzleSmoke == null) && muzzleSmoke.enabled)
			{
				muzzleSmoke.LateUpdateValues(cam, deltaTime);
			}
		}
	}

	private void _E002(float t)
	{
		if (Light != null && this.m__E001 != null && this.m__E001.Length != 0 && _E00A > 0f)
		{
			Light.SetIntensity(Light.LightIntensityCurve.Evaluate(t));
		}
	}

	private void _E003()
	{
		if (_E005 == null)
		{
			return;
		}
		for (int num = _E005.Length - 1; num >= 0; num--)
		{
			MuzzleSmoke muzzleSmoke = _E005[num];
			if (muzzleSmoke != null)
			{
				muzzleSmoke.Clear();
			}
		}
	}

	internal void _E004()
	{
		Light = Hierarchy.GetComponentInChildrenActiveIgnoreFirstLevel<MuzzleLight>();
		if (!(Light != null))
		{
			Light = new GameObject(_ED3E._E000(88469), typeof(MuzzleLight)).GetComponent<MuzzleLight>();
			Transform transform = _E38B.FindTransformRecursive(Hierarchy, _ED3E._E000(64493));
			Transform transform2 = ((transform == null) ? Hierarchy : transform.transform);
			Light.transform.parent = transform2.parent;
			Light.transform.position = transform2.position;
			Light.transform.localPosition = new Vector3(Light.transform.localPosition.x - 0.05f, Light.transform.localPosition.y, Light.transform.localPosition.z);
		}
	}
}
