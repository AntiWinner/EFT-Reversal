using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleCharacterController : MonoBehaviour, _E33A
{
	public class _E000
	{
		public struct _E000
		{
			public bool IsOverlapped;
		}

		public Collider[] colliders;

		public _E000[] collisionInfos;

		public int count;

		public List<Collider> seepColliders = new List<Collider>(2);

		public _E000(int capacity)
		{
			colliders = new Collider[capacity];
			collisionInfos = new _E000[capacity];
		}
	}

	[SerializeField]
	private Transform _movementTransform;

	[SerializeField]
	private CapsuleCollider _mainCollider;

	[SerializeField]
	private Collider[] _colliders;

	[SerializeField]
	private LayerMask _collisionMask;

	private const int m__E000 = 64;

	[SerializeField]
	[Header("Settings")]
	private float _castHalo = 0.05f;

	[SerializeField]
	private float _groundDotThreshold = 0.0871558f;

	[SerializeField]
	private float _maxDepenetrationDistance = 0.2f;

	[SerializeField]
	private float _maxDepenetrationDistanceResolve = 0.05f;

	[SerializeField]
	private float _snapGroundDenepentrationToUp = 0.64f;

	[SerializeField]
	private float _fixedDeltaDistance;

	[SerializeField]
	[Header("Speed Limit")]
	private float _speedLimit = -1f;

	[SerializeField]
	private float _sqrSpeedLimit = -1f;

	[Header("Resolve Tightness")]
	[SerializeField]
	private float _tightDotThreshold = -0.6f;

	[SerializeField]
	private int _maxIterationsToResolveTightness = 3;

	[SerializeField]
	private float _feelingTightMaxMotion = 0.2f;

	[SerializeField]
	private int _finePositionsToRemember = 8;

	[SerializeField]
	private bool _isSeepingActive = true;

	[SerializeField]
	[Header("Stepping")]
	private float _canStandUpRising = 0.005f;

	[SerializeField]
	private float _canStepUpExpanse = 0.005f;

	[SerializeField]
	private float _stepUpDumpPercent = 0.5f;

	[SerializeField]
	private float _canStandUpObstacleHeight = 0.15f;

	private const float m__E001 = 0.17f;

	[SerializeField]
	[Header("Huunity Depenetration Bug")]
	private bool _resolveHuunityDepenetrationBug = true;

	[SerializeField]
	private float _depenetrationBugRayAddition = -0.001f;

	[SerializeField]
	[Header("Alerts")]
	private int _maxCycleCountToHalt = 1000;

	private Vector3 m__E002;

	private int m__E003;

	private bool m__E004;

	private HashSet<Collider> m__E005 = new HashSet<Collider>();

	[CompilerGenerated]
	private bool m__E006;

	private _E000[] m__E007;

	private RaycastHit[] m__E008 = new RaycastHit[64];

	private Vector3 m__E009;

	[CompilerGenerated]
	private Action<float> m__E00A;

	private bool m__E00B;

	private bool m__E00C;

	private float m__E00D;

	private float m__E00E;

	[SerializeField]
	[Header("Other")]
	private float _slopLimitDot;

	private float m__E00F;

	[SerializeField]
	private float _stepOffsetInternal;

	private Vector3 m__E010;

	private Vector3 m__E011;

	private Func<Collider, int> m__E012;

	public bool IsSpeedLimitWasEnabledAtTheFrame
	{
		[CompilerGenerated]
		get
		{
			return this.m__E006;
		}
		[CompilerGenerated]
		set
		{
			this.m__E006 = value;
		}
	}

	public bool SupportDepenetration => true;

	public float SpeedLimit
	{
		get
		{
			return _speedLimit;
		}
		set
		{
			_speedLimit = value;
			_sqrSpeedLimit = _speedLimit * _speedLimit;
		}
	}

	public bool isEnabled
	{
		get
		{
			return _mainCollider.enabled;
		}
		set
		{
			_mainCollider.enabled = value;
		}
	}

	public Vector3 center
	{
		get
		{
			return _mainCollider.center;
		}
		set
		{
			_mainCollider.center = value;
		}
	}

	public CollisionFlags collisionFlags
	{
		get
		{
			throw new NotSupportedException();
		}
		private set
		{
			throw new NotSupportedException();
		}
	}

	public bool detectCollisions
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public float height
	{
		get
		{
			return _mainCollider.height;
		}
		set
		{
			_mainCollider.height = value;
			if (this.m__E00A != null)
			{
				this.m__E00A(value);
			}
		}
	}

	public bool isGrounded
	{
		get
		{
			return this.m__E00B;
		}
		private set
		{
			this.m__E00B = value;
		}
	}

	public float skinWidth
	{
		get
		{
			return this.m__E00D;
		}
		set
		{
			this.m__E00D = value;
		}
	}

	public float radius
	{
		get
		{
			return _mainCollider.radius;
		}
		set
		{
			_mainCollider.radius = value;
		}
	}

	public float slopeLimit
	{
		get
		{
			return this.m__E00E;
		}
		set
		{
			this.m__E00E = value;
			_slopLimitDot = Mathf.Cos(this.m__E00E * ((float)Math.PI / 180f));
		}
	}

	public float stepOffset
	{
		get
		{
			return this.m__E00F;
		}
		set
		{
			this.m__E00F = value;
			_stepOffsetInternal = this.m__E00F + 0.17f;
		}
	}

	public Vector3 velocity
	{
		get
		{
			return this.m__E010;
		}
		private set
		{
			this.m__E010 = value;
		}
	}

	public Rigidbody attachedRigidbody
	{
		get
		{
			throw new NotSupportedException();
		}
		private set
		{
			throw new NotSupportedException();
		}
	}

	public Bounds bounds
	{
		get
		{
			return _mainCollider.bounds;
		}
		private set
		{
		}
	}

	public float contactOffset
	{
		get
		{
			return _mainCollider.contactOffset;
		}
		set
		{
			_mainCollider.contactOffset = value;
		}
	}

	public Vector3 surfaceNormalAccordingToCapsule
	{
		get
		{
			return this.m__E011;
		}
		private set
		{
			this.m__E011 = value;
		}
	}

	private bool _E013
	{
		get
		{
			if (_isSeepingActive)
			{
				return this.m__E012 != null;
			}
			return false;
		}
	}

	public bool ShouldStickToGround => true;

	public event Action<float> OnHeightChanged
	{
		[CompilerGenerated]
		add
		{
			Action<float> action = this.m__E00A;
			Action<float> action2;
			do
			{
				action2 = action;
				Action<float> value2 = (Action<float>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E00A, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<float> action = this.m__E00A;
			Action<float> action2;
			do
			{
				action2 = action;
				Action<float> value2 = (Action<float>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E00A, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SetSteerDirection(Vector3 steerDirection)
	{
	}

	public void Init(Transform movementTransform, Collider[] colliders, CapsuleCollider mainCollider, float fixedDeltaDistance, LayerMask collisionMask, float stepOffset, float slopeLimit)
	{
		_movementTransform = movementTransform;
		_mainCollider = mainCollider;
		_fixedDeltaDistance = fixedDeltaDistance;
		_collisionMask = collisionMask;
		this.stepOffset = stepOffset;
		this.slopeLimit = slopeLimit;
		_E000(colliders);
	}

	public void RegisterCanSeepThroughDelegate(Func<Collider, int> canSeepThroughDelegate)
	{
		this.m__E012 = canSeepThroughDelegate;
	}

	public void AddNoSpeedLimitCollisionColliders(IEnumerable<Collider> colliders)
	{
		foreach (Collider collider in colliders)
		{
			this.m__E005.Add(collider);
		}
	}

	public void RemoveNoSpeedLimitCollisionColliders(IEnumerable<Collider> colliders)
	{
		foreach (Collider collider in colliders)
		{
			this.m__E005.Remove(collider);
		}
	}

	private void _E000(Collider[] colliders)
	{
		_colliders = colliders;
		this.m__E007 = new _E000[_colliders.Length];
		for (int i = 0; i < _colliders.Length; i++)
		{
			this.m__E007[i] = new _E000(64);
		}
	}

	public CollisionFlags Move(Vector3 motion, float deltaTime)
	{
		float magnitude = motion.magnitude;
		if (magnitude > 0f)
		{
			Vector3 vector = (this.m__E009 = _movementTransform.position);
			int num = (int)(magnitude / _fixedDeltaDistance);
			float num2 = magnitude % _fixedDeltaDistance;
			if (num > 0)
			{
				if (num >= _maxCycleCountToHalt)
				{
					return CollisionFlags.None;
				}
				Vector3 motion2 = _fixedDeltaDistance / magnitude * motion;
				for (int i = 0; i < num; i++)
				{
					_E004(motion2);
				}
			}
			Vector3 motion3 = num2 / magnitude * motion;
			_E004(motion3);
			if (_resolveHuunityDepenetrationBug)
			{
				this.m__E009 = _E012(this.m__E009);
			}
			_E00D(vector, this.m__E009, deltaTime);
			_E001(vector, deltaTime);
			if (vector != this.m__E009)
			{
				_movementTransform.position = this.m__E009;
			}
			bool flag = Physics.Raycast(this.m__E009 + Vector3.up * 0.05f, Vector3.down, 0.2f, _collisionMask, QueryTriggerInteraction.Ignore);
			this.m__E00C = isGrounded || flag;
		}
		return CollisionFlags.None;
	}

	private void _E001(Vector3 startPosition, float deltaTime)
	{
		IsSpeedLimitWasEnabledAtTheFrame = false;
		if (_speedLimit < 0f || _E002())
		{
			return;
		}
		Vector3 rhs = this.m__E010;
		if (rhs.y < 0f)
		{
			rhs.y = 0f;
		}
		float sqrMagnitude = rhs.sqrMagnitude;
		if (sqrMagnitude > _sqrSpeedLimit)
		{
			Vector3 normalized = this.m__E010.normalized;
			float num2;
			if (this.m__E010.y < 0f)
			{
				float num = Vector3.Dot(normalized, rhs) / Mathf.Sqrt(sqrMagnitude);
				num2 = _speedLimit / num;
			}
			else
			{
				num2 = _speedLimit;
			}
			this.m__E010 = normalized * num2;
			this.m__E009 = startPosition + this.m__E010 * deltaTime;
		}
		IsSpeedLimitWasEnabledAtTheFrame = true;
	}

	private bool _E002()
	{
		for (int i = 0; i < this.m__E007.Length; i++)
		{
			_E000 obj = this.m__E007[i];
			for (int j = 0; j < obj.count; j++)
			{
				Collider item = obj.colliders[j];
				if (obj.collisionInfos[j].IsOverlapped && this.m__E005.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool _E003(LayerMask layerMask)
	{
		for (int i = 0; i < this.m__E007.Length; i++)
		{
			_E000 obj = this.m__E007[i];
			for (int j = 0; j < obj.count; j++)
			{
				Collider collider = obj.colliders[j];
				if (obj.collisionInfos[j].IsOverlapped && ((1 << collider.gameObject.layer) & (int)layerMask) != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void _E004(Vector3 motion)
	{
		Vector3 resolvedPoint = this.m__E009 + motion;
		if (_stepOffsetInternal > 0.17f)
		{
			resolvedPoint = _E00E(this.m__E009, motion, resolvedPoint);
		}
		isGrounded = false;
		bool feelingTightMain = false;
		bool flag = false;
		for (int i = 0; i < 1 + _maxIterationsToResolveTightness; i++)
		{
			bool updateNeighbours = i == 0;
			_E005(resolvedPoint, updateNeighbours, !feelingTightMain, out resolvedPoint, out feelingTightMain);
			if (!feelingTightMain)
			{
				break;
			}
			if (!flag && this._E013)
			{
				_E00A();
				flag = true;
			}
		}
		bool flag2 = true;
		Vector3 vector = resolvedPoint - this.m__E009;
		float magnitude = vector.magnitude;
		if (magnitude > 0f)
		{
			if (magnitude >= _maxDepenetrationDistance)
			{
				resolvedPoint = this.m__E009 + vector * (_maxDepenetrationDistanceResolve / magnitude);
				flag2 = false;
			}
			else if (feelingTightMain)
			{
				float num = magnitude / _feelingTightMaxMotion;
				if (num > 1f)
				{
					num = 1f;
				}
				num = 1f - num;
				if (num > 0f)
				{
					resolvedPoint = this.m__E009 + vector * num;
				}
				else if (this.m__E004)
				{
					resolvedPoint = this.m__E002;
				}
				flag2 = false;
			}
		}
		if (flag2)
		{
			this.m__E003++;
			if (this.m__E003 >= _finePositionsToRemember)
			{
				this.m__E002 = resolvedPoint;
				this.m__E003 = 0;
				this.m__E004 = true;
			}
		}
		else
		{
			this.m__E003 = 0;
		}
		this.m__E009 = resolvedPoint;
	}

	private int _E005(Vector3 desiredPoint, bool updateNeighbours, bool doSnapToGround, out Vector3 resolvedPoint, out bool feelingTightMain)
	{
		int num = 0;
		feelingTightMain = false;
		resolvedPoint = desiredPoint;
		for (int i = 0; i < _colliders.Length; i++)
		{
			Collider collider = _colliders[i];
			Vector3 desiredPoint2 = resolvedPoint + (collider.transform.position - _movementTransform.position);
			float snapGroundDenepentrationToUp = 0f;
			if (doSnapToGround && collider == _mainCollider)
			{
				snapGroundDenepentrationToUp = _snapGroundDenepentrationToUp;
			}
			_E000 neighboursColliders = this.m__E007[i];
			Vector3 surfaceNormal;
			bool feelingTight;
			int num2 = _E006(collider, neighboursColliders, desiredPoint2, snapGroundDenepentrationToUp, updateNeighbours, out resolvedPoint, out surfaceNormal, out feelingTight);
			num += num2;
			if (collider == _mainCollider)
			{
				this.m__E011 = surfaceNormal;
				if (feelingTight)
				{
					feelingTightMain = true;
				}
			}
		}
		if (num > 0)
		{
			this.m__E011 = this.m__E011.normalized;
		}
		else
		{
			this.m__E011 = Vector3.up;
		}
		return num;
	}

	private int _E006(Collider collider, _E000 neighboursColliders, Vector3 desiredPoint, float snapGroundDenepentrationToUp, bool updateNeighbours, out Vector3 resolvedPoint, out Vector3 surfaceNormal, out bool feelingTight)
	{
		int num = 0;
		Vector3 zero = Vector3.zero;
		feelingTight = false;
		resolvedPoint = desiredPoint;
		if (updateNeighbours)
		{
			_E007(collider, neighboursColliders, desiredPoint);
			_E009(neighboursColliders);
		}
		surfaceNormal = Vector3.zero;
		for (int i = 0; i < neighboursColliders.count; i++)
		{
			Collider collider2 = neighboursColliders.colliders[i];
			if (_E008(collider2))
			{
				continue;
			}
			Vector3 direction;
			float distance;
			bool flag = Physics.ComputePenetration(collider, resolvedPoint, collider.transform.rotation, collider2, collider2.transform.position, collider2.transform.rotation, out direction, out distance);
			neighboursColliders.collisionInfos[i] = new _E000._E000
			{
				IsOverlapped = flag
			};
			bool flag2 = neighboursColliders.seepColliders.Contains(collider2);
			if (flag)
			{
				if (flag2)
				{
					continue;
				}
				float num2 = Vector3.Dot(direction, Vector3.up);
				if (num2 >= _groundDotThreshold)
				{
					if (direction.y > surfaceNormal.y)
					{
						surfaceNormal = direction;
					}
					if (collider2.gameObject.layer == _E37B.PlayerLayer)
					{
						distance *= EFTHardSettings.Instance.HumanPyramidExtraDepenetration;
					}
					else if (snapGroundDenepentrationToUp > 0f && num2 >= snapGroundDenepentrationToUp)
					{
						direction = Vector3.up;
						if (num2 > 0f)
						{
							distance /= num2;
						}
					}
					isGrounded = true;
				}
				if (num > 0 && Vector3.Dot(zero.normalized, direction) < _tightDotThreshold)
				{
					feelingTight = true;
				}
				num++;
				Vector3 vector = distance * direction;
				zero += direction;
				resolvedPoint += vector;
			}
			else if (flag2)
			{
				neighboursColliders.seepColliders.Remove(collider2);
			}
		}
		return num;
	}

	private void _E007(Collider collider, _E000 neighboursColliders, Vector3 desiredPoint)
	{
		Vector3 position = desiredPoint + (collider.bounds.center - collider.transform.position);
		float num = Mathf.Max(Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z) + _castHalo;
		neighboursColliders.count = Physics.OverlapSphereNonAlloc(position, num, neighboursColliders.colliders, _collisionMask, QueryTriggerInteraction.Ignore);
	}

	private bool _E008(Collider collider)
	{
		for (int i = 0; i < _colliders.Length; i++)
		{
			Collider collider2 = _colliders[i];
			if (collider2 == collider || _E320.GetIgnoreCollision(collider2, collider))
			{
				return true;
			}
		}
		return false;
	}

	private void _E009(_E000 neighboursColliders)
	{
		for (int num = neighboursColliders.seepColliders.Count - 1; num >= 0; num--)
		{
			Collider collider = neighboursColliders.seepColliders[num];
			bool flag = false;
			for (int i = 0; i < neighboursColliders.count; i++)
			{
				Collider collider2 = neighboursColliders.colliders[i];
				if (collider == collider2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				neighboursColliders.seepColliders.RemoveAt(num);
			}
		}
	}

	private void _E00A()
	{
		for (int i = 0; i < this.m__E007.Length; i++)
		{
			_E000 collidersArray = this.m__E007[i];
			_E00B(collidersArray);
		}
	}

	private void _E00B(_E000 collidersArray)
	{
		int num = int.MaxValue;
		bool flag = false;
		for (int i = 0; i < collidersArray.count; i++)
		{
			Collider collider = collidersArray.colliders[i];
			if (_E008(collider))
			{
				continue;
			}
			int num2 = _E00C(collider);
			if (num2 >= 0 && num2 <= num)
			{
				if (!collidersArray.seepColliders.Contains(collider))
				{
					collidersArray.seepColliders.Add(collider);
				}
				if (num2 < num)
				{
					flag = true;
				}
				num = num2;
			}
		}
		if (!flag)
		{
			return;
		}
		for (int num3 = collidersArray.seepColliders.Count - 1; num3 >= 0; num3--)
		{
			Collider collider2 = collidersArray.seepColliders[num3];
			if (_E00C(collider2) > num)
			{
				collidersArray.seepColliders.RemoveAt(num3);
			}
		}
	}

	private int _E00C(Collider collider)
	{
		return this.m__E012(collider);
	}

	private void _E00D(Vector3 currentPoint, Vector3 destinationPoint, float deltaTime)
	{
		this.m__E010 = (destinationPoint - currentPoint) / deltaTime;
	}

	private Vector3 _E00E(Vector3 startPoint, Vector3 motion, Vector3 desiredPoint)
	{
		Vector3 result = desiredPoint;
		Vector3 vector = new Vector3(motion.x, 0f, motion.z);
		Vector3 normalized = vector.normalized;
		float magnitude = vector.magnitude;
		if (magnitude > 0f && _E00F(startPoint, normalized, magnitude, out var bestHit))
		{
			float num = 1f - Vector3.Dot(-normalized, bestHit.normal);
			if (_slopLimitDot <= num && num <= 1f)
			{
				float num2 = bestHit.point.y - result.y;
				if (num2 > 0f && _E011(bestHit.point))
				{
					result.y += num2 * _stepUpDumpPercent;
				}
			}
		}
		return result;
	}

	private bool _E00F(Vector3 startPoint, Vector3 forwardDirection, float forwardDistance, out RaycastHit bestHit)
	{
		float num = radius / 2f;
		bool flag = false;
		if (!this.m__E00C)
		{
			flag = true;
		}
		else if (Physics.Raycast(new Ray(startPoint + Vector3.up * (height * _canStandUpObstacleHeight), forwardDirection), radius * 1.1f, _collisionMask, QueryTriggerInteraction.Ignore))
		{
			flag = true;
		}
		bool flag2 = false;
		if (flag)
		{
			Vector3 vector = startPoint + forwardDirection * (forwardDistance + _canStepUpExpanse + radius) + Vector3.up * _stepOffsetInternal;
			Ray ray = new Ray(vector, Vector3.down);
			flag2 = _E010(ray, this.m__E008, _stepOffsetInternal, _collisionMask, out bestHit);
			if (flag2 && !this.m__E00C)
			{
				float num2 = radius * 0.5f;
				Ray ray2 = new Ray(vector + Vector3.up * num2, forwardDirection.normalized * num2);
				RaycastHit[] results = new RaycastHit[5];
				flag2 = !_E010(ray2, results, num2, _collisionMask, out var _);
			}
		}
		else
		{
			Vector3 vector2 = startPoint + forwardDirection * forwardDistance + Vector3.up * (num + _canStandUpRising + _stepOffsetInternal);
			flag2 = Physics.CapsuleCast(vector2, vector2, num, Vector3.down, out bestHit, _stepOffsetInternal, _collisionMask, QueryTriggerInteraction.Ignore);
		}
		return flag2;
	}

	private bool _E010(Ray ray, RaycastHit[] results, float maxDistance, int layerMask, out RaycastHit bestHit)
	{
		int num = Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
		float num2 = float.MaxValue;
		bestHit = default(RaycastHit);
		bool result = false;
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = results[i];
			if (!_E008(raycastHit.collider) && raycastHit.distance < num2)
			{
				num2 = raycastHit.distance;
				bestHit = raycastHit;
				result = true;
			}
		}
		return result;
	}

	private bool _E011(Vector3 point)
	{
		int count = Physics.RaycastNonAlloc(new Ray(point + Vector3.up * _canStandUpRising, Vector3.up), this.m__E008, height, _collisionMask, QueryTriggerInteraction.Ignore);
		return !_E013(count, this.m__E008);
	}

	private Vector3 _E012(Vector3 point)
	{
		Vector3 result = point;
		float num = radius;
		Ray ray = new Ray(point + Vector3.up * num, Vector3.down);
		if (_E010(ray, this.m__E008, num + _depenetrationBugRayAddition, _collisionMask, out var bestHit))
		{
			result = bestHit.point;
		}
		return result;
	}

	private bool _E013(int count, RaycastHit[] colliders)
	{
		bool result = false;
		for (int i = 0; i < count; i++)
		{
			RaycastHit raycastHit = colliders[i];
			if (!_E008(raycastHit.collider))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool _E014(int count, Collider[] colliders)
	{
		bool result = false;
		for (int i = 0; i < count; i++)
		{
			Collider collider = colliders[i];
			if (!_E008(collider))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool SimpleMove(Vector3 motion)
	{
		throw new NotImplementedException();
	}

	public void CopyFields(_E33A characterController)
	{
		_E33B.CopyFields(this, characterController);
	}

	public void CopyFields(_E721 footprint)
	{
		_E33B.CopyFields(this, footprint);
	}

	public _E721 GetFootprint()
	{
		return _E33B.GetFootprint(this);
	}

	public Collider GetCollider()
	{
		return _mainCollider;
	}

	[Conditional("UNITY_EDITOR")]
	private void _E015(string message, params object[] args)
	{
	}

	[Conditional("UNITY_EDITOR")]
	private void _E016<_E077>(string name, _E077 msg)
	{
	}

	public void SeTightDotThreshold(float degrees)
	{
		_tightDotThreshold = Mathf.Cos(degrees * ((float)Math.PI / 180f));
	}

	public void SetSnapGroundDenepentrationToUpInDegrees(float degrees)
	{
		_snapGroundDenepentrationToUp = Mathf.Cos(degrees * ((float)Math.PI / 180f));
	}

	public void SeGroundDotThreshold(float degrees)
	{
		_groundDotThreshold = Mathf.Cos(degrees * ((float)Math.PI / 180f));
	}

	public void SeSlotLimitDot(float degrees)
	{
		slopeLimit = degrees;
	}

	private void OnDrawGizmos()
	{
		Color green = Color.green;
		for (int i = 0; i < _colliders.Length; i++)
		{
			Collider collider = _colliders[i];
			Matrix4x4 matrix4x = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, collider.transform.lossyScale);
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix *= matrix4x;
			CapsuleCollider capsuleCollider = collider as CapsuleCollider;
			SphereCollider sphereCollider = collider as SphereCollider;
			if (capsuleCollider != null)
			{
				Vector3 vector = new Vector3(0f, capsuleCollider.height / 2f, 0f);
				_E36B.DrawCapsule(capsuleCollider.center - vector, capsuleCollider.center + vector, green, capsuleCollider.radius);
			}
			else if (sphereCollider != null)
			{
				Gizmos.color = green;
				Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
			}
			else
			{
				Gizmos.color = green;
				Gizmos.DrawWireCube(collider.transform.InverseTransformPoint(collider.bounds.center), collider.bounds.size);
			}
			Gizmos.matrix = matrix;
		}
	}
}
