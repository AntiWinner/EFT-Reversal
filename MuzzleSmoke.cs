using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class MuzzleSmoke : MonoBehaviour
{
	private class _E000
	{
		public bool SkipProcess;

		public Vector3 Position;

		public Vector3 Velocity;

		public Vector3 Turbulence;

		public Vector3 Direction;

		public Color32 Color;

		public float Diffusion;

		public float YUv;
	}

	public Material Material;

	public float SmokeEnd = 10f;

	public float BrakeDistance = 0.1f;

	[Space(8f)]
	public float DragValue = 0.98f;

	public float Gravity = -2f;

	public float SmokeVelocity = 0.1f;

	[Space(8f)]
	public float TurbulenceDensity = 0.1f;

	public float TurbulenceIntensity = 0.5f;

	[Space(8f)]
	public float SmokeDiffusionBySmokeVelocity;

	[Header("Driven By Muzzle Speed")]
	public float MuzzleSpeedMultiplier;

	public AnimationCurve SpeedTurbulenceDensity = AnimationCurve.Linear(0f, 0f, 30f, 6f);

	public AnimationCurve SpeedTurbulenceStrength = AnimationCurve.Linear(0f, 0f, 30f, 80f);

	public AnimationCurve SpeedSmokeStrength = AnimationCurve.Linear(0f, 1f, 20f, 0.1f);

	public AnimationCurve SpeedStartDiffusion = AnimationCurve.Linear(0f, 1f, 20f, 0.1f);

	[Header("Driven By Time")]
	public AnimationCurve Smoke = AnimationCurve.EaseInOut(0.1f, 0.2f, 3f, 0f);

	public float SmokeStrength = 1f;

	public float SmokeLength = 1f;

	public float SmokeLengthRandomness;

	public float SmokeIncreasingByShot = 0.4f;

	public float ShotFactorDropTime = 0.5f;

	private float m__E000;

	private Transform m__E001;

	[CompilerGenerated]
	private bool m__E002;

	private float m__E003;

	private LinkedList<_E000> m__E004 = new LinkedList<_E000>();

	private Vector3 m__E005;

	private Vector3 _E006;

	private Vector3 _E007;

	private float _E008;

	private int _E009;

	private float _E00A;

	private float _E00B;

	private float _E00C;

	private int _E00D;

	public bool Destroyed
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

	private void Awake()
	{
		Clear();
		_E00A = BrakeDistance * BrakeDistance;
		this.m__E001 = base.transform;
	}

	private void OnValidate()
	{
		_E00A = BrakeDistance * BrakeDistance;
	}

	public void LateUpdateValues(Camera cam, float dt)
	{
		this.m__E000 = Mathf.Max(0f, this.m__E000 - dt * ShotFactorDropTime);
		_E006 = this.m__E005;
		this.m__E005 = Vector3.Slerp(this.m__E005, this.m__E001.position, 0.125f);
		_E007 = Vector3.Slerp(_E007, (this.m__E005 - _E006) / dt, 1f / 32f);
		_E008 = Mathf.Lerp(_E008, _E007.magnitude * MuzzleSpeedMultiplier, 0.25f);
		float num = (float)_E00D * _E00C;
		this.m__E003 = Smoke.Evaluate(num) * SmokeStrength;
		if (num >= 1f)
		{
			this.m__E003 = 0f;
		}
		if (cam != null)
		{
			_E001(cam);
			if (this.m__E004.Count != 0)
			{
				_E004();
			}
		}
	}

	private void OnRenderObject()
	{
		if (!(Camera.current != _E8A8.Instance.Camera) && this.m__E004.Count != 0)
		{
			Material.SetPass(0);
			GL.Begin(7);
			_E005();
			GL.End();
		}
	}

	private void _E000()
	{
		base.enabled = true;
		_E00D = 1;
		_E00C = 1f / (SmokeLength + Random.Range(0f, SmokeLengthRandomness));
	}

	public void Clear()
	{
		this.m__E004.Clear();
		this.m__E003 = 0f;
		if (this == null)
		{
			Debug.LogError(_ED3E._E000(82984));
		}
		else
		{
			base.enabled = false;
		}
	}

	public void Shot()
	{
		this.m__E000 = Mathf.Min(this.m__E000 + SmokeIncreasingByShot, 1f);
		if (this.m__E000 >= 1f)
		{
			Clear();
			_E000();
		}
	}

	private void _E001(Camera cam)
	{
		if (this.m__E003 > 0.001f)
		{
			if (this.m__E004.Count == 0)
			{
				_E003(_E008, skipProcess: false);
				_E003(_E008);
			}
			else
			{
				this.m__E004.First.Value.Position = this.m__E005;
			}
			if ((this.m__E004.First.Value.Position - this.m__E004.First.Next.Value.Position).sqrMagnitude > _E00A)
			{
				_E000 value = this.m__E004.First.Value;
				_E00D++;
				float num = SpeedTurbulenceDensity.Evaluate(_E008);
				_E00B += num * TurbulenceDensity;
				Vector3 lhs = _E007;
				lhs.y += 0.1f;
				value.Turbulence = Vector3.Cross(lhs, cam.transform.forward).normalized * _E002(_E00B) * TurbulenceIntensity;
				value.Velocity += (_E007 + value.Turbulence * SpeedTurbulenceStrength.Evaluate(_E008)) * SmokeVelocity;
				value.Diffusion = SpeedStartDiffusion.Evaluate(_E008) * SmokeEnd;
				value.SkipProcess = false;
				_E003(_E008);
			}
		}
		else if (this.m__E004.Count >= 2 && this.m__E004.First.Value.SkipProcess && (this.m__E004.First.Value.Position - this.m__E004.First.Next.Value.Position).sqrMagnitude > _E00A)
		{
			this.m__E004.First.Value.SkipProcess = false;
		}
		if (this.m__E004.Count > 0)
		{
			if (this.m__E004.Count == 2)
			{
				if (this.m__E004.Last.Previous.Value.Diffusion >= SmokeEnd)
				{
					this.m__E004.Clear();
				}
			}
			else if (this.m__E004.Last.Previous.Value.Diffusion >= SmokeEnd)
			{
				this.m__E004.RemoveLast();
			}
		}
		else
		{
			Clear();
		}
	}

	private static float _E002(float t)
	{
		int num = (int)t;
		int num2 = num + 1;
		float a = (float)_E8EE.Int(num + 8324234, -1000, 2000) * 0.001f;
		float b = (float)_E8EE.Int(num2 + 8324234, -1000, 2000) * 0.001f;
		t -= (float)num;
		t = t * t * (3f - 2f * t);
		return Mathf.Lerp(a, b, t);
	}

	private void _E003(float velocity, bool skipProcess = true)
	{
		byte a = (byte)(Mathf.Clamp01(this.m__E003 * SpeedSmokeStrength.Evaluate(velocity)) * 255f);
		this.m__E004.AddFirst(new _E000
		{
			Position = this.m__E005,
			Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a),
			YUv = _E009++,
			SkipProcess = skipProcess
		});
	}

	private void _E004()
	{
		float deltaTime = Time.deltaTime;
		float num = Gravity * deltaTime;
		foreach (_E000 item in this.m__E004)
		{
			if (!item.SkipProcess)
			{
				item.Velocity.y -= num;
				item.Velocity *= DragValue;
				Vector3 vector = (item.Velocity + item.Turbulence) * deltaTime;
				item.Position += vector;
				item.Diffusion += vector.sqrMagnitude * SmokeDiffusionBySmokeVelocity;
			}
		}
	}

	private void _E005()
	{
		this.m__E004.First.Value.Direction = this.m__E004.Last.Value.Position - this.m__E004.First.Value.Position;
		for (LinkedListNode<_E000> next = this.m__E004.First.Next; next != null; next = next.Next)
		{
			_E000 value = next.Value;
			LinkedListNode<_E000> previous = next.Previous;
			LinkedListNode<_E000> next2 = next.Next;
			Vector3 direction = ((next2 != null) ? (next2.Value.Position - previous.Value.Position) : (value.Position - previous.Value.Position));
			value.Direction = direction;
		}
		_E000 obj = this.m__E004.First.Value;
		for (LinkedListNode<_E000> next3 = this.m__E004.First.Next; next3 != null; next3 = next3.Next)
		{
			_E000 value2 = next3.Value;
			GL.Color(obj.Color);
			GL.MultiTexCoord3(0, -1f, obj.YUv, obj.Diffusion);
			GL.MultiTexCoord(1, obj.Direction);
			GL.Vertex(obj.Position);
			GL.Color(value2.Color);
			GL.MultiTexCoord3(0, -1f, value2.YUv, value2.Diffusion);
			GL.MultiTexCoord(1, value2.Direction);
			GL.Vertex(value2.Position);
			GL.Color(value2.Color);
			GL.MultiTexCoord3(0, 1f, value2.YUv, value2.Diffusion);
			GL.MultiTexCoord(1, value2.Direction);
			GL.Vertex(value2.Position);
			GL.Color(obj.Color);
			GL.MultiTexCoord3(0, 1f, obj.YUv, obj.Diffusion);
			GL.MultiTexCoord(1, obj.Direction);
			GL.Vertex(obj.Position);
			obj = value2;
		}
	}
}
