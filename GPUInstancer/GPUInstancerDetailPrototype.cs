using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class GPUInstancerDetailPrototype : GPUInstancerPrototype
{
	public int prototypeIndex;

	public DetailRenderMode detailRenderMode;

	public bool usePrototypeMesh;

	public Texture2D prototypeTexture;

	public Texture2D bumpMap;

	public bool useCustomMaterialForTextureDetail;

	public Material textureDetailCustomMaterial;

	public int detailResolution;

	[Range(0f, 1f)]
	public float detailDensity = 1f;

	[Range(0f, 1f)]
	public float detailGrowDirection;

	public bool useCrossQuads = true;

	[Range(1f, 4f)]
	public int quadCount = 2;

	public bool isBillboard;

	[Range(0.5f, 1f)]
	public float billboardDistance = 0.985f;

	public bool billboardDistanceDebug;

	public Color billboardDistanceDebugColor = Color.red;

	public bool billboardFaceCamPos;

	public Color detailHealthyColor;

	public Color detailDryColor;

	public float noiseSpread = 0.2f;

	[Range(0f, 1f)]
	public float ambientOcclusion = 0.3f;

	[Range(0f, 1f)]
	public float gradientPower = 0.3f;

	public Color windWaveTintColor;

	[Range(0f, 1f)]
	public float windIdleSway = 0.3f;

	public bool windWavesOn = true;

	[Range(0f, 1f)]
	public float windWaveTint = 0.5f;

	[Range(0f, 1f)]
	public float windWaveSway = 0.15f;

	public Vector4 detailScale;

	public int[] cachedDensityMapForInstance;

	public float densityFadeFactor = 10f;

	public int densityMapIndex;

	public Vector4 densityChanelMask;

	public Vector2 densityMinMax;

	public bool useDensityMask;

	public bool useCustomHealthyDryNoiseTexture;

	public Texture2D healthyDryNoiseTexture;

	public List<Vector4> alphaChannelMask = new List<Vector4>
	{
		Vector4.zero,
		Vector4.zero,
		Vector4.zero,
		Vector4.zero
	};

	public bool useAlphaMask;

	public bool useVertexFit;

	public bool useTerrainNormal = true;

	[Range(0f, 5f)]
	public float gradientNormalHeight = 1f;

	public override string ToString()
	{
		if (prototypeTexture != null)
		{
			return prototypeTexture.name;
		}
		return base.ToString();
	}

	public Texture2DArray CreateTextureArray(Texture2D[] textureList)
	{
		Texture2D texture2D = textureList[0];
		Texture2DArray texture2DArray = new Texture2DArray(texture2D.width, texture2D.height, textureList.Length, TextureFormat.RGBA32, mipChain: true, linear: false)
		{
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Repeat
		};
		for (int i = 0; i < textureList.Length; i++)
		{
			Graphics.CopyTexture(textureList[i], 0, 0, texture2DArray, i, 0);
		}
		return texture2DArray;
	}

	public float[] CreateAlphaMaskArray()
	{
		if (!alphaChannelMask.Any())
		{
			alphaChannelMask = new List<Vector4>
			{
				Vector4.zero,
				Vector4.zero,
				Vector4.zero,
				Vector4.zero
			};
		}
		List<float> list = new List<float>();
		foreach (Vector4 item in alphaChannelMask)
		{
			list.Add(item.x);
			list.Add(item.y);
			list.Add(item.z);
			list.Add(item.w);
		}
		return list.ToArray();
	}
}
