using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Game.Spawning;
using EFT.Interactive;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BotZone : MonoBehaviour
{
	public const float MIN_BORN_CHECK_TIME = 2f;

	public const float MAX_BORN_CHECK_TIME = 60f;

	public float DistanceCoef = 1f;

	public int PoolSize = 10;

	public int Id;

	[CompilerGenerated]
	private int m__E000 = 9999;

	[CompilerGenerated]
	private bool m__E001;

	[FormerlySerializedAs("MaxPersonsOnPatrol")]
	[SerializeField]
	private int _maxPersonsOnPatrol;

	public bool CanSpawnBoss;

	public bool CachePathLength;

	public bool SnipeZone;

	public bool DoDownToEarthPoints;

	public CustomNavigationPoint[] CoverPoints;

	public CustomNavigationPoint[] AmbushPoints;

	public CustomNavigationPoint[] BushPoints;

	public AIPlaceInfo[] AllPlaces;

	public ZoneTriangleData ZoneTriangleData;

	public PatrolWay[] PatrolWays;

	public List<SpawnPointMarker> SpawnPointMarkers;

	public UnspawnPoint[] UnSpawnPoints;

	private ISpawnPoint[] m__E002;

	private float m__E003 = 60f;

	public BotZone[] NeightbourZones;

	private readonly Stack<CustomNavigationPoint[]> m__E004 = new Stack<CustomNavigationPoint[]>();

	private readonly Stack<CustomNavigationPoint[]> m__E005 = new Stack<CustomNavigationPoint[]>();

	private Door[] m__E006;

	public bool DrawSidesAmbush;

	public bool DrawOnlyInPlaces;

	public bool DrawSidesCover;

	public float MinDefenceLevelToDraw;

	public float DistDrawCover = 50f;

	public float DistDrawAmbush = 50f;

	public float DistDrawBush = 50f;

	public BotLocationModifier Modifier;

	[CompilerGenerated]
	private BoxCollider[] m__E007;

	public BotZoneManualInfo ZoneManualInfo;

	public BotZoneStationaryWeapons StationaryWeapons;

	public BotZonePatrolData ZonePatrolData;

	public BotZoneNavMeshCutter ZoneNavMeshCutters;

	public BotZoneEntranceInfo EntranceInfo;

	[CompilerGenerated]
	private _E111 m__E008;

	[CompilerGenerated]
	private string m__E009;

	public List<BotPointOfInterest> PointsOfInterest = new List<BotPointOfInterest>();

	[HideInInspector]
	public string CoverPointCreatorPresetName;

	public int GizmosMaxDangerLevel
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public bool AlwaysDrawGizmos
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		set
		{
			this.m__E001 = value;
		}
	}

	public int MaxPersonsOnPatrol
	{
		get
		{
			if (DebugBotData.UseDebugData && DebugBotData.Instance.IgnoreBotLimits)
			{
				return int.MaxValue;
			}
			return _maxPersonsOnPatrol;
		}
		set
		{
			_maxPersonsOnPatrol = value;
		}
	}

	public ISpawnPoint[] SpawnPoints => this.m__E002 ?? (this.m__E002 = SpawnPointMarkers?.Select((SpawnPointMarker marker) => marker.SpawnPoint).ToArray() ?? _EBCA.Empty);

	public BoxCollider[] AllBounds
	{
		[CompilerGenerated]
		get
		{
			return this.m__E007;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E007 = value;
		}
	}

	public string NameZone => base.name;

	public _E111 ZoneDangerAreas
	{
		[CompilerGenerated]
		get
		{
			return this.m__E008;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E008 = value;
		}
	}

	public string ShortName
	{
		[CompilerGenerated]
		get
		{
			return this.m__E009;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E009 = value;
		}
	}

	private void Awake()
	{
		CheckBounds();
		_E002();
	}

	public void Init(BotLocationModifier modifier)
	{
		if (BushPoints == null)
		{
			BushPoints = new CustomNavigationPoint[0];
		}
		if (EntranceInfo != null)
		{
			EntranceInfo.Init();
		}
		ShortName = base.name;
		ShortName = ShortName.Replace(_ED3E._E000(13881), "");
		ShortName = ShortName.Replace(_ED3E._E000(13877), "");
		if (ShortName.Length < 2)
		{
			ShortName = base.name;
		}
		else
		{
			int value = Mathf.Min(10, ShortName.Length);
			value = Mathf.Clamp(value, 1, 20);
			ShortName = ShortName.Substring(0, value);
			if (ShortName.Length == 0)
			{
				ShortName = base.name;
			}
		}
		if (SpawnPoints != null)
		{
			_ = SpawnPoints.LongLength;
		}
		ZoneDangerAreas = new _E111();
		if (StationaryWeapons != null)
		{
			StationaryWeapons.Init();
		}
		if (ZoneManualInfo != null)
		{
			ZoneManualInfo.Init();
			List<CustomNavigationPoint> list = CoverPoints.ToList();
			List<CustomNavigationPoint> list2 = AmbushPoints.ToList();
			foreach (CustomNavigationPoint point in ZoneManualInfo.Points)
			{
				switch (point.StrategyType)
				{
				case PointWithNeighborType.cover:
					list.Add(point);
					break;
				case PointWithNeighborType.ambush:
					list2.Add(point);
					break;
				case PointWithNeighborType.both:
					list.Add(point);
					list2.Add(point);
					break;
				}
			}
			CoverPoints = list.ToArray();
			AmbushPoints = list2.ToArray();
		}
		if (ZonePatrolData != null)
		{
			ZonePatrolData.Init(ZoneDangerAreas, this);
		}
		_E079.CheckNeighbourhoods(CoverPoints, AmbushPoints);
		_E000();
		string mapName = base.gameObject.scene.name;
		if (CachePathLength)
		{
			ZoneTriangleData = new ZoneTriangleData(base.name, mapName);
		}
		Modifier = modifier;
		_E003(modifier.MagnetPower);
		if (SpawnPointMarkers != null && SpawnPointMarkers.Count > 0)
		{
			foreach (SpawnPointMarker spawnPointMarker in SpawnPointMarkers)
			{
				spawnPointMarker.BotZone = this;
				if (!_E001(spawnPointMarker.transform.position))
				{
					spawnPointMarker.transform.Ground();
				}
			}
			this.m__E003 = Mathf.Clamp(SpawnPointMarkers.Sum((SpawnPointMarker marker) => marker.SpawnPoint.DelayToCanSpawnSec) / (float)SpawnPointMarkers.Count, 2f, 60f);
		}
		PatrolWay[] patrolWays = PatrolWays;
		for (int i = 0; i < patrolWays.Length; i++)
		{
			patrolWays[i].InitPoints();
		}
		foreach (BotPointOfInterest item in PointsOfInterest)
		{
			item.Init(modifier);
		}
		StartCoroutine(_E006());
	}

	private void _E000()
	{
		List<CustomNavigationPoint> list = CoverPoints.ToList().Concat(AmbushPoints).Concat(BushPoints)
			.ToList();
		foreach (CustomNavigationPoint item in list)
		{
			item.UpdateFromSerializable();
		}
		foreach (CustomNavigationPoint item2 in list)
		{
			item2.UpdateCoversFromIds(list);
		}
	}

	private static bool _E001(Vector3 position)
	{
		NavMesh.SamplePosition(position, out var hit, 0.2f, -1);
		return hit.hit;
	}

	private void _E002()
	{
		_E005(base.gameObject);
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.GetComponent<SpawnPointMarker>() == null)
			{
				_E005(item.gameObject);
			}
		}
	}

	private void _E003(float magnetPower)
	{
		if (!(magnetPower > 0f))
		{
			return;
		}
		int num = _E39D.RandomInclude(_E2A0.Core.CORE_POINTS_MIN, _E2A0.Core.CORE_POINTS_MAX);
		if (num <= 0)
		{
			return;
		}
		List<PatrolPoint> list = new List<PatrolPoint>();
		foreach (PatrolWay item in PatrolWays.Where((PatrolWay x) => x.PatrolType == PatrolType.patrolling))
		{
			list.AddRange(item.Points);
		}
		if (list.Count <= num)
		{
			return;
		}
		foreach (PatrolPoint item2 in list.RandomElement(num))
		{
			_E004(CoverPoints, item2, magnetPower);
			_E004(AmbushPoints, item2, magnetPower);
		}
	}

	private void _E004(CustomNavigationPoint[] points2check, PatrolPoint corePoint, float maxPower)
	{
		float num = float.MinValue;
		CustomNavigationPoint[] array = points2check;
		foreach (CustomNavigationPoint customNavigationPoint in array)
		{
			float magnitude = (customNavigationPoint.Position - corePoint.transform.position).magnitude;
			magnitude = ((!(magnitude > maxPower)) ? (maxPower - magnitude) : 0f);
			customNavigationPoint.BaseWeight += magnitude;
			if (customNavigationPoint.BaseWeight > num)
			{
				num = customNavigationPoint.BaseWeight;
			}
		}
		array = points2check;
		foreach (CustomNavigationPoint customNavigationPoint2 in array)
		{
			customNavigationPoint2.BaseWeight = ((num > 0f) ? (1f + customNavigationPoint2.BaseWeight / num) : 1f);
		}
	}

	private void _E005(GameObject obj)
	{
		Collider component = obj.GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public bool IsValid()
	{
		if (SpawnPointMarkers.Count > 0 && CoverPoints != null && CoverPoints.Length != 0)
		{
			return PatrolWays.Length != 0;
		}
		return false;
	}

	public float GetMiddleTime()
	{
		return this.m__E003;
	}

	public CustomNavigationPoint[] GetCoverPoints()
	{
		if (this.m__E004.Count <= 0)
		{
			return _E009(CoverPoints);
		}
		return this.m__E004.Pop();
	}

	public CustomNavigationPoint[] GetAmbushPoints()
	{
		if (this.m__E005.Count <= 0)
		{
			return _E009(AmbushPoints);
		}
		return this.m__E005.Pop();
	}

	public CustomNavigationPoint[] GetBushPoints()
	{
		return _E009(BushPoints);
	}

	private IEnumerator _E006()
	{
		CustomNavigationPoint[] array = new CustomNavigationPoint[CoverPoints.Length];
		int num = array.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			array[i] = CustomNavigationPoint.Copy(CoverPoints[i]);
			if (i % 40 == 0)
			{
				yield return null;
			}
			if (i == num - 1)
			{
				this.m__E004.Push(array);
				int num3 = num2 + 1;
				num2 = num3;
				if (num3 > PoolSize)
				{
					break;
				}
				array = new CustomNavigationPoint[CoverPoints.Length];
				i = -1;
			}
		}
	}

	private IEnumerator _E007()
	{
		CustomNavigationPoint[] array = new CustomNavigationPoint[AmbushPoints.Length];
		int num = array.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			array[i] = CustomNavigationPoint.Copy(AmbushPoints[i]);
			if (i % 40 == 0)
			{
				yield return null;
			}
			if (i == num - 1)
			{
				this.m__E005.Push(array);
				int num3 = num2 + 1;
				num2 = num3;
				if (num3 > PoolSize)
				{
					break;
				}
				array = new CustomNavigationPoint[AmbushPoints.Length];
				i = -1;
			}
		}
	}

	private List<CustomNavigationPoint> _E008(Vector3 p, float dist, CustomNavigationPoint[] testedList)
	{
		List<CustomNavigationPoint> list = new List<CustomNavigationPoint>();
		float num = dist * dist;
		foreach (CustomNavigationPoint customNavigationPoint in testedList)
		{
			if ((customNavigationPoint.Position - p).sqrMagnitude < num)
			{
				list.Add(customNavigationPoint);
			}
		}
		return list;
	}

	public List<CustomNavigationPoint> GetCoverPointsInRadius(Vector3 p, float dist)
	{
		return _E008(p, dist, CoverPoints);
	}

	public List<CustomNavigationPoint> GetAmbushPointsInRadius(Vector3 p, float dist)
	{
		return _E008(p, dist, AmbushPoints);
	}

	private CustomNavigationPoint[] _E009(CustomNavigationPoint[] points)
	{
		CustomNavigationPoint[] array = new CustomNavigationPoint[points.Length];
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = CustomNavigationPoint.Copy(points[i]);
		}
		return array;
	}

	public List<Transform> GetAndCalcReachobjects()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			if (item.name.Contains(_ED3E._E000(13874)))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<BoxCollider> GetAllBounds(bool onlyActive)
	{
		List<BoxCollider> list = new List<BoxCollider>();
		foreach (Transform item in _E39C.GetChildsName(base.transform, _ED3E._E000(13871), onlyActive))
		{
			BoxCollider component = item.GetComponent<BoxCollider>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	public void DrawBaseWeight()
	{
		float a = 0f;
		if (CoverPoints.Length != 0)
		{
			a = CoverPoints.Max((CustomNavigationPoint x) => x.BaseWeight);
		}
		float b = 0f;
		if (AmbushPoints.Length != 0)
		{
			b = AmbushPoints.Max((CustomNavigationPoint x) => x.BaseWeight);
		}
		Mathf.Max(a, b);
	}

	public void OnDrawGizmos()
	{
		if (_E366.DrawBounds)
		{
			foreach (BoxCollider allBound in GetAllBounds(onlyActive: false))
			{
				_E00A(allBound);
			}
		}
		if (AlwaysDrawGizmos && !Application.isPlaying)
		{
			_E00D();
			_E00E();
			_E00B();
			_E00C();
		}
	}

	private void _E00A(BoxCollider boxCollider)
	{
		Gizmos.color = Color.yellow;
		Vector3 size = boxCollider.size;
		Vector3 localScale = boxCollider.transform.localScale;
		Gizmos.matrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(size: new Vector3(size.x * localScale.x, size.y * localScale.y, size.z * localScale.z), center: Vector3.zero);
		Gizmos.matrix = Matrix4x4.identity;
	}

	public void OnDrawGizmosSelected()
	{
		if (!Application.isPlaying)
		{
			_E00D();
			_E00E();
			_E00B();
			_E00C();
		}
		else if (ZoneDangerAreas != null)
		{
			ZoneDangerAreas.OnDrawGizmosSelected();
		}
	}

	private void _E00B()
	{
		Vector3 position = Camera.current.transform.position;
		float sDist = DistDrawAmbush * DistDrawAmbush;
		CustomNavigationPoint[] ambushPoints = AmbushPoints;
		foreach (CustomNavigationPoint customNavigationPoint in ambushPoints)
		{
			if ((!DrawOnlyInPlaces || customNavigationPoint.PlaceId <= 0) && customNavigationPoint.CovPointsPlaceSerializable.DefenceInfo.DangerCoeff <= GizmosMaxDangerLevel)
			{
				customNavigationPoint.OnDrawGizmosAsAmbush(position, sDist, DrawSidesAmbush);
			}
		}
	}

	private void _E00C()
	{
		Vector3 position = Camera.current.transform.position;
		float sDist = DistDrawBush * DistDrawBush;
		if (BushPoints != null)
		{
			CustomNavigationPoint[] bushPoints = BushPoints;
			for (int i = 0; i < bushPoints.Length; i++)
			{
				bushPoints[i].OnDrawGizmosAsAmbush(position, sDist, DrawSidesAmbush);
			}
		}
	}

	private void _E00D()
	{
		if (MinDefenceLevelToDraw <= 0f)
		{
			return;
		}
		int num = 15;
		Gizmos.color = new Color(0.9f, 0.2f, 0.2f, 0.5f);
		foreach (CustomNavigationPoint item in CoverPoints.Concat(AmbushPoints))
		{
			if (item.CovPointsPlaceSerializable.DefenceLevel > MinDefenceLevelToDraw)
			{
				_E395.DrawCube(item.Position + Vector3.up * num * 0.5f, base.transform.rotation, new Vector3(1f, num, 1f));
			}
		}
	}

	private void _E00E()
	{
		if (CoverPoints == null || CoverPoints.Length == 0)
		{
			return;
		}
		Vector3 position = Camera.current.transform.position;
		float sDist = DistDrawCover * DistDrawCover;
		CustomNavigationPoint[] coverPoints = CoverPoints;
		foreach (CustomNavigationPoint customNavigationPoint in coverPoints)
		{
			if ((!DrawOnlyInPlaces || customNavigationPoint.PlaceId <= 0) && customNavigationPoint.CovPointsPlaceSerializable.DefenceInfo.DangerCoeff <= GizmosMaxDangerLevel)
			{
				customNavigationPoint.OnDrawGizmosFullAsCover(position, sDist, DrawSidesCover);
			}
		}
	}

	public bool CheckBounds()
	{
		AllBounds = GetAllBounds(onlyActive: false).ToArray();
		for (int i = 0; i < AllBounds.Length; i++)
		{
			BoxCollider boxCollider = AllBounds[i];
			if (boxCollider.enabled)
			{
				Debug.LogError(_ED3E._E000(13862) + boxCollider.gameObject.name);
				boxCollider.enabled = false;
			}
		}
		return true;
	}

	public void ClearPools()
	{
		this.m__E004.Clear();
		this.m__E005.Clear();
	}
}
