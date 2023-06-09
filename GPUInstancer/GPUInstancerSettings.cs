using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class GPUInstancerSettings : ScriptableObject
{
	[Serializable]
	public class GPUIRenderingSettings
	{
		public GPUIPlatform platform;

		public GPUIMatrixHandlingType matrixHandlingType;

		public GPUIComputeThreadCount computeThreadCount;
	}

	public float versionNo;

	public GPUInstancerShaderBindings shaderBindings;

	public GPUInstancerBillboardAtlasBindings billboardAtlasBindings;

	public ShaderVariantCollection shaderVariantCollection;

	public bool packagesLoaded;

	public bool isHDRP;

	public bool isLWRP;

	public bool isShaderGraphPresent;

	public int instancingBoundsSize = 10000;

	public float MAX_DETAIL_DISTANCE = 2500f;

	public float MAX_TREE_DISTANCE = 2500f;

	public float MAX_PREFAB_DISTANCE = 10000f;

	public int MAX_PREFAB_EXTRA_BUFFER_SIZE = 16384;

	public Vector2Int DEFAULT_DETAIL_DISTANCE = new Vector2Int(55, 450);

	public Vector2Int DEFAULT_DETAIL_CELL_SIZE = new Vector2Int(50, 150);

	public Vector2 DEFAULT_DETAIL_DENSITY_FACTOR = new Vector2(0.5f, 0.25f);

	public Vector2 DEFAULT_FRUSTUM_OFFSET = new Vector2(0.62f, 0.1f);

	public bool DEFAULT_USE_DENSITY_MASK;

	public float DEFAULT_GRASS_SHADOW_DISTANCE = 25f;

	public bool useCustomPreviewBackgroundColor;

	public Color previewBackgroundColor = Color.white;

	public bool disableAutoGenerateBillboards;

	public bool disableShaderVariantCollection;

	public bool disableInstanceCountWarning;

	public bool disableAutoShaderConversion;

	public bool disableAutoVariantHandling;

	public bool testBothEyesForVROcclusion = true;

	public int vrRenderingMode;

	public List<GPUInstancerSettingsExtension> extensionSettings;

	public List<GPUIRenderingSettings> renderingSettingPresets;

	public bool hasCustomRenderingSettings;

	public GPUIRenderingSettings customRenderingSettings;

	public GPUIOcclusionCullingType occlusionCullingType;

	public int GetDefaultDetailDistance(bool isOptic)
	{
		if (!isOptic)
		{
			return DEFAULT_DETAIL_DISTANCE.x;
		}
		return DEFAULT_DETAIL_DISTANCE.y;
	}

	public float GetDefaultFrustumOffset(bool isOptic)
	{
		if (!isOptic)
		{
			return DEFAULT_FRUSTUM_OFFSET.x;
		}
		return DEFAULT_FRUSTUM_OFFSET.y;
	}

	public int GetDefaultDetailCellSize(bool isOptic)
	{
		if (!isOptic)
		{
			return DEFAULT_DETAIL_CELL_SIZE.x;
		}
		return DEFAULT_DETAIL_CELL_SIZE.y;
	}

	public float GetDefaultDetailDensity(Terrain terrain, bool isOptic)
	{
		return terrain.detailObjectDensity * (isOptic ? DEFAULT_DETAIL_DENSITY_FACTOR.y : DEFAULT_DETAIL_DENSITY_FACTOR.x);
	}

	public void AddExtension(GPUInstancerSettingsExtension extension)
	{
		if (!(extension == null))
		{
			if (extensionSettings == null)
			{
				extensionSettings = new List<GPUInstancerSettingsExtension>();
			}
			extensionSettings.RemoveAll((GPUInstancerSettingsExtension ex) => ex == null);
			if (!extensionSettings.Contains(extension))
			{
				extensionSettings.Add(extension);
			}
		}
	}

	public static GPUInstancerSettings GetDefaultGPUInstancerSettings()
	{
		GPUInstancerSettings gPUInstancerSettings = Resources.Load<GPUInstancerSettings>(_E4BF.SETTINGS_PATH + _E4BF.GPUI_SETTINGS_DEFAULT_NAME);
		gPUInstancerSettings.SetDefultBindings();
		return gPUInstancerSettings;
	}

	public virtual void SetDefultBindings()
	{
		SetDefaultGPUInstancerShaderBindings();
		SetDefaultGPUInstancerBillboardAtlasBindings();
		SetDefaultShaderVariantCollection();
		renderingSettingPresets = new List<GPUIRenderingSettings>
		{
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.Default,
				matrixHandlingType = GPUIMatrixHandlingType.Default,
				computeThreadCount = GPUIComputeThreadCount.x512
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.OpenGLCore,
				matrixHandlingType = GPUIMatrixHandlingType.Default,
				computeThreadCount = GPUIComputeThreadCount.x256
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.Metal,
				matrixHandlingType = GPUIMatrixHandlingType.Default,
				computeThreadCount = GPUIComputeThreadCount.x256
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.GLES31,
				matrixHandlingType = GPUIMatrixHandlingType.CopyToTexture,
				computeThreadCount = GPUIComputeThreadCount.x128
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.Vulkan,
				matrixHandlingType = GPUIMatrixHandlingType.MatrixAppend,
				computeThreadCount = GPUIComputeThreadCount.x128
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.PS4,
				matrixHandlingType = GPUIMatrixHandlingType.Default,
				computeThreadCount = GPUIComputeThreadCount.x512
			},
			new GPUIRenderingSettings
			{
				platform = GPUIPlatform.XBoxOne,
				matrixHandlingType = GPUIMatrixHandlingType.Default,
				computeThreadCount = GPUIComputeThreadCount.x512
			}
		};
	}

	public virtual void SetDefaultGPUInstancerShaderBindings()
	{
		if (shaderBindings == null)
		{
			shaderBindings = GetDefaultGPUInstancerShaderBindings();
		}
	}

	public static GPUInstancerShaderBindings GetDefaultGPUInstancerShaderBindings()
	{
		return Resources.Load<GPUInstancerShaderBindings>(_E4BF.SETTINGS_PATH + _E4BF.SHADER_BINDINGS_DEFAULT_NAME);
	}

	public virtual void SetDefaultGPUInstancerBillboardAtlasBindings()
	{
		if (billboardAtlasBindings == null)
		{
			billboardAtlasBindings = GetDefaultGPUInstancerBillboardAtlasBindings();
		}
	}

	public static GPUInstancerBillboardAtlasBindings GetDefaultGPUInstancerBillboardAtlasBindings()
	{
		return Resources.Load<GPUInstancerBillboardAtlasBindings>(_E4BF.SETTINGS_PATH + _E4BF.BILLBOARD_ATLAS_BINDINGS_DEFAULT_NAME);
	}

	public virtual void SetDefaultShaderVariantCollection()
	{
		if (!disableShaderVariantCollection)
		{
			if (shaderVariantCollection == null)
			{
				shaderVariantCollection = GetDefaultShaderVariantCollection();
			}
			SetDefaultShaderVariants();
		}
	}

	public static ShaderVariantCollection GetDefaultShaderVariantCollection()
	{
		return Resources.Load<ShaderVariantCollection>(_E4BF.SETTINGS_PATH + _E4BF.SHADER_VARIANT_COLLECTION_DEFAULT_NAME);
	}

	public virtual void SetDefaultShaderVariants()
	{
		AddShaderVariantToCollection(_E4BF.SHADER_GPUI_SHADOWS_ONLY);
		AddShaderVariantToCollection(_E4BF.SHADER_GPUI_HIZ_OCCLUSION_GENERATOR);
		AddShaderVariantToCollection(_E4BF.SHADER_GPUI_TREE_PROXY);
	}

	public virtual void AddShaderVariantToCollection(string shaderName, string extensionCode = null)
	{
		_ = disableShaderVariantCollection;
	}

	public virtual void AddShaderVariantToCollection(Material material, string extensionCode = null)
	{
		_ = disableShaderVariantCollection;
	}

	public GPUIMatrixHandlingType GetMatrixHandlingType(GPUIPlatform platform)
	{
		if (hasCustomRenderingSettings && customRenderingSettings != null)
		{
			return customRenderingSettings.matrixHandlingType;
		}
		for (int i = 0; i < renderingSettingPresets.Count; i++)
		{
			if (renderingSettingPresets[i].platform == platform)
			{
				return renderingSettingPresets[i].matrixHandlingType;
			}
		}
		return GPUIMatrixHandlingType.Default;
	}

	public GPUIComputeThreadCount GetComputeThreadCount(GPUIPlatform platform)
	{
		if (hasCustomRenderingSettings && customRenderingSettings != null)
		{
			return customRenderingSettings.computeThreadCount;
		}
		for (int i = 0; i < renderingSettingPresets.Count; i++)
		{
			if (renderingSettingPresets[i].platform == platform)
			{
				return renderingSettingPresets[i].computeThreadCount;
			}
		}
		return GPUIComputeThreadCount.x1024;
	}
}
