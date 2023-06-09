using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingAdaptiveGrid : MonoBehaviour
{
	[CompilerGenerated]
	private static PerfectCullingAdaptiveGrid m__E000;

	[SerializeField]
	private PerfectCullingAdaptiveGridData _gridData;

	private _E4B7 m__E001;

	private List<PerfectCullingCrossSceneGroup> m__E002;

	private List<OrientedBounds> _E003;

	private List<OrientedPoint> _E004;

	public static PerfectCullingAdaptiveGrid Instance
	{
		[CompilerGenerated]
		get
		{
			return PerfectCullingAdaptiveGrid.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			PerfectCullingAdaptiveGrid.m__E000 = value;
		}
	}

	public PerfectCullingAdaptiveGridData GridData => _gridData;

	public _E4B7 RuntimeBVH => this.m__E001;

	public List<PerfectCullingCrossSceneGroup> RuntimeGroupMapping => this.m__E002;

	public List<CullingCellData> AdaptiveCells => _gridData.CellData;

	internal void _E000()
	{
		Instance = this;
		_E001();
		_E002();
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
			this.m__E001 = null;
			_gridData = null;
			if (this.m__E002 != null)
			{
				this.m__E002.Clear();
				this.m__E002 = null;
			}
		}
	}

	private void _E001()
	{
		this.m__E002 = new List<PerfectCullingCrossSceneGroup>();
		if (_gridData == null)
		{
			return;
		}
		foreach (GuidReference item in _gridData.GroupMapping)
		{
			if (item.gameObject == null)
			{
				_E4BB.Throw(_ED3E._E000(67215) + item.ObjectGuidString);
			}
			PerfectCullingCrossSceneGroup component = item.gameObject.GetComponent<PerfectCullingCrossSceneGroup>();
			if (component == null)
			{
				_E4BB.Throw(_ED3E._E000(67244) + item.gameObject.name);
			}
			this.m__E002.Add(component);
		}
	}

	internal void _E002()
	{
		this.m__E001 = new _E4B7();
		if (!(_gridData != null) || _gridData.CellData == null || _gridData.CellData.Count <= 0)
		{
			return;
		}
		this.m__E001.Initialize(_gridData.CellData.Count);
		int num = 0;
		foreach (CullingCellData cellDatum in _gridData.CellData)
		{
			this.m__E001.Add(cellDatum);
			cellDatum.RuntimeCellId = num;
			num++;
		}
	}
}
