using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Game.Spawning;
using UnityEngine;

namespace EFT.Interactive;

[ExecuteInEditMode]
public class Location : MonoBehaviourSingleton<Location>
{
	public string SceneId;

	[CompilerGenerated]
	private LootPoint[] m__E000;

	[CompilerGenerated]
	private LootPoint[] _E001;

	[CompilerGenerated]
	private LootPoint[] _E002;

	[CompilerGenerated]
	private LootPoint[] _E003;

	[CompilerGenerated]
	private LootableContainer[] _E004;

	[CompilerGenerated]
	private LootableContainer[] _E005;

	[CompilerGenerated]
	private LootableContainer[] _E006;

	[CompilerGenerated]
	private LootableContainer[] _E007;

	[CompilerGenerated]
	private SpawnPointMarker[] _E008;

	[CompilerGenerated]
	private StationaryWeapon[] _E009;

	[CompilerGenerated]
	private _E5E2[] _E00A;

	public LootPoint[] ValidLootPoints
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public LootPoint[] EmptyLootPoints
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		private set
		{
			_E001 = value;
		}
	}

	public LootPoint[] DisabledLootPoints
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public LootPoint[] LootPoints
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	public LootableContainer[] ValidLootableContainers
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		private set
		{
			_E004 = value;
		}
	}

	public LootableContainer[] EmptyLootableContainers
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public LootableContainer[] DisabledLootableContainers
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		private set
		{
			_E006 = value;
		}
	}

	public LootableContainer[] LootableContainers
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		private set
		{
			_E007 = value;
		}
	}

	public SpawnPointMarker[] SpawnPointMarkers
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
		[CompilerGenerated]
		private set
		{
			_E008 = value;
		}
	}

	public StationaryWeapon[] Stationary
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		private set
		{
			_E009 = value;
		}
	}

	public _E5E2[] DailyQuestZones
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		private set
		{
			_E00A = value;
		}
	}

	public void CollectLootPointsData()
	{
		Stationary = Object.FindObjectsOfType<StationaryWeapon>();
		SpawnPointMarkers = Object.FindObjectsOfType<SpawnPointMarker>();
		LootPoints = Object.FindObjectsOfType<LootPoint>();
		EmptyLootPoints = LootPoints.Where((LootPoint point) => point.SelectedFilters.Length == 0).ToArray();
		DisabledLootPoints = LootPoints.Where((LootPoint point) => !point.Enabled).ToArray();
		ValidLootPoints = LootPoints.Except(EmptyLootPoints).Except(DisabledLootPoints).ToArray();
		LootableContainers = Object.FindObjectsOfType<LootableContainer>();
		EmptyLootableContainers = LootableContainers.Where((LootableContainer x) => string.IsNullOrEmpty(x.Template)).ToArray();
		DisabledLootableContainers = LootableContainers.Where((LootableContainer x) => !x.enabled).ToArray();
		ValidLootableContainers = LootableContainers.Except(EmptyLootableContainers).Except(DisabledLootableContainers).ToArray();
		DailyQuestZones = (from x in (from x in Object.FindObjectsOfType<TriggerWithId>()
				where _E5E2.IsDailyQuestZone(x.name)
				select x).DistinctBy((TriggerWithId x) => x.gameObject.GetInstanceID())
			select new _E5E2(x.name, SceneId)).ToArray();
	}

	[CompilerGenerated]
	private _E5E2 _E000(TriggerWithId x)
	{
		return new _E5E2(x.name, SceneId);
	}
}
