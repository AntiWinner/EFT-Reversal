using System;
using System.Collections.Generic;
using Audio.SpatialSystem;
using EFT.Game.Spawning;
using EFT.Interactive;
using UnityEngine.Serialization;

namespace UnityEngine.AI;

[AddComponentMenu("Navigation/NavMeshSurface", 30)]
[DefaultExecutionOrder(-102)]
[ExecuteInEditMode]
[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
public class NavMeshSurface : MonoBehaviour
{
	[SerializeField]
	private int m_AgentTypeID;

	[SerializeField]
	private CollectObjects m_CollectObjects;

	[SerializeField]
	private Vector3 m_Size = new Vector3(10f, 10f, 10f);

	[SerializeField]
	private Vector3 m_Center = new Vector3(0f, 2f, 0f);

	[SerializeField]
	private LayerMask m_LayerMask = -1;

	[SerializeField]
	private NavMeshCollectGeometry m_UseGeometry;

	[SerializeField]
	private int m_DefaultArea;

	[SerializeField]
	private bool m_IgnoreNavMeshAgent = true;

	[SerializeField]
	private bool m_IgnoreNavMeshObstacle = true;

	[SerializeField]
	private bool m_IgnoreAudio = true;

	[SerializeField]
	private bool m_IgnoreSpawns = true;

	[SerializeField]
	private bool m_IgnoreQuests = true;

	[SerializeField]
	private bool m_OverrideTileSize;

	[SerializeField]
	private int m_TileSize = 256;

	[SerializeField]
	private bool m_OverrideVoxelSize;

	[SerializeField]
	private float m_VoxelSize;

	[SerializeField]
	private bool m_BuildHeightMesh;

	[SerializeField]
	[FormerlySerializedAs("m_BakedNavMeshData")]
	private NavMeshData m_NavMeshData;

	private NavMeshDataInstance m__E000;

	private Vector3 m__E001 = Vector3.zero;

	private Quaternion m__E002 = Quaternion.identity;

	private static readonly List<NavMeshSurface> m__E003 = new List<NavMeshSurface>();

	public int agentTypeID
	{
		get
		{
			return m_AgentTypeID;
		}
		set
		{
			m_AgentTypeID = value;
		}
	}

	public CollectObjects collectObjects
	{
		get
		{
			return m_CollectObjects;
		}
		set
		{
			m_CollectObjects = value;
		}
	}

	public Vector3 size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
		}
	}

	public Vector3 center
	{
		get
		{
			return m_Center;
		}
		set
		{
			m_Center = value;
		}
	}

	public LayerMask layerMask
	{
		get
		{
			return m_LayerMask;
		}
		set
		{
			m_LayerMask = value;
		}
	}

	public NavMeshCollectGeometry useGeometry
	{
		get
		{
			return m_UseGeometry;
		}
		set
		{
			m_UseGeometry = value;
		}
	}

	public int defaultArea
	{
		get
		{
			return m_DefaultArea;
		}
		set
		{
			m_DefaultArea = value;
		}
	}

	public bool ignoreNavMeshAgent
	{
		get
		{
			return m_IgnoreNavMeshAgent;
		}
		set
		{
			m_IgnoreNavMeshAgent = value;
		}
	}

	public bool ignoreNavMeshObstacle
	{
		get
		{
			return m_IgnoreNavMeshObstacle;
		}
		set
		{
			m_IgnoreNavMeshObstacle = value;
		}
	}

	public bool overrideTileSize
	{
		get
		{
			return m_OverrideTileSize;
		}
		set
		{
			m_OverrideTileSize = value;
		}
	}

	public int tileSize
	{
		get
		{
			return m_TileSize;
		}
		set
		{
			m_TileSize = value;
		}
	}

	public bool overrideVoxelSize
	{
		get
		{
			return m_OverrideVoxelSize;
		}
		set
		{
			m_OverrideVoxelSize = value;
		}
	}

	public float voxelSize
	{
		get
		{
			return m_VoxelSize;
		}
		set
		{
			m_VoxelSize = value;
		}
	}

	public bool buildHeightMesh
	{
		get
		{
			return m_BuildHeightMesh;
		}
		set
		{
			m_BuildHeightMesh = value;
		}
	}

	public NavMeshData navMeshData
	{
		get
		{
			return m_NavMeshData;
		}
		set
		{
			m_NavMeshData = value;
		}
	}

	public static List<NavMeshSurface> activeSurfaces => NavMeshSurface.m__E003;

	private void OnEnable()
	{
		_E000(this);
		AddData();
	}

	private void OnDisable()
	{
		RemoveData();
		_E001(this);
	}

	public void AddData()
	{
		if (!this.m__E000.valid)
		{
			if (m_NavMeshData != null)
			{
				this.m__E000 = NavMesh.AddNavMeshData(m_NavMeshData, base.transform.position, base.transform.rotation);
				this.m__E000.owner = this;
			}
			this.m__E001 = base.transform.position;
			this.m__E002 = base.transform.rotation;
		}
	}

	public void RemoveData()
	{
		this.m__E000.Remove();
		this.m__E000 = default(NavMeshDataInstance);
	}

	public NavMeshBuildSettings GetBuildSettings()
	{
		NavMeshBuildSettings settingsByID = NavMesh.GetSettingsByID(m_AgentTypeID);
		if (settingsByID.agentTypeID == -1)
		{
			Debug.LogWarning(_ED3E._E000(244758) + agentTypeID, this);
			settingsByID.agentTypeID = m_AgentTypeID;
		}
		if (overrideTileSize)
		{
			settingsByID.overrideTileSize = true;
			settingsByID.tileSize = tileSize;
		}
		if (overrideVoxelSize)
		{
			settingsByID.overrideVoxelSize = true;
			settingsByID.voxelSize = voxelSize;
		}
		return settingsByID;
	}

	public void BuildNavMesh()
	{
		List<NavMeshBuildSource> sources = _E004();
		Debug.Log(_ED3E._E000(244787));
		Bounds localBounds = new Bounds(m_Center, _E005(m_Size));
		if (m_CollectObjects == CollectObjects.All || m_CollectObjects == CollectObjects.Children)
		{
			localBounds = _E007(sources);
		}
		NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(GetBuildSettings(), sources, localBounds, base.transform.position, base.transform.rotation);
		if (navMeshData != null)
		{
			navMeshData.name = base.gameObject.name;
			RemoveData();
			m_NavMeshData = navMeshData;
			if (base.isActiveAndEnabled)
			{
				AddData();
			}
		}
	}

	public AsyncOperation UpdateNavMesh(NavMeshData data)
	{
		List<NavMeshBuildSource> sources = _E004();
		Bounds localBounds = new Bounds(m_Center, _E005(m_Size));
		if (m_CollectObjects == CollectObjects.All || m_CollectObjects == CollectObjects.Children)
		{
			localBounds = _E007(sources);
		}
		return NavMeshBuilder.UpdateNavMeshDataAsync(data, GetBuildSettings(), sources, localBounds);
	}

	private static void _E000(NavMeshSurface surface)
	{
		if (NavMeshSurface.m__E003.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(_E002));
		}
		if (!NavMeshSurface.m__E003.Contains(surface))
		{
			NavMeshSurface.m__E003.Add(surface);
		}
	}

	private static void _E001(NavMeshSurface surface)
	{
		NavMeshSurface.m__E003.Remove(surface);
		if (NavMeshSurface.m__E003.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(_E002));
		}
	}

	private static void _E002()
	{
		for (int i = 0; i < NavMeshSurface.m__E003.Count; i++)
		{
			NavMeshSurface.m__E003[i]._E009();
		}
	}

	private void _E003(ref List<NavMeshBuildSource> sources)
	{
		List<NavMeshModifierVolume> list;
		if (m_CollectObjects == CollectObjects.Children)
		{
			list = new List<NavMeshModifierVolume>(GetComponentsInChildren<NavMeshModifierVolume>());
			list.RemoveAll((NavMeshModifierVolume x) => !x.isActiveAndEnabled);
		}
		else
		{
			list = NavMeshModifierVolume.activeModifiers;
		}
		foreach (NavMeshModifierVolume item2 in list)
		{
			if (((int)m_LayerMask & (1 << item2.gameObject.layer)) != 0 && item2.AffectsAgentType(m_AgentTypeID))
			{
				Vector3 pos = item2.transform.TransformPoint(item2.center);
				Vector3 lossyScale = item2.transform.lossyScale;
				Vector3 vector = new Vector3(item2.size.x * Mathf.Abs(lossyScale.x), item2.size.y * Mathf.Abs(lossyScale.y), item2.size.z * Mathf.Abs(lossyScale.z));
				NavMeshBuildSource item = default(NavMeshBuildSource);
				item.shape = NavMeshBuildSourceShape.ModifierBox;
				item.transform = Matrix4x4.TRS(pos, item2.transform.rotation, Vector3.one);
				item.size = vector;
				item.area = item2.area;
				sources.Add(item);
			}
		}
	}

	private List<NavMeshBuildSource> _E004()
	{
		List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
		List<NavMeshBuildMarkup> list = new List<NavMeshBuildMarkup>();
		List<NavMeshModifier> list2;
		if (m_CollectObjects == CollectObjects.Children)
		{
			list2 = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
			list2.RemoveAll((NavMeshModifier x) => !x.isActiveAndEnabled);
		}
		else
		{
			list2 = NavMeshModifier.activeModifiers;
		}
		foreach (NavMeshModifier item2 in list2)
		{
			if (((int)m_LayerMask & (1 << item2.gameObject.layer)) != 0 && item2.AffectsAgentType(m_AgentTypeID))
			{
				NavMeshBuildMarkup item = default(NavMeshBuildMarkup);
				item.root = item2.transform;
				item.overrideArea = item2.overrideArea;
				item.area = item2.area;
				item.ignoreFromBuild = item2.ignoreFromBuild;
				list.Add(item);
			}
		}
		if (m_CollectObjects == CollectObjects.All)
		{
			NavMeshBuilder.CollectSources(null, m_LayerMask, m_UseGeometry, m_DefaultArea, list, sources);
		}
		else if (m_CollectObjects == CollectObjects.Children)
		{
			NavMeshBuilder.CollectSources(base.transform, m_LayerMask, m_UseGeometry, m_DefaultArea, list, sources);
		}
		else if (m_CollectObjects == CollectObjects.Volume)
		{
			NavMeshBuilder.CollectSources(_E006(Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one), new Bounds(m_Center, m_Size)), m_LayerMask, m_UseGeometry, m_DefaultArea, list, sources);
		}
		if (m_IgnoreNavMeshAgent)
		{
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshAgent>() != null);
		}
		if (m_IgnoreNavMeshObstacle)
		{
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshObstacle>() != null);
		}
		if (m_IgnoreSpawns)
		{
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<SpawnPointMarker>() != null);
		}
		if (m_IgnoreQuests)
		{
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<FlareShootDetectorZone>() != null);
		}
		if (m_IgnoreAudio)
		{
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<SpatialAudioCrossSceneGroup>() != null);
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<SpatialAudioPortal>() != null);
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<SpatialAudioRoom>() != null);
			sources.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<AudioTriggerArea>() != null);
		}
		_E003(ref sources);
		return sources;
	}

	private static Vector3 _E005(Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

	private static Bounds _E006(Matrix4x4 mat, Bounds bounds)
	{
		Vector3 vector = _E005(mat.MultiplyVector(Vector3.right));
		Vector3 vector2 = _E005(mat.MultiplyVector(Vector3.up));
		Vector3 vector3 = _E005(mat.MultiplyVector(Vector3.forward));
		Vector3 vector4 = mat.MultiplyPoint(bounds.center);
		Vector3 vector5 = vector * bounds.size.x + vector2 * bounds.size.y + vector3 * bounds.size.z;
		return new Bounds(vector4, vector5);
	}

	private Bounds _E007(List<NavMeshBuildSource> sources)
	{
		Matrix4x4 inverse = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one).inverse;
		Bounds result = default(Bounds);
		foreach (NavMeshBuildSource source in sources)
		{
			switch (source.shape)
			{
			case NavMeshBuildSourceShape.Mesh:
			{
				Mesh mesh = source.sourceObject as Mesh;
				result.Encapsulate(_E006(inverse * source.transform, mesh.bounds));
				break;
			}
			case NavMeshBuildSourceShape.Terrain:
			{
				TerrainData terrainData = source.sourceObject as TerrainData;
				result.Encapsulate(_E006(inverse * source.transform, new Bounds(0.5f * terrainData.size, terrainData.size)));
				break;
			}
			case NavMeshBuildSourceShape.Box:
			case NavMeshBuildSourceShape.Sphere:
			case NavMeshBuildSourceShape.Capsule:
			case NavMeshBuildSourceShape.ModifierBox:
				result.Encapsulate(_E006(inverse * source.transform, new Bounds(Vector3.zero, source.size)));
				break;
			}
		}
		result.Expand(0.1f);
		return result;
	}

	private bool _E008()
	{
		if (this.m__E001 != base.transform.position)
		{
			return true;
		}
		if (this.m__E002 != base.transform.rotation)
		{
			return true;
		}
		return false;
	}

	private void _E009()
	{
		if (_E008())
		{
			RemoveData();
			AddData();
		}
	}
}
