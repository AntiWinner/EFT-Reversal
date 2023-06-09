using System;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class GPUInstancerBillboard
{
	public BillboardQuality billboardQuality = BillboardQuality.Mid;

	public int atlasResolution = 2048;

	public int frameCount = 8;

	public bool replaceLODCullWithBillboard;

	public bool isOverridingOriginalCutoff;

	public float cutoffOverride = -1f;

	[Range(0f, 1f)]
	public float billboardBrightness = 0.5f;

	[Range(0.01f, 1f)]
	public float billboardDistance = 0.8f;

	public float quadSize;

	public float yPivotOffset;

	public Texture2D albedoAtlasTexture;

	public Texture2D normalAtlasTexture;

	public bool customBillboardInLODGroup;

	public bool useCustomBillboard;

	public Mesh customBillboardMesh;

	public Material customBillboardMaterial;

	public bool isBillboardShadowCasting;

	public bool billboardFaceCamPos;
}
