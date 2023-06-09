using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT;
using UnityEngine;

[Serializable]
public class CustomNavigationPoint : IPositionPoint, IPointForNet
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public float sDistToEnemy;

		public Vector3 dirEnemy;

		public CustomNavigationPoint _003C_003E4__this;

		internal bool _E000()
		{
			if (sDistToEnemy <= 0.1f)
			{
				return true;
			}
			return _E39C.IsAngLessNormalized(_E39C.NormalizeFastSelf(dirEnemy), _003C_003E4__this.ToWallVector, 0.1736482f);
		}
	}

	public const int BASE_HIDE_VAL = 51;

	public const float LIGHT_WALL_ANG = 57f;

	public const float MAX_DEFENCE_LEVEL_SIDE = 8f;

	public const int MAX_HIDE_VAL = 100;

	[SerializeField]
	private Vector3 _cachedPosition;

	public int Id;

	public Vector3 AltPosition;

	public bool HaveAltPosition;

	public Vector3 BasePosition;

	public CoverPointPlaceSerializable CovPointsPlaceSerializable;

	public _E081 CovPointsPlace;

	public Vector3 ToWallVector;

	public Vector3 FirePosition;

	public BotTiltType TiltType;

	public CoverLevel CoverLevel;

	public bool AlwaysGood;

	public bool DrawSign;

	public bool BordersLightHave;

	public Vector3 LeftBorderLight;

	public Vector3 RightBorderLight;

	[HideInInspector]
	public bool CanIShootToEnemy;

	[HideInInspector]
	public bool lastCanShoot;

	public bool CanLookLeft;

	public bool CanLookRight;

	public int HideLevel = 51;

	public int PlaceId = -1;

	public PointWithNeighborType StrategyType;

	public CoverPointCreatorPreset coverPointCreatorPreset;

	private float _startBaseWeight = 1f;

	private bool _isSpotted;

	private bool _blocked;

	private float _spottedTime;

	private bool _isGoodInsideBuilding;

	private float _coveringWeight;

	private float _unSpottedTime;

	private float _decreasedWeightCoef = 1f;

	private float _nextCheckCanShootTime;

	public Vector3 Position => _cachedPosition;

	public bool IsGoodInsideBuilding
	{
		get
		{
			return _isGoodInsideBuilding;
		}
		set
		{
			_isGoodInsideBuilding = value;
		}
	}

	public float CoveringWeight
	{
		get
		{
			return _coveringWeight;
		}
		private set
		{
			_coveringWeight = value;
		}
	}

	public bool IsSpotted
	{
		get
		{
			if (_blocked)
			{
				return false;
			}
			if (_isSpotted)
			{
				if (_unSpottedTime < Time.time)
				{
					_isSpotted = false;
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public float BaseWeight
	{
		get
		{
			return _startBaseWeight;
		}
		set
		{
			if (value <= 1f)
			{
				_startBaseWeight = 1f;
			}
			else
			{
				_startBaseWeight = value;
			}
		}
	}

	public ECoverPointSpecial Special => CovPointsPlace.Special;

	public int EnvironmentId => CovPointsPlace.IdEnvironment;

	public static CustomNavigationPoint Copy(CustomNavigationPoint group)
	{
		try
		{
			new CustomNavigationPoint(group.Id, group.Position, group.AltPosition, group.ToWallVector, group.FirePosition, group.CoverLevel, group.AlwaysGood, group.StrategyType, group.CovPointsPlace.DefenceInfo, group.PlaceId, group.IsGoodInsideBuilding, withInit: false);
		}
		catch (Exception)
		{
		}
		CustomNavigationPoint customNavigationPoint = new CustomNavigationPoint(group.Id, group.Position, group.AltPosition, group.ToWallVector, group.FirePosition, group.CoverLevel, group.AlwaysGood, group.StrategyType, group.CovPointsPlace.DefenceInfo, group.PlaceId, group.IsGoodInsideBuilding, withInit: false);
		customNavigationPoint.CovPointsPlace = group.CovPointsPlace;
		customNavigationPoint.BaseWeight = group.BaseWeight;
		if (!customNavigationPoint.AlwaysGood && customNavigationPoint.ToWallVector == Vector3.zero)
		{
			Debug.LogError(string.Concat(_ED3E._E000(31345), customNavigationPoint.Position, _ED3E._E000(31382), customNavigationPoint.Id));
		}
		customNavigationPoint.PlaceId = group.PlaceId;
		customNavigationPoint.BordersLightHave = group.BordersLightHave;
		customNavigationPoint.LeftBorderLight = group.LeftBorderLight;
		customNavigationPoint.RightBorderLight = group.RightBorderLight;
		customNavigationPoint.TiltType = group.TiltType;
		customNavigationPoint.CanLookLeft = group.CanLookLeft;
		customNavigationPoint.CanLookRight = group.CanLookRight;
		return customNavigationPoint;
	}

	public CustomNavigationPoint(int name, Vector3 position, Vector3? altPosition, Vector3 toWallVector, Vector3 firePosition, CoverLevel coverLevel, bool alwaysGood, PointWithNeighborType type, CoverPointDefenceInfo defenceInfo, int placeInfo, bool isGoodInsideBuilding, bool withInit = true)
	{
		Id = name;
		IsGoodInsideBuilding = isGoodInsideBuilding;
		PlaceId = placeInfo;
		BasePosition = position;
		_cachedPosition = BasePosition;
		HaveAltPosition = false;
		if (altPosition.HasValue && altPosition.Value != Vector3.zero)
		{
			HaveAltPosition = true;
			AltPosition = altPosition.Value;
		}
		FirePosition = firePosition;
		CoverLevel = coverLevel;
		AlwaysGood = alwaysGood;
		ToWallVector = toWallVector.normalized;
		StrategyType = type;
		if (withInit)
		{
			InitLightBorders();
			_E001();
			CovPointsPlaceSerializable = new CoverPointPlaceSerializable(Position, defenceInfo, CoverType.Wall, IsGoodInsideBuilding);
			_E002();
		}
	}

	public void SetWeight(float v, bool withBaseWeight = true)
	{
		if (withBaseWeight)
		{
			v *= BaseWeight;
		}
		CoveringWeight = v * _decreasedWeightCoef;
	}

	public void SetClose()
	{
		_cachedPosition = BasePosition;
	}

	public void SetLong()
	{
		if (HaveAltPosition)
		{
			_cachedPosition = AltPosition;
		}
	}

	public void UpdateCoversFromIds(List<CustomNavigationPoint> allPoints)
	{
		CovPointsPlace.UpdateCoversFromIds(allPoints);
	}

	public void UpdateFromSerializable()
	{
		CovPointsPlace = new _E081(CovPointsPlaceSerializable);
	}

	public void InitLightBorders()
	{
		if (ToWallVector.sqrMagnitude > 0f)
		{
			LeftBorderLight = _E39C.RotateOnAngUp(ToWallVector, 57f);
			RightBorderLight = _E39C.RotateOnAngUp(ToWallVector, -57f);
			BordersLightHave = true;
			LeftBorderLight = _E39C.NormalizeFastSelf(LeftBorderLight);
			RightBorderLight = _E39C.NormalizeFastSelf(RightBorderLight);
		}
		else
		{
			BordersLightHave = false;
		}
	}

	public void Block()
	{
		_blocked = true;
	}

	public void Unblock()
	{
		_blocked = false;
	}

	public void Spotted(float period)
	{
		if (_isSpotted)
		{
			float a = Time.time + period;
			_unSpottedTime = Mathf.Max(a, _unSpottedTime);
		}
		else
		{
			_isSpotted = true;
			_unSpottedTime = Time.time + period;
		}
	}

	public bool IsDangerPositionFarEnough(HashSet<Vector3> positionsIMustCare, float minSDistToEnemy)
	{
		foreach (Vector3 item in positionsIMustCare)
		{
			if (!((item - Position).sqrMagnitude < minSDistToEnemy))
			{
				continue;
			}
			return false;
		}
		return true;
	}

	public bool CanIHide(HashSet<Vector3> positionsIMustCare, float minSDistToEnemy, bool useRaycast, bool useAng = true)
	{
		foreach (Vector3 item in positionsIMustCare)
		{
			if (CanIHideFromPos(minSDistToEnemy, useRaycast, useAng, item))
			{
				continue;
			}
			return false;
		}
		return true;
	}

	public bool CanIHideFromPos(float minSDistToEnemy, bool useRaycast, bool useAng, Vector3 pos)
	{
		Vector3 vector = pos + BotOwner.STAY_HEIGHT;
		Vector3 vector2 = Position + BotOwner.STAY_HEIGHT;
		Vector3 dirEnemy = vector - vector2;
		float sDistToEnemy = dirEnemy.sqrMagnitude;
		if (sDistToEnemy < minSDistToEnemy)
		{
			return false;
		}
		Func<bool> func = () => sDistToEnemy <= 0.1f || _E39C.IsAngLessNormalized(_E39C.NormalizeFastSelf(dirEnemy), ToWallVector, 0.1736482f);
		if (!((useAng && useRaycast) ? (func() && Physics.Linecast(vector2, vector, _E37B.HighPolyWithTerrainMask)) : (useRaycast ? Physics.Linecast(vector2, vector, _E37B.HighPolyWithTerrainMask) : (!useAng || func()))))
		{
			return false;
		}
		return true;
	}

	public bool CanShootToTargetCast(BotOwner shooter, float deltaLastTimeVision)
	{
		_E1F0 goalEnemy = shooter.Memory.GoalEnemy;
		if (goalEnemy.CanShoot && goalEnemy.IsVisible)
		{
			return true;
		}
		_E07F obj = shooter.CurrentEnemyTargetPosition(sensPosition: false);
		if (obj == null)
		{
			return false;
		}
		_ = obj.Point;
		bool flag = lastCanShoot;
		if (_nextCheckCanShootTime < Time.time)
		{
			if (Time.time - goalEnemy.TimeLastSeen > deltaLastTimeVision)
			{
				flag = false;
			}
			else
			{
				float num = 2f;
				_nextCheckCanShootTime = Time.time + num;
				flag = _E079.CanShootToTarget(obj, this, shooter.LookSensor.Mask);
			}
		}
		lastCanShoot = flag;
		return lastCanShoot;
	}

	public void SetDecreasedWeight(bool val)
	{
		_decreasedWeightCoef = (val ? _E2A0.Core.MIDDLE_POINT_COEF : 1f);
	}

	public void DrawSides()
	{
		Vector3 up = Vector3.up;
		Gizmos.color = Color.yellow;
		if (BordersLightHave)
		{
			if (CanLookLeft)
			{
				Gizmos.DrawRay(Position + up, LeftBorderLight);
			}
			if (CanLookRight)
			{
				Gizmos.DrawRay(Position + up, RightBorderLight);
			}
		}
	}

	public void OnDrawGizmosAsAmbush(Vector3? cameraPos = null, float sDist = 0f, bool drawSides = true)
	{
		if (!cameraPos.HasValue || !((Position - cameraPos.Value).sqrMagnitude > sDist))
		{
			_E000();
			_E004();
			Vector3 up = Vector3.up;
			Gizmos.color = new Color(0.1f, 0.2f, 0.7f);
			Gizmos.DrawLine(Position, Position + up);
			if (HaveAltPosition)
			{
				Gizmos.DrawLine(AltPosition, AltPosition + up);
			}
			_E005();
			Gizmos.color = new Color(0.7f, 0.2f, 0.2f);
			int num = 6;
			Gizmos.DrawLine(Position + Vector3.right / num, Position + up + Vector3.right / num);
			Gizmos.DrawLine(Position + Vector3.left / num, Position + up + Vector3.left / num);
			Gizmos.DrawLine(Position + Vector3.back / num, Position + up + Vector3.back / num);
			Gizmos.DrawLine(Position + Vector3.forward / num, Position + up + Vector3.forward / num);
			Gizmos.color = new Color(0.5f, 1f, 0.2f);
			Vector3 from = Position + up;
			Vector3 to = Position + ToWallVector.normalized * 0.8f + up;
			Gizmos.DrawLine(from, to);
			Color color = new Color(0.9f, 0.5f, 0.1f);
			switch (CoverLevel)
			{
			case CoverLevel.Sit:
				color = new Color(0.5f, 0.1f, 0.9f);
				break;
			case CoverLevel.Lay:
				color = new Color(0.1f, 0.9f, 0.3f);
				break;
			}
			Gizmos.color = color;
			Gizmos.DrawSphere(Position + up, 1f / 6f);
			if (drawSides)
			{
				DrawSides();
			}
		}
	}

	public void OnDrawGizmosFullAsCover(Vector3? cameraPos = null, float sDist = 0f, bool drawSides = true)
	{
		if (cameraPos.HasValue && sDist > -1f && (Position - cameraPos.Value).sqrMagnitude > sDist)
		{
			return;
		}
		if (DrawSign)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(Position + Vector3.up * 2f, Vector3.one / 2f);
			Gizmos.DrawWireCube(Position + Vector3.up * 4f, Vector3.one / 2f);
			Gizmos.DrawWireCube(Position + Vector3.up * 6f, Vector3.one / 2f);
		}
		_E000();
		_E005();
		if (StrategyType == PointWithNeighborType.both)
		{
			Gizmos.color = new Color(1f, 0.4f, 0f);
			Gizmos.DrawWireCube(Position + Vector3.up * 0.5f, Vector3.one / 3f);
		}
		_E004();
		Vector3 up = Vector3.up;
		if (HaveAltPosition)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(AltPosition, AltPosition + up);
		}
		if (drawSides)
		{
			DrawSides();
		}
		if (AlwaysGood)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(Position, Position + up);
			Gizmos.color = Color.red;
			int num = 2;
			Gizmos.DrawLine(Position + up, Position + up + Vector3.right / num);
			Gizmos.DrawLine(Position + up, Position + up + Vector3.left / num);
			Gizmos.DrawLine(Position + up, Position + up + Vector3.back / num);
			Gizmos.DrawLine(Position + up, Position + up + Vector3.forward / num);
		}
		else
		{
			if (!(ToWallVector != Vector3.zero))
			{
				return;
			}
			float num2 = 0f;
			switch (CoverLevel)
			{
			case CoverLevel.Lay:
				num2 = 0.5f;
				break;
			case CoverLevel.Sit:
				num2 = 1f;
				break;
			case CoverLevel.Stay:
				num2 = 1.7f;
				break;
			}
			up = Vector3.up * num2;
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(Position, Position + up);
			Gizmos.color = Color.red;
			Vector3 vector = Position + up;
			Vector3 vector2 = Position + ToWallVector.normalized * 0.8f + up;
			Gizmos.DrawLine(vector, vector2);
			Vector3 b = vector2 - vector;
			Vector3 normalized = _E39C.RotateOnAngUp(b, 13f).normalized;
			Vector3 normalized2 = _E39C.RotateOnAngUp(b, -13f).normalized;
			Gizmos.DrawLine(vector, vector + normalized);
			Gizmos.DrawLine(vector, vector + normalized2);
			if (!(FirePosition != Vector3.zero))
			{
				return;
			}
			Vector3 firePosition = FirePosition;
			switch (CoverLevel)
			{
			case CoverLevel.Lay:
			{
				Gizmos.color = Color.cyan;
				Vector3 vector3 = Position + up;
				Gizmos.DrawLine(vector3, vector3 + ToWallVector.normalized * 0.8f);
				break;
			}
			case CoverLevel.Sit:
			{
				Vector3 vector4 = Position + up;
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(vector4, vector4 + ToWallVector.normalized * 0.8f);
				break;
			}
			case CoverLevel.Stay:
			{
				Vector3 position = Position;
				Gizmos.color = Color.yellow;
				position.y = firePosition.y;
				if (firePosition.x != 0f || firePosition.z != 0f)
				{
					Gizmos.DrawLine(firePosition, position);
					Gizmos.color = Color.green;
					Gizmos.DrawLine(firePosition, firePosition + ToWallVector.normalized * 0.8f);
				}
				break;
			}
			}
		}
	}

	private void _E000()
	{
		Gizmos.color = Color.magenta;
		bool flag = CovPointsPlaceSerializable.IdEnvironment > 0 && CovPointsPlaceSerializable.EnvironmentType == EnvironmentType.Outdoor;
		bool flag2 = CovPointsPlaceSerializable.IdEnvironment == 0 && CovPointsPlaceSerializable.EnvironmentType == EnvironmentType.Indoor;
		if (CovPointsPlaceSerializable.IdEnvironment > 0)
		{
			Gizmos.DrawWireSphere(Position, 0.33f);
			if (flag)
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere(Position, 0.3f);
			if (flag2)
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere(Position, 0.23f);
		}
	}

	public void SetHideLevel(int lockCount)
	{
		HideLevel = lockCount;
	}

	private void _E001()
	{
		if (CoverLevel == CoverLevel.Stay)
		{
			Vector3 n = Position - FirePosition;
			n.y = 0f;
			Vector3 toWallVector = ToWallVector;
			toWallVector.y = 0f;
			Vector3 from = _E39D.Rotate90(n, 1);
			Vector3 from2 = _E39D.Rotate90(n, -1);
			float num = Vector3.Angle(from, toWallVector);
			float num2 = Vector3.Angle(from2, toWallVector);
			TiltType = ((!(num2 > num)) ? BotTiltType.right : BotTiltType.left);
		}
	}

	private void _E002()
	{
		Vector3 vector = _E39C.Rotate90(ToWallVector, _E39C.SideTurn.left);
		Vector3 vector2 = _E39C.Rotate90(ToWallVector, _E39C.SideTurn.right);
		Vector3 vector3 = Vector3.up * 0.8f;
		CanLookLeft = _E079.TestDir(Position + vector3, vector, _E2A0.Core.HOLD_MIN_LIGHT_DIST, out var outPos);
		CanLookRight = _E079.TestDir(Position + vector3, vector2, _E2A0.Core.HOLD_MIN_LIGHT_DIST, out var outPos2);
		if (CoverLevel == CoverLevel.Stay && StrategyType != PointWithNeighborType.ambush)
		{
			Vector3 rhs = FirePosition - Position;
			if (!CanLookLeft && Vector3.Dot(vector, rhs) > 0f)
			{
				CanLookLeft = true;
			}
			if (!CanLookRight && Vector3.Dot(vector2, rhs) > 0f)
			{
				CanLookRight = true;
			}
		}
		if (!CanLookLeft)
		{
			CanLookLeft = _E003(outPos);
		}
		if (!CanLookRight)
		{
			CanLookRight = _E003(outPos2);
		}
	}

	private bool _E003(Vector3? sidePos)
	{
		float num = 0.5f;
		float dist = 1.2f;
		if (sidePos.HasValue)
		{
			Vector3 value = sidePos.Value;
			value.y = Position.y;
			Vector3 v = Position - value;
			int num2 = (int)(v.magnitude / num);
			Vector3 sTAY_HEIGHT = BotOwner.STAY_HEIGHT;
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i * num;
				Vector3 vector = _E39C.NormalizeFastSelf(v);
				Vector3 vector2 = value + vector * num3;
				if (_E079.TestDir(sTAY_HEIGHT + vector2, ToWallVector, dist))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void _E004()
	{
		float t = (float)HideLevel / 100f;
		Gizmos.color = Color.Lerp(Color.red, Color.green, t);
		float num = 0.3f;
		Gizmos.DrawCube(Position + Vector3.up * 0.5f, new Vector3(num, num, num));
	}

	private void _E005()
	{
		if (IsGoodInsideBuilding)
		{
			Gizmos.color = new Color(0.1f, 1f, 0f);
			Gizmos.DrawWireCube(Position + Vector3.up * 0.5f, new Vector3(0.3f, 1f, 0.2f));
		}
	}
}
