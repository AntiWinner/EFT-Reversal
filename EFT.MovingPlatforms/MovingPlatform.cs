using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BezierSplineTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace EFT.MovingPlatforms;

public abstract class MovingPlatform : MonoBehaviour, IPhysicsTrigger
{
	public interface _E000
	{
		void Board(MovingPlatform platform);

		void GetOff(MovingPlatform platform);
	}

	public class _E001
	{
		public Locomotive Platform;

		public byte Id;

		private _E5C6 _E000;

		public const float SYNC_SMOOTHNESS = 1.5f;

		private float _E001;

		private bool _E002;

		public bool HasNetPacket
		{
			get
			{
				if (Platform.enabled)
				{
					return Platform.TravelState.Value != Locomotive.ETravelState.NotStarted;
				}
				return false;
			}
		}

		public _E5C6 GetNetPacket()
		{
			_E5C6 result = default(_E5C6);
			result.Id = Id;
			result.Position = Platform.NormalCurvePosition;
			return result;
		}

		public void StoreNetPacket(_E5C6 packet)
		{
			if (!(packet.Position <= _E000.Position))
			{
				_E000 = packet;
				_E002 = true;
			}
		}

		public void ApplyStoredPackets()
		{
			if (Platform.Initialized && _E002)
			{
				float target = (float)(_E5AD.UtcNow - TimeSpan.FromSeconds(_E000.Position * Platform._routeDuration) - Platform._E010).TotalSeconds;
				float num = Mathf.SmoothDamp(0f, target, ref _E001, 1.5f);
				Platform._E010 += TimeSpan.FromSeconds(num);
				_E002 = false;
			}
		}
	}

	[Serializable]
	public class PreciseInterpolator
	{
		public float Distance1;

		public float Distance2;

		public Vector3 Point1;

		public Vector3 Point2;

		public Vector3 GetPosition(Vector3 initial, float distance, out float blendFactor)
		{
			blendFactor = 0f;
			if (distance < Distance1)
			{
				return initial;
			}
			float num = Mathf.InverseLerp(Distance1, Distance2, distance);
			Vector3 b = Vector3.Lerp(Point1, Point2, num);
			blendFactor = Mathf.Sqrt(num);
			return Vector3.Lerp(initial, b, blendFactor);
		}

		public Vector3 GetPosition(Vector3 initial, float distance)
		{
			if (distance < Distance1)
			{
				return initial;
			}
			float num = Mathf.InverseLerp(Distance1, Distance2, distance);
			Vector3 b = Vector3.Lerp(Point1, Point2, num);
			return Vector3.Lerp(initial, b, Mathf.Sqrt(num));
		}
	}

	[FormerlySerializedAs("Spline")]
	[SerializeField]
	private BezierSpline _spline;

	[SerializeField]
	private float _approxLength;

	[SerializeField]
	protected float _routeDuration;

	[SerializeField]
	private int DebugViewSteps = 50;

	[SerializeField]
	protected Vector3 _forwardRotation;

	[SerializeField]
	protected float _currentDistance;

	[SerializeField]
	protected float _currentTime;

	[SerializeField]
	protected Vector3 _contactPoint;

	public AnimationCurve MovementCurve;

	public _ECEF<Player> Passengers = new _ECEF<Player>();

	public float Precision = 0.01f;

	[HideInInspector]
	protected Transform Transform;

	public PreciseInterpolator LinearInterpolator;

	public BoxCollider Area;

	[CompilerGenerated]
	private bool _E00B;

	[CompilerGenerated]
	private Vector3[] _E00C;

	[CompilerGenerated]
	private readonly string _E00D = _ED3E._E000(209307);

	public Vector3 JitterDirection = new Vector3(0f, 0.01f, 0f);

	public Vector3 JitterAngle = new Vector3(0f, 1f, 0f);

	public float JitterPeriod = 0.5f;

	private Quaternion _E00E;

	private readonly List<Vector3> _E00F = new List<Vector3>(16);

	private DateTime _E010;

	public bool Initialized
	{
		[CompilerGenerated]
		get
		{
			return _E00B;
		}
		[CompilerGenerated]
		set
		{
			_E00B = value;
		}
	}

