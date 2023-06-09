using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class MultisceneSharedOccluder : MonoBehaviour
{
	[Tooltip("Culling type.\n\nSharedOccluder\n-> not culled\n-> occludes in other scenes\n\nSharedOccludeeOccluder\n-> culled in volume\n-> occludes in other scenes\n\nOccludee\n-> culled in volume\n-> doesnt occlude in other scenes")]
	[SerializeField]
	private EOccludeMode _occludeMode;

	[SerializeField]
	[Tooltip("Which LOD to select when generating multiscene shared occluder")]
	private ESharedOccluderLODMode _LODMode;

	[Tooltip("Shadow culling mode on runtime")]
	[SerializeField]
	private EShadowMode _shadowMode;

	[SerializeField]
	[Tooltip("How to treat transprent materials when generating shared occluder")]
	private ETransparencyMode _transparencyMode;

	public EOccludeMode OccludeMode => _occludeMode;

	public ESharedOccluderLODMode SharedOccluderLODMode => _LODMode;

	public EShadowMode ShadowMode => _shadowMode;

	public ETransparencyMode TransparencyMode => _transparencyMode;
}
