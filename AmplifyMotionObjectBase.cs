using System;
using System.Collections.Generic;
using AmplifyMotion;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("")]
public class AmplifyMotionObjectBase : MonoBehaviour
{
	public enum MinMaxCurveState
	{
		Scalar,
		Curve,
		TwoCurves,
		TwoScalars
	}

	internal static bool _E000 = true;

	[SerializeField]
	private bool m_applyToChildren = AmplifyMotionObjectBase._E000;

	private ObjectType m__E001;

	private Dictionary<Camera, MotionState> m__E002 = new Dictionary<Camera, MotionState>();

	private bool m__E003;

	private int m__E004;

	private Vector3 m__E005 = Vector3.zero;

	private int m__E006 = -1;

	internal bool _E007 => this.m__E003;

	internal int _E008 => this.m__E004;

	public ObjectType Type => this.m__E001;

	internal void _E000(AmplifyMotionCamera camera)
	{
		Camera component = camera.GetComponent<Camera>();
		if ((component.cullingMask & (1 << base.gameObject.layer)) != 0 && !this.m__E002.ContainsKey(component))
		{
			MotionState motionState = null;
			motionState = this.m__E001 switch
			{
				ObjectType.Solid => new _ED32(camera, this), 
				ObjectType.Skinned => new _ED31(camera, this), 
				ObjectType.Cloth => new _ED2F(camera, this), 
				ObjectType.Particle => new _ED30(camera, this), 
				_ => throw new Exception(_ED3E._E000(18376)), 
			};
			camera.RegisterObject(this);
			this.m__E002.Add(component, motionState);
		}
	}

	internal void _E001(AmplifyMotionCamera camera)
	{
		Camera component = camera.GetComponent<Camera>();
		if (this.m__E002.TryGetValue(component, out var value))
		{
			camera.UnregisterObject(this);
			if (this.m__E002.TryGetValue(component, out value))
			{
				value._E05E();
			}
			this.m__E002.Remove(component);
		}
	}

	private bool _E002()
	{
		Renderer component = GetComponent<Renderer>();
		if (AmplifyMotionEffectBase._E017(base.gameObject, autoReg: false))
		{
			if (GetComponent<ParticleSystem>() != null)
			{
				this.m__E001 = ObjectType.Particle;
				AmplifyMotionEffectBase._E014(this);
			}
			else if (component != null)
			{
				if (component.GetType() == typeof(MeshRenderer))
				{
					this.m__E001 = ObjectType.Solid;
				}
				else if (component.GetType() == typeof(SkinnedMeshRenderer))
				{
					if (GetComponent<Cloth>() != null)
					{
						this.m__E001 = ObjectType.Cloth;
					}
					else
					{
						this.m__E001 = ObjectType.Skinned;
					}
				}
				AmplifyMotionEffectBase._E014(this);
			}
		}
		return component != null;
	}

	private void OnEnable()
	{
		bool flag = _E002();
		if (flag)
		{
			if (this.m__E001 == ObjectType.Cloth)
			{
				this.m__E003 = false;
			}
			else if (this.m__E001 == ObjectType.Solid)
			{
				Rigidbody component = GetComponent<Rigidbody>();
				if (component != null && component.interpolation == RigidbodyInterpolation.None && !component.isKinematic)
				{
					this.m__E003 = true;
				}
			}
		}
		if (m_applyToChildren)
		{
			foreach (Transform item in base.gameObject.transform)
			{
				AmplifyMotionEffectBase.RegisterRecursivelyS(item.gameObject);
			}
		}
		if (!flag)
		{
			base.enabled = false;
		}
	}

	private void OnDisable()
	{
		AmplifyMotionEffectBase._E015(this);
	}

	private void _E003()
	{
		Dictionary<Camera, MotionState>.Enumerator enumerator = this.m__E002.GetEnumerator();
		while (enumerator.MoveNext())
		{
			MotionState value = enumerator.Current.Value;
			if (value.Owner.Initialized && !value.Error && !value.Initialized)
			{
				value._E05D();
			}
		}
	}

	private void Start()
	{
		if (AmplifyMotionEffectBase.Instance != null)
		{
			_E003();
		}
		this.m__E005 = base.transform.position;
	}

	private void Update()
	{
		if (AmplifyMotionEffectBase.Instance != null)
		{
			_E003();
		}
	}

	private static void _E004(Transform transform, AmplifyMotionObjectBase obj, int frame)
	{
		if (obj != null)
		{
			obj.m__E006 = frame;
		}
		foreach (Transform item in transform)
		{
			_E004(item, item.GetComponent<AmplifyMotionObjectBase>(), frame);
		}
	}

	public void ResetMotionNow()
	{
		_E004(base.transform, this, Time.frameCount);
	}

	public void ResetMotionAtFrame(int frame)
	{
		_E004(base.transform, this, frame);
	}

	private void _E005(AmplifyMotionEffectBase inst)
	{
		if (Vector3.SqrMagnitude(base.transform.position - this.m__E005) > inst.MinResetDeltaDistSqr)
		{
			_E004(base.transform, this, Time.frameCount + inst.ResetFrameDelay);
		}
	}

	internal void _E006(AmplifyMotionEffectBase inst, Camera camera, CommandBuffer updateCB, bool starting)
	{
		if (this.m__E002.TryGetValue(camera, out var value) && !value.Error)
		{
			_E005(inst);
			bool flag = this.m__E006 > 0 && Time.frameCount >= this.m__E006;
			value._E060(updateCB, starting || flag);
		}
		this.m__E005 = base.transform.position;
	}

	internal void _E007(Camera camera, CommandBuffer renderCB, float scale, Quality quality)
	{
		if (this.m__E002.TryGetValue(camera, out var value) && !value.Error)
		{
			value._E061(camera, renderCB, scale, quality);
			if (this.m__E006 > 0 && Time.frameCount >= this.m__E006)
			{
				this.m__E006 = -1;
			}
		}
	}
}
