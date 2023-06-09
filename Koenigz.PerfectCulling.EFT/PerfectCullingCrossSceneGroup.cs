using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[RequireComponent(typeof(GuidComponent))]
public sealed class PerfectCullingCrossSceneGroup : MonoBehaviour
{
	public static readonly List<PerfectCullingCrossSceneGroup> AllCrossGroups = new List<PerfectCullingCrossSceneGroup>();

	[SerializeField]
	[HideInInspector]
	public PerfectCullingBakeGroup[] bakeGroups = Array.Empty<PerfectCullingBakeGroup>();

	[SerializeField]
	[HideInInspector]
	public GameObject[] sharedOccluders = Array.Empty<GameObject>();

	[HideInInspector]
	[SerializeField]
	public GameObject[] sharedOccludeeOccluders = Array.Empty<GameObject>();

	[SerializeField]
	public SharedOccluder sharedOccluder;

	[SerializeField]
	public bool debugDrawVisibilityLines;

	[SerializeField]
	public bool lightsOnly;

	[SerializeField]
	public bool disableOnRuntime;

	[SerializeField]
	public BakeBatch[] bakeBatches = Array.Empty<BakeBatch>();

	[SerializeField]
	public bool useGroundSuperSampling;

	[SerializeField]
	public bool disableGroupOnPointSample;

	[SerializeField]
	public bool allowGroupCulling;

	[SerializeField]
	public Transform groupRoot;

	[SerializeField]
	private Bounds _groupBoundingBox;

	[SerializeField]
	public bool AllowAdaptiveGridMapping;

	private _E4A0<ushort> m__E000;

	private _E4A0<ushort> m__E001;

	private (int, int) m__E002;

	private volatile int m__E003;

	public volatile int counterMainThread;

	public volatile int counterWorkThread;

	public object lockUpdateVisibilityQueues = new object();

	public List<PerfectCullingCrossSceneVolume._E000> _runtimeSharedVolumes = new List<PerfectCullingCrossSceneVolume._E000>();

	public string runtimeGroupName;

	private List<Vector2Int> m__E004 = new List<Vector2Int>();

	public Transform GroupRoot
	{
		get
		{
			if (!(groupRoot != null))
			{
				return base.transform;
			}
			return groupRoot;
		}
	}

	public Bounds GroupBoundingBox => _groupBoundingBox;

	internal bool _E005
	{
		get
		{
			_E4A0<ushort> obj = this.m__E000;
			if (obj != null && obj.Count == 0)
			{
				_E4A0<ushort> obj2 = this.m__E001;
				if (obj2 != null && obj2.Count == 0)
				{
					return counterMainThread == counterWorkThread;
				}
			}
			return false;
		}
	}

	public (int, int) SwitchStats => this.m__E002;

	public LODGroup[] GetLODGroups()
	{
		PerfectCullingCrossSceneGroupPreProcess component = GetComponent<PerfectCullingCrossSceneGroupPreProcess>();
		if (component != null)
		{
			return component.GetLODGroups();
		}
		return GroupRoot.GetComponentsInChildren<LODGroup>();
	}

