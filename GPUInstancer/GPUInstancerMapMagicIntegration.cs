using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

[ExecuteInEditMode]
public class GPUInstancerMapMagicIntegration : MonoBehaviour
{
	public List<GPUInstancerPrototype> detailPrototypes;

	public List<GPUInstancerPrototype> treePrototypes;

	public List<GPUInstancerPrototype> prefabPrototypes;

	public GPUInstancerTerrainSettings terrainSettings;

	public bool importDetails;

	public bool importTrees;

	public bool importObjects;

	private bool _E000;

	public bool isOpticCamera = true;

	public GPUInstancerCameraData cameraData = new GPUInstancerCameraData();

	public bool isFrustumCulling = true;

	public bool isOcclusionCulling = true;

	public float minCullingDistance;

	public int detailLayer;

	public bool detailRunInThreads = true;

	public bool useSinglePrefabManager;

	public bool disableMeshRenderers;

	public bool prefabRunInThreads;

	public List<DetailPrototype> terrainDetailPrototypes;

	public List<TreePrototype> terrainTreePrototypes;

	public List<GameObject> prefabs;

	public List<GameObject> selectedPrefabs;

	public GPUInstancerPrefabManager prefabManagerInstance;

	public void SetCamera(Camera camera)
	{
		cameraData.mainCamera = camera;
		_E4BD.SetCamera(camera);
	}
}
