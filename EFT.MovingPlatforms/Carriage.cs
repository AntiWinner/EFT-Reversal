using System;
using System.Linq;
using BezierSplineTools;
using UnityEngine;

namespace EFT.MovingPlatforms;

public class Carriage : MovingPlatform
{
	public enum CarriageMoveType
	{
		LegacyBySelfRoute,
		BasedOnLocomotivePos
	}

	[SerializeField]
	private float _suspensionStiffness = 3f;

	[SerializeField]
	private Vector2 _suspension = new Vector2(-0.1f, 0.2f);

	[SerializeField]
	private float _shiftDelta;

	public CarriageMoveType carriageMoveType;

	public Locomotive Locomotive;

	public Vector3 LocomotivePivot;

	public Vector3 LocalPivot;

	[Tooltip("Смещение вагона относительно локомотива")]
	[Header("Trailer shift and jitter")]
	public float Shift;

	[Tooltip("Диапозон тряски вагона назад/вперед")]
	public Vector3 ShiftRange = new Vector3(-0.15f, 0.15f, 0f);

	[Tooltip("Объект в направлении которого вагон смотрит (локомотив или след. вагон)")]
	public Transform RotationTarget;

	[Tooltip("Смещение вагона, использовать чтобы выровнять его (менять крайне аккуратно)")]
	public Vector3 CarriagePosOffset;

	private new Transform m__E000;

	private new float m__E001;

	private Vector2 m__E002;

	private float m__E003;

	private bool _E004;

	private float _E005;

	public override Vector3[] CachedDirections => Locomotive.CachedDirections;

	public override float CurveApproxLength => Locomotive.CurveApproxLength;

	public override BezierSpline Spline => Locomotive.Spline;

	public Transform ParentPlatform
	{
		get
		{
			if (this.m__E000 != null)
			{
				return this.m__E000;
			}
			for (int i = 0; i < Locomotive.Carriage.Length; i++)
			{
				if (!(Locomotive.Carriage[i] != this))
				{
					int num = i - 1;
					this.m__E000 = ((num < 0) ? Locomotive.transform : Locomotive.Carriage[num].transform);
				}
			}
			return this.m__E000;
		}
	}

	public override void Init(DateTime depart)
	{
		base.Init(depart);
		this.m__E003 = UnityEngine.Random.Range(0f, 1f);
		_E004 = UnityEngine.Random.Range(0, 2) == 0;
		_E005 = UnityEngine.Random.Range(0f, 100f);
		this.m__E002 = new Vector2(Shift - _shiftDelta, Shift + _shiftDelta);
		this.m__E001 = Shift;
	}

	public override void Move(bool force = false)
	{
		if (!Locomotive.OnRoute && !force)
		{
			return;
		}
		base.Move(force);
		try
		{
			switch (carriageMoveType)
			{
			case CarriageMoveType.LegacyBySelfRoute:
				_E000();
				break;
			case CarriageMoveType.BasedOnLocomotivePos:
				_E001(Locomotive.Position - Shift, Locomotive.Position);
				break;
			}
		}
		catch (Exception arg)
		{
			Debug.LogError(string.Format(_ED3E._E000(209175), arg, Locomotive == null, RotationTarget == null));
		}
	}

	private void _E000()
	{
		float num = Locomotive.Position + Mathf.Lerp(this.m__E001, Shift, Locomotive.SuspensionBlendFactor);
		if (!(num < 0f))
		{
			Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(_currentDistance, num, Locomotive.Precision, ref _currentTime);
			_currentDistance = Mathf.Clamp(0f, num, CurveApproxLength);
			pointForHomogeneousMovement = LinearInterpolator.GetPosition(pointForHomogeneousMovement, _currentDistance);
			Quaternion rotation = base.RotationAtCurrentDistance;
			Vector3 position = pointForHomogeneousMovement - rotation * _contactPoint;
			Jitter(ref position, ref rotation);
			SetPositionAndRotation(position, rotation);
			Vector3 vector = base.transform.TransformPoint(LocalPivot);
			Vector3 vector2 = ParentPlatform.TransformPoint(LocomotivePivot);
			float num2 = Vector3.Distance(vector, ParentPlatform.TransformPoint(LocomotivePivot));
			Vector3 normalized = (vector2 - vector).normalized;
			Vector3 normalized2 = (ParentPlatform.position - vector).normalized;
			if (Vector3.Dot(normalized, normalized2) < 0f)
			{
				num2 = 0f - num2;
			}
			if (!(num2 > _suspension.x) || !(num2 < _suspension.y))
			{
				float b = Mathf.Clamp(this.m__E001 + num2, this.m__E002.x, this.m__E002.y);
				this.m__E001 = Mathf.Lerp(this.m__E001, b, Time.deltaTime * _suspensionStiffness);
			}
		}
	}