	private void _E000()
	{
		int num = 0;
		_groupBoundingBox = default(Bounds);
		if (bakeGroups != null && bakeGroups.Length != 0)
		{
			PerfectCullingBakeGroup[] array = bakeGroups;
			for (int i = 0; i < array.Length; i++)
			{
				(bool, Bounds) groupBounds = array[i].GetGroupBounds();
				if (groupBounds.Item1)
				{
					_groupBoundingBox = groupBounds.Item2;
					break;
				}
				num++;
			}
			array = bakeGroups;
			for (int i = 0; i < array.Length; i++)
			{
				(bool, Bounds) groupBounds2 = array[i].GetGroupBounds();
				if (groupBounds2.Item1)
				{
					_groupBoundingBox.Encapsulate(groupBounds2.Item2);
				}
				else
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			Debug.LogWarning(_ED3E._E000(67282) + base.gameObject.name, base.gameObject);
		}
	}

	public float GetDistanceToNearestRendererSquared(Vector3 point)
	{
		float num = -1f;
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer[] renderers = array[i].renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null)
				{
					float sqrMagnitude = (renderer.transform.position - point).sqrMagnitude;
					if (num < 0f || sqrMagnitude < num)
					{
						num = sqrMagnitude;
					}
				}
			}
		}
		return num;
	}

	public int GetBakeHash()
	{
		PerfectCullingCrossSceneGroupPreProcess component = GetComponent<PerfectCullingCrossSceneGroupPreProcess>();
		if (component != null)
		{
			return component.GetBakeHash();
		}
		return GetBakeHashDefault();
	}

	public int GetBakeHashDefault()
	{
		int num = 13;
		num = num * 17 + bakeGroups.Length;
		for (int i = 0; i < bakeGroups.Length; i++)
		{
			num = (int)(num * 53 + bakeGroups[i].groupType);
			if (bakeGroups[i].renderers != null)
			{
				num = num * 23 + bakeGroups[i].renderers.Length;
			}
		}
		return num + _E4A3.HashStringInt32(base.gameObject.scene.name + base.gameObject.name);
	}

	public static void RefreshBakeGroups(PerfectCullingCrossSceneGroup group)
	{
	}

	public static void RefreshSharedOccluders(PerfectCullingCrossSceneGroup group)
	{
	}

	public HashSet<Renderer> GetSharedOccluderRenderers()
	{
		if (sharedOccluder == null)
		{
			return new HashSet<Renderer>();
		}
		return new HashSet<Renderer>(sharedOccluder?.GetRenderers());
	}

	public static HashSet<Renderer> GetGroupRenderers(Transform groupRoot, bool applyTypeFiltering = true, bool includeInactiveRenderers = true)
	{
		HashSet<Renderer> hashSet = new HashSet<Renderer>();
		for (int i = 0; i < groupRoot.childCount; i++)
		{
			Transform child = groupRoot.GetChild(i);
			MultisceneSharedOccluder component = child.GetComponent<MultisceneSharedOccluder>();
			if (component == null || component.OccludeMode == EOccludeMode.SharedOccluder)
			{
				continue;
			}
			List<Renderer> list = new List<Renderer>();
			child.GetComponentsInChildren(includeInactiveRenderers, list);
			if (applyTypeFiltering)
			{
				list.RemoveAll((Renderer rend) => !RendererFilter(rend));
			}
			foreach (Renderer item in list)
			{
				hashSet.Add(item);
			}
		}
		return hashSet;
	}

	public static bool RendererFilter(Renderer renderer)
	{
		if (renderer == null)
		{
			return false;
		}
		if (!renderer.enabled || !renderer.gameObject.activeInHierarchy)
		{
			return false;
		}
		MeshRenderer meshRenderer = renderer as MeshRenderer;
		if (meshRenderer != null)
		{
			MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh == null)
			{
				return false;
			}
		}
		if (!_E49D.SupportedRendererTypes.Contains(renderer.GetType()))
		{
			return false;
		}
		if (!_E4A3.ShouldProcessRenderer(renderer))
		{
			return false;
		}
		return true;
	}

	public void OnPostLevelLoaded()
	{
		_runtimeSharedVolumes.Sort(_E001);
	}

	private static int _E001(PerfectCullingCrossSceneVolume._E000 a, PerfectCullingCrossSceneVolume._E000 b)
	{
		if ((a.IsFineVolume && b.IsFineVolume) || (!a.IsFineVolume && !b.IsFineVolume))
		{
			return 0;
		}
		if (a.IsFineVolume && !b.IsFineVolume)
		{
			return -1;
		}
		if (!a.IsFineVolume && b.IsFineVolume)
		{
			return 1;
		}
		return 0;
	}

	public bool UpdateVisibleSetsMT(PerfectCullingCamera cam)
	{
		if (disableOnRuntime)
		{
			return true;
		}
		this.m__E003++;
		bool flag = false;
		foreach (PerfectCullingCrossSceneVolume._E000 runtimeSharedVolume in _runtimeSharedVolumes)
		{
			if (runtimeSharedVolume.UpdateVisibleRenderersAtPointMT(cam.ObservePosition, this.m__E003) == PerfectCullingCrossSceneVolume._E000.EUpdateResult.Ok)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		this.m__E000.Clear();
		this.m__E001.Clear();
		PerfectCullingBakeGroup[] array = bakeGroups;
		foreach (PerfectCullingBakeGroup perfectCullingBakeGroup in array)
		{
			if (perfectCullingBakeGroup.updateCounter >= this.m__E003)
			{
				if (!perfectCullingBakeGroup.isGroupEnabled)
				{
					this.m__E000.Add(perfectCullingBakeGroup.runtimeGroupIndex);
				}
			}
			else if (perfectCullingBakeGroup.isGroupEnabled)
			{
				this.m__E001.Add(perfectCullingBakeGroup.runtimeGroupIndex);
			}
		}
		this.m__E002 = (this.m__E000.Count, this.m__E001.Count);
		return true;
	}

	public void UpdateSwitchQueuesMT()
	{
		if ((this.m__E001.Count > 0 || this.m__E000.Count > 0) && Monitor.TryEnter(lockUpdateVisibilityQueues))
		{
			_E002();
			Monitor.Exit(lockUpdateVisibilityQueues);
		}
	}

	private void _E002()
	{
		int b = PerfectCullingSettings.Instance.numActivationsPerVolume / 2;
		int num = Mathf.Min(this.m__E001.Count, b);
		int num2 = Mathf.Min(this.m__E000.Count, b);
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < num; i++)
		{
			array[this.m__E001.Dequeue()].IsEnabled = false;
		}
		for (int j = 0; j < num2; j++)
		{
			array[this.m__E000.Dequeue()].IsEnabled = true;
		}
	}

	private void Awake()
	{
		_E006();
	}

	private void OnDestroy()
	{
		_E008();
	}

	private void Start()
	{
		_E005();
		_E007();
		runtimeGroupName = base.gameObject.name;
	}

	private void Update()
	{
		UpdateSwitchQueuesMT();
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying && debugDrawVisibilityLines)
		{
			_E004();
		}
	}

	private void _E003()
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
		Gizmos.DrawCube(GroupBoundingBox.center, GroupBoundingBox.size);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(GroupBoundingBox.center, GroupBoundingBox.size);
	}

	private void _E004()
	{
		if (PerfectCullingCrossSceneSampler.Instance == null || PerfectCullingCrossSceneSampler.Instance.CullingCamera == null)
		{
			return;
		}
		Vector3 position = PerfectCullingCrossSceneSampler.Instance.CullingCamera.transform.position;
		Gizmos.color = Color.red;
		PerfectCullingBakeGroup[] array = bakeGroups;
		foreach (PerfectCullingBakeGroup perfectCullingBakeGroup in array)
		{
			IEnumerator<Vector3> enumerateCenters = perfectCullingBakeGroup.EnumerateCenters;
			while (enumerateCenters.MoveNext())
			{
				Gizmos.color = (perfectCullingBakeGroup.IsEnabled ? Color.green : Color.red);
				Gizmos.DrawLine(position, enumerateCenters.Current);
			}
		}
	}

	private void _E005()
	{
		PerfectCullingCrossSceneGroupPreProcess component = GetComponent<PerfectCullingCrossSceneGroupPreProcess>();
		if ((bool)component)
		{
			bakeGroups = component.PrepareRuntimeContent();
		}
	}

	internal void _E006()
	{
		AllCrossGroups.Add(this);
		this.m__E000 = new _E4A0<ushort>(65535);
		this.m__E001 = new _E4A0<ushort>(65535);
	}

	internal void _E007()
	{
		ushort num = 0;
		PerfectCullingBakeGroup[] array = bakeGroups;
		foreach (PerfectCullingBakeGroup obj in array)
		{
			obj.Init();
			obj.runtimeGroupIndex = num;
			num = (ushort)(num + 1);
		}
		runtimeGroupName = base.gameObject.name;
	}

	internal void _E008()
	{
		AllCrossGroups.Remove(this);
		_runtimeSharedVolumes?.Clear();
		this.m__E000?.Clear();
		this.m__E001?.Clear();
		this.m__E000 = null;
		this.m__E001 = null;
	}

	public void CreateRuntimeProxies()
	{
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CreateRuntimeProxies();
		}
	}

	public void DeleteRuntimeProxies()
	{
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DeleteRuntimeProxies();
		}
	}

	internal void _E009()
	{
		PerfectCullingBakeGroup[] array = bakeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i]._E000();
		}
	}

	private void _E00A(ushort[] indices, int newCounter)
	{
		PerfectCullingBakeGroup[] array = bakeGroups;
		int num = indices.Length;
		for (int i = 0; i < num; i++)
		{
			array[indices[i]].updateCounter = newCounter;
		}
	}

	internal void _E00B(CullingGroupData data)
	{
		lock (lockUpdateVisibilityQueues)
		{
			this.m__E003++;
			try
			{
				if (data != null && data.Indices != null)
				{
					_E00A(data.Indices, this.m__E003);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(runtimeGroupName + _ED3E._E000(91199) + ex.Message);
			}
			this.m__E000.Clear();
			this.m__E001.Clear();
			PerfectCullingBakeGroup[] array = bakeGroups;
			foreach (PerfectCullingBakeGroup perfectCullingBakeGroup in array)
			{
				if (perfectCullingBakeGroup.isGroupEnabled && _E00C(perfectCullingBakeGroup.runtimeGroupIndex))
				{
					this.m__E001.Add(perfectCullingBakeGroup.runtimeGroupIndex);
				}
				else if (perfectCullingBakeGroup.updateCounter == this.m__E003)
				{
					if (!perfectCullingBakeGroup.isGroupEnabled && !_E00C(perfectCullingBakeGroup.runtimeGroupIndex))
					{
						this.m__E000.Add(perfectCullingBakeGroup.runtimeGroupIndex);
					}
				}
				else if (perfectCullingBakeGroup.isGroupEnabled)
				{
					this.m__E001.Add(perfectCullingBakeGroup.runtimeGroupIndex);
				}
			}
			this.m__E002 = (this.m__E000.Count, this.m__E001.Count);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddHiddenIndex(Vector2Int v)
	{
		this.m__E004.Add(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveHiddenIndex(Vector2Int v)
	{
		this.m__E004.Remove(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool _E00C(ushort index)
	{
		if (this.m__E004.Count == 0)
		{
			return false;
		}
		foreach (Vector2Int item in this.m__E004)
		{
			if (index >= item.x && index <= item.y)
			{
				return true;
			}
		}
		return false;
	}
}
