using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
[PreferBinarySerialization]
[CreateAssetMenu(fileName = "New grid data", menuName = "Perfect Culling/Adaptive grid data", order = 1)]
public sealed class PerfectCullingAdaptiveGridData : ScriptableObject
{
	[SerializeField]
	public List<CullingCellData> CellData;

	[SerializeField]
	public List<GuidReference> GroupMapping;

	[SerializeField]
	public string GridHash;

	public int NumCells => CellData.Count;
}
