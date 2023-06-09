using System;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public abstract class GPUInstancerPrototype : ScriptableObject
{
	public GameObject prefabObject;

	public bool isShadowCasting;

	public bool useCustomShadowDistance;

	public float shadowDistance;

	public float[] shadowLODMap = new float[16]
	{
		0f, 4f, 0f, 0f, 1f, 5f, 0f, 0f, 2f, 6f,
		0f, 0f, 3f, 7f, 0f, 0f
	};

	public bool useOriginalShaderForShadow;

	public bool cullShadows;

	public float minDistance;

	public float maxDistance = 500f;

	public float minDistanceOptic;

	public float maxDistanceOptic = 500f;

	public bool isFrustumCulling = true;

	public bool isOcclusionCulling = true;

	public float minCullingDistance;

	public float occlusionOffset;

	public int occlusionAccuracy = 1;

	public Vector3 boundsOffset;

	public bool isLODCrossFade;

	public bool isLODCrossFadeAnimate;

	[Range(0.01f, 1f)]
	public float lodFadeTransitionWidth = 0.1f;

	public float lodBiasAdjustment = 1f;

	public GPUInstancerBillboard billboard;

	public bool isBillboardDisabled;

	public bool useGeneratedBillboard;

	public bool checkedForBillboardExtensions;

	public bool autoUpdateTransformData;

	public GPUInstancerTreeType treeType;

	public string warningText;

	public override string ToString()
	{
		if (prefabObject != null)
		{
			return prefabObject.name;
		}
		return base.ToString();
	}
}
