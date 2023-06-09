using System;
using System.Globalization;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class BillboardAtlasBinding
{
	public GameObject prefab;

	public int atlasResolution;

	public int frameCount;

	public Texture2D albedoAtlasTexture;

	public Texture2D normalAtlasTexture;

	public float quadSize;

	public float yPivotOffset;

	public string modifiedDate;

	public BillboardAtlasBinding(GameObject prefab, int atlasResolution, int frameCount, Texture2D albedoAtlasTexture, Texture2D normalAtlasTexture, float quadSize, float yPivotOffset)
	{
		this.prefab = prefab;
		this.atlasResolution = atlasResolution;
		this.frameCount = frameCount;
		this.albedoAtlasTexture = albedoAtlasTexture;
		this.normalAtlasTexture = normalAtlasTexture;
		this.quadSize = quadSize;
		this.yPivotOffset = yPivotOffset;
		modifiedDate = _E5AD.Now.ToString(_ED3E._E000(77760), CultureInfo.InvariantCulture);
	}
}
