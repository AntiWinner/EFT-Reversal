using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BouncingObject : MonoBehaviour
{
	private struct _E000
	{
		public int index;

		public Vector3 beginPoint;

		public Vector3 startVelocity;

		public Vector3 acceleration;

		public float endTime;

		public float currentTime;

		public Vector3 currentPoint;

		public Vector3 currentVelocity;

		public Vector3 nextJumpDirection;

		public bool useBezier;

		public Vector3 bezierRaisePoint;

		public float bezierStartTime;

		public Vector3 bezierStartPoint;

		public Vector3 bezierEndPoint;

		public Collider hitCollider;

		private const float _E000 = 0.3f;

		public void Init(int index, Vector3 beginPoint, Vector3 startVelocity, Vector3 acceleration)
		{
			this.index = index;
			this.beginPoint = beginPoint;
			this.startVelocity = startVelocity;
			this.acceleration = acceleration;
		}

		public void SetUseBezier(float bezierStartTime, Vector3 startPoint, Vector3 endPoint, float distance)
		{
			Vector3 vector = (endPoint + startPoint) * 0.5f;
			SetUseBezier(bezierStartTime, startPoint, vector + Vector3.up * (distance * 0.3f), endPoint);
		}

		public void SetUseBezier(float bezierStartTime, Vector3 startPoint, Vector3 bezierRaisePoint, Vector3 endPoint)
		{
			useBezier = true;
			bezierStartPoint = startPoint;
			bezierEndPoint = endPoint;
			this.bezierRaisePoint = bezierRaisePoint;
			this.bezierStartTime = bezierStartTime;
		}

		public Vector3 CalculateVelocity(float time)
		{
			return startVelocity + acceleration * time;
		}

		public Vector3 CalculatePoint(float time, Vector3 currentVelocity)
		{
			return beginPoint + currentVelocity * time + 0.5f * acceleration * (time * time);
		}

		public bool Update(ref float deltaTime)
		{
			currentTime += deltaTime;
			float num = currentTime - endTime;
			bool result;
			if (num > 0f)
			{
				currentTime = endTime;
				deltaTime = num;
				result = true;
			}
			else
			{
				deltaTime = 0f;
				result = false;
			}
			currentVelocity = CalculateVelocity(currentTime);
			if (useBezier && currentTime >= bezierStartTime)
			{
				if (endTime > 0f)
				{
					float t = currentTime / endTime;
					currentPoint = _E4D2.GetPoint(bezierStartPoint, bezierRaisePoint, bezierEndPoint, t);
				}
				else
				{
					currentPoint = bezierEndPoint;
				}
			}
			else
			{
				currentPoint = CalculatePoint(currentTime, currentVelocity);
			}
			return result;
		}
	}

	private float m__E000;

	private LayerMask m__E001;

	private RaycastHit[] m__E002 = new RaycastHit[8];

	private int m__E003;

	private _E000 _E004;

	private float _E005;

	private float _E006;

	private float _E007;

	private float _E008;

	private int _E009;

	private const int _E00A = 3;

	private bool _E00B;

	private Vector3 _E00C = Physics.gravity;

	[CompilerGenerated]
	private bool _E00D;

	private Vector3 _E00E;

	public bool Finished
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		private set
		{
			_E00D = value;
		}
	}

	public float VelocitySqrMagnitude => _E004.currentVelocity.sqrMagnitude;

	public void Init(Vector3 beginPoint, Vector3 velocity, float radius, float playMult, LayerMask castMask, int maxCastCount, float deltaTimeStep, float randomReboundSpread, float bounceSpeedMult, bool showDebug)
	{
		base.transform.position = beginPoint;
		this.m__E001 = castMask;
		this.m__E003 = maxCastCount;
		this.m__E000 = deltaTimeStep;
		_E005 = playMult;
		_E006 = randomReboundSpread;
		_E00B = showDebug;
		_E007 = bounceSpeedMult;
		_E008 = radius;
		Finished = false;
		_E004 = _E000(0, beginPoint, velocity);
		_E009 = 3;
	}

	private _E000 _E000(int index, Vector3 beginPoint, Vector3 velocity)
	{
		_E000 result = default(_E000);
		result.Init(index, beginPoint, velocity, _E00C);
		float num = 0f;
		float num2 = 0f;
		while (this.m__E003 > 0)
		{
			num2 = num;
			num += this.m__E000;
			this.m__E003--;
			Vector3 currentVelocity = result.CalculateVelocity(num);
			Vector3 vector = result.CalculatePoint(num, currentVelocity);
			Vector3 direction = vector - beginPoint;
			float magnitude = direction.magnitude;
			if (_E003(beginPoint, direction, magnitude, out var hit))
			{
				result.hitCollider = hit.collider;
				Vector3 vector2;
				if (magnitude > 0f)
				{
					vector2 = direction.normalized;
					num -= (1f - hit.distance / magnitude) * this.m__E000;
				}
				else
				{
					vector2 = Vector3.down;
				}
				result.nextJumpDirection = (vector2 + 2f * hit.normal + Random.insideUnitSphere * _E006).normalized;
				Vector3 endPoint = hit.point + hit.normal * _E008;
				result.SetUseBezier(num2, beginPoint, endPoint, hit.distance);
				if (_E00B)
				{
					_E36B.DebugPoint(result.bezierRaisePoint, Color.red, 0.2f, 5f);
				}
				break;
			}
			beginPoint = vector;
		}
		result.endTime = num;
		return result;
	}

	[Conditional("UNITY_EDITOR")]
	private void _E001(_E000 jump, float time)
	{
	}

	private void _E002(float time)
	{
		Collider collider = null;
		int num = 100;
		while (time > 0f)
		{
			bool num2 = _E004.Update(ref time);
			if (_E00B)
			{
				if (_E00E == Vector3.zero)
				{
					_E00E = base.transform.position;
				}
				UnityEngine.Debug.DrawLine(_E00E, _E004.currentPoint, Color.blue, 5f);
				_E00E = _E004.currentPoint;
			}
			if (num2)
			{
				if (this.m__E003 <= 0)
				{
					OnDone();
					break;
				}
				collider = _E004.hitCollider;
				float num3 = _E004.currentVelocity.magnitude * _E007;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				Vector3 velocity = _E004.nextJumpDirection * num3;
				_E004 = _E000(_E004.index + 1, _E004.currentPoint, velocity);
			}
			num--;
			if (num <= 0)
			{
				UnityEngine.Debug.LogError(_ED3E._E000(56119));
				OnDone();
				break;
			}
		}
		if (collider != null && _E009 >= 3)
		{
			_E009 = 0;
			OnBounce(collider);
		}
	}

	protected virtual void Update()
	{
		_E002(Time.deltaTime * _E005);
		_E009++;
		base.transform.position = _E004.currentPoint;
	}

	private bool _E003(Vector3 point, Vector3 direction, float maxDistance, out RaycastHit hit)
	{
		hit = default(RaycastHit);
		Ray ray = new Ray(point, direction);
		if (_E00B)
		{
			UnityEngine.Debug.DrawRay(ray.origin, ray.direction * maxDistance, new Color(1f, 1f, 0f, 0.25f), 5f);
		}
		int num = Physics.RaycastNonAlloc(ray, this.m__E002, maxDistance, this.m__E001, QueryTriggerInteraction.Ignore);
		if (num > 0)
		{
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.m__E002[i];
				if (raycastHit.distance < num2)
				{
					hit = raycastHit;
					num2 = raycastHit.distance;
				}
			}
			return true;
		}
		return false;
	}

	protected virtual void OnDone()
	{
		Finished = true;
		base.enabled = false;
		if (_E00B)
		{
			UnityEngine.Debug.Log(_ED3E._E000(56108));
		}
	}

	protected virtual void OnBounce(Collider collider)
	{
		if (_E00B)
		{
			UnityEngine.Debug.Log(_ED3E._E000(56105));
		}
	}

	private void OnDrawGizmos()
	{
		if (_E00B)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 0.1f);
		}
	}
}