	public virtual Vector3[] CachedDirections
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		set
		{
			_E00C = value;
		}
	}

	public float CurrentRouteTime => Mathf.Max(0f, (float)(_E5AD.UtcNow - _E010).TotalSeconds);

	public float NormalCurvePosition => CurrentRouteTime / _routeDuration;

	public float Position => _currentDistance;

	public float CTime => _currentTime;

	public virtual float CurveApproxLength => _approxLength;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
	}

	public virtual BezierSpline Spline
	{
		get
		{
			return _spline;
		}
		set
		{
			_spline = value;
		}
	}

	public Quaternion RotationAtCurrentDistance => RotationAtDistance(_currentDistance);

	public virtual void Init(DateTime depart)
	{
		_E010 = depart;
		Initialized = true;
		base.enabled = true;
		Transform = base.transform;
	}

	public virtual void CalculateLength()
	{
		Spline.CreateCurvesLengthCache(0.01f);
		_approxLength = Spline.GetLengthFromChache();
		_routeDuration = MovementCurve.GetDuration();
	}

	public abstract void PlaceAtStartPosition();

	public abstract void PlaceAtEndPosition();

	public Quaternion RotationAtDistance(float distance)
	{
		int num = (int)distance;
		Vector3 vector = CachedDirections[num];
		Vector3 b = ((num + 1 >= CachedDirections.Length) ? vector : CachedDirections[num + 1]);
		float t = ((num > 0) ? (distance % (float)num) : distance);
		return Quaternion.LookRotation(Vector3.Lerp(vector, b, t), Vector3.up) * Quaternion.Euler(_forwardRotation);
	}

	public void Jitter(ref Vector3 position, ref Quaternion rotation)
	{
		float num = position.x * JitterPeriod;
		float num2 = 0.5f;
		position += (Mathf.Max(num2, Mathf.Sin(num) + Mathf.Cos(2f * num)) - num2) * JitterDirection;
		rotation *= Quaternion.Euler(JitterAngle * Mathf.Sin(num));
	}

	public virtual void Move(bool force = false)
	{
		StorePassengers();
	}

	protected void StorePassengers()
	{
		for (int i = 0; i < Passengers.Count; i++)
		{
			Player player = Passengers[i];
			_E00F.Add(Transform.InverseTransformPoint(player.Position));
		}
	}

	public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		_E00E = Transform.rotation;
		Transform.SetPositionAndRotation(position, rotation);
		UpdatePassengerPositions();
	}

	protected void UpdatePassengerPositions()
	{
		Quaternion platformRotation = Transform.rotation * Quaternion.Inverse(_E00E);
		for (int i = 0; i < Passengers.Count; i++)
		{
			Player player = Passengers[i];
			player.MovementContext.PlatformRotation = platformRotation;
			Vector3 vector = Transform.TransformPoint(_E00F[i]);
			player.MovementContext.PlatformMotion = vector - player.Position;
		}
		_E00F.Clear();
	}

	public virtual void Update()
	{
		Move();
	}

	public virtual void FixedUpdate()
	{
	}

	public virtual void OnDrawGizmosSelected()
	{
		float currentTime = 1f;
		float num = CurveApproxLength;
		if (Spline != null)
		{
			Gizmos.color = Color.green;
			for (int num2 = DebugViewSteps - 1; num2 >= 0; num2--)
			{
				float num3 = CurveApproxLength * (float)num2 / (float)DebugViewSteps;
				Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(num, num3, Precision, ref currentTime);
				num = num3;
				Debug.DrawRay(pointForHomogeneousMovement, CachedDirections[(int)num] * 3f, Color.green, Time.deltaTime);
				Gizmos.DrawWireCube(pointForHomogeneousMovement, Vector3.one / 5f);
			}
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		col.GetComponent<_E000>()?.Board(this);
	}

	public void OnTriggerExit(Collider col)
	{
		col.GetComponent<_E000>()?.GetOff(this);
	}

	public virtual void OnRouteFinished()
	{
		base.enabled = false;
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