	private void _E001(float currentDistance, float locomotiveDistance)
	{
		float currentTime = Locomotive.CTime;
		Vector3 pointForHomogeneousMovement = Locomotive.Spline.GetPointForHomogeneousMovement(currentDistance, locomotiveDistance, Locomotive.Precision, ref currentTime);
		Vector3 position = Locomotive.transform.position;
		float num = Vector3.Distance(position, pointForHomogeneousMovement);
		Vector3 normalized = (position - pointForHomogeneousMovement).normalized;
		normalized.y = 0f;
		this.m__E003 += Time.deltaTime;
		_currentDistance = currentDistance;
		if (this.m__E003 > 1f)
		{
			this.m__E003 -= 1f;
			_E004 = !_E004;
		}
		float num2 = (_E004 ? Mathf.Lerp(ShiftRange.y, ShiftRange.x, this.m__E003) : Mathf.Lerp(ShiftRange.x, ShiftRange.y, this.m__E003));
		if (num > Mathf.Abs(Shift))
		{
			pointForHomogeneousMovement += normalized * (num - Mathf.Abs(Shift));
		}
		pointForHomogeneousMovement += CarriagePosOffset;
		Vector3 normalized2 = (RotationTarget.transform.position - pointForHomogeneousMovement).normalized;
		normalized2.y = 0f;
		pointForHomogeneousMovement += new Vector3(0f, 0f, num2 * (1f - Locomotive.SuspensionBlendFactor));
		Quaternion rotation = Quaternion.LookRotation(normalized2, Vector3.up) * Quaternion.Euler(_forwardRotation);
		if (currentDistance <= 60f)
		{
			rotation = Quaternion.LookRotation((Locomotive.transform.TransformPoint(0f, 100f, 0f) - pointForHomogeneousMovement).normalized, Vector3.up) * Quaternion.Euler(_forwardRotation);
		}
		rotation *= Quaternion.Euler(JitterAngle * Mathf.Sin(_currentDistance * JitterPeriod + _E005));
		SetPositionAndRotation(pointForHomogeneousMovement, rotation);
	}

	[ContextMenu("Start_Position")]
	public override void PlaceAtStartPosition()
	{
		if (carriageMoveType == CarriageMoveType.LegacyBySelfRoute)
		{
			_E002();
			return;
		}
		Transform = base.transform;
		_E001(0f, 0f);
	}

	[ContextMenu("End_Position")]
	public override void PlaceAtEndPosition()
	{
		if (carriageMoveType == CarriageMoveType.LegacyBySelfRoute)
		{
			_E003();
			return;
		}
		Transform = base.transform;
		float num = Locomotive.MovementCurve.Evaluate(Locomotive.GetArrivedTiming());
		_E001(0f, num + Shift);
	}

	private void _E002()
	{
		int num = 0;
		float currentTime = 0f;
		Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(0f, num, Precision, ref currentTime);
		Debug.Log(string.Format(_ED3E._E000(209229), base.gameObject.name, pointForHomogeneousMovement.x, pointForHomogeneousMovement.y, pointForHomogeneousMovement.z, num));
		pointForHomogeneousMovement = LinearInterpolator.GetPosition(pointForHomogeneousMovement, num);
		Quaternion quaternion = RotationAtDistance(num);
		Vector3 position = pointForHomogeneousMovement - quaternion * _contactPoint;
		base.transform.SetPositionAndRotation(position, quaternion);
	}

	private void _E003()
	{
		float num = Locomotive.MovementCurve.keys.Max((Keyframe x) => x.value) + Shift;
		float currentTime = 0f;
		Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(0f, num, Precision, ref currentTime);
		Debug.Log(string.Format(_ED3E._E000(209229), base.gameObject.name, pointForHomogeneousMovement.x, pointForHomogeneousMovement.y, pointForHomogeneousMovement.z, num));
		pointForHomogeneousMovement = LinearInterpolator.GetPosition(pointForHomogeneousMovement, num);
		Quaternion quaternion = RotationAtDistance(num);
		Vector3 position = pointForHomogeneousMovement - quaternion * _contactPoint;
		base.transform.SetPositionAndRotation(position, quaternion);
	}
}
