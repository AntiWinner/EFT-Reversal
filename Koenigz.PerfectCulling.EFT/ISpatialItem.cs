using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public interface ISpatialItem
{
	Bounds SpatialItemBounds { get; }
}
