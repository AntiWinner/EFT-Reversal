using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerShaderBindings : ScriptableObject
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string extensionCode;

		internal bool _E000(_E4C5 ex)
		{
			return ex.GetExtensionCode().Equals(extensionCode);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E4C5 extension;

		internal bool _E000(_E4C5 ex)
		{
			return ex.GetExtensionCode().Equals(extension.GetExtensionCode());
		}
	}

	public List<ShaderInstance> shaderInstances;

	private static readonly List<string> m__E000 = new List<string>
	{
		_E4BF.SHADER_UNITY_STANDARD,
		_E4BF.SHADER_UNITY_STANDARD_SPECULAR,
		_E4BF.SHADER_UNITY_STANDARD_ROUGHNESS,
		_E4BF.SHADER_UNITY_VERTEXLIT,
		_E4BF.SHADER_UNITY_SPEED_TREE,
		_E4BF.SHADER_UNITY_SPEED_TREE_8,
		_E4BF.SHADER_UNITY_TREE_CREATOR_BARK,
		_E4BF.SHADER_UNITY_TREE_CREATOR_BARK_OPTIMIZED,
		_E4BF.SHADER_UNITY_TREE_CREATOR_LEAVES,
		_E4BF.SHADER_UNITY_TREE_CREATOR_LEAVES_OPTIMIZED,
		_E4BF.SHADER_UNITY_TREE_CREATOR_LEAVES_FAST,
		_E4BF.SHADER_UNITY_TREE_CREATOR_LEAVES_FAST_OPTIMIZED,
		_E4BF.SHADER_UNITY_TREE_SOFT_OCCLUSION_BARK,
		_E4BF.SHADER_UNITY_TREE_SOFT_OCCLUSION_LEAVES
	};

	private static readonly List<string> m__E001 = new List<string>
	{
		_E4BF.SHADER_GPUI_STANDARD,
		_E4BF.SHADER_GPUI_STANDARD_SPECULAR,
		_E4BF.SHADER_GPUI_STANDARD_ROUGHNESS,
		_E4BF.SHADER_GPUI_VERTEXLIT,
		_E4BF.SHADER_GPUI_SPEED_TREE,
		_E4BF.SHADER_GPUI_SPEED_TREE_8,
		_E4BF.SHADER_GPUI_TREE_CREATOR_BARK,
		_E4BF.SHADER_GPUI_TREE_CREATOR_BARK_OPTIMIZED,
		_E4BF.SHADER_GPUI_TREE_CREATOR_LEAVES,
		_E4BF.SHADER_GPUI_TREE_CREATOR_LEAVES_OPTIMIZED,
		_E4BF.SHADER_GPUI_TREE_CREATOR_LEAVES_FAST,
		_E4BF.SHADER_GPUI_TREE_CREATOR_LEAVES_FAST_OPTIMIZED,
		_E4BF.SHADER_GPUI_TREE_SOFT_OCCLUSION_BARK,
		_E4BF.SHADER_GPUI_TREE_SOFT_OCCLUSION_LEAVES
	};

	private static readonly List<string> _E002 = new List<string>
	{
		_E4BF.SHADER_GPUI_FOLIAGE,
		_E4BF.SHADER_GPUI_FOLIAGE_LWRP,
		_E4BF.SHADER_GPUI_SHADOWS_ONLY,
		_E4BF.SHADER_GPUI_BILLBOARD_2D_RENDERER_TREE,
		_E4BF.SHADER_GPUI_BILLBOARD_2D_RENDERER_TREECREATOR,
		_E4BF.SHADER_GPUI_BILLBOARD_2D_RENDERER_SOFTOCCLUSION,
		_E4BF.SHADER_GPUI_BILLBOARD_2D_RENDERER_STANDARD,
		_E4BF.SHADER_GPUI_HIZ_OCCLUSION_GENERATOR,
		_E4BF.SHADER_GPUI_TREE_PROXY
	};

	public List<_E4C5> shaderBindingsExtensions;

	public virtual bool HasExtension(string extensionCode)
	{
		if (shaderBindingsExtensions == null)
		{
			return false;
		}
		return shaderBindingsExtensions.Exists((_E4C5 ex) => ex.GetExtensionCode().Equals(extensionCode));
	}

	public virtual void AddExtension(_E4C5 extension)
	{
		if (shaderBindingsExtensions == null)
		{
			shaderBindingsExtensions = new List<_E4C5>();
		}
		if (!shaderBindingsExtensions.Exists((_E4C5 ex) => ex.GetExtensionCode().Equals(extension.GetExtensionCode())))
		{
			shaderBindingsExtensions.Add(extension);
		}
	}

	public virtual _E4C5 GetExtension(string extensionCode)
	{
		if (shaderBindingsExtensions != null && shaderBindingsExtensions.Count > 0)
		{
			foreach (_E4C5 shaderBindingsExtension in shaderBindingsExtensions)
			{
				if (shaderBindingsExtension.GetExtensionCode().Equals(extensionCode))
				{
					return shaderBindingsExtension;
				}
			}
		}
		Debug.LogError(_ED3E._E000(121534) + extensionCode);
		return null;
	}

	public virtual Shader GetInstancedShader(string shaderName, string extensionCode = null)
	{
		if (string.IsNullOrEmpty(extensionCode))
		{
			if (string.IsNullOrEmpty(shaderName))
			{
				return null;
			}
			if (shaderInstances == null)
			{
				shaderInstances = new List<ShaderInstance>();
			}
			foreach (ShaderInstance shaderInstance in shaderInstances)
			{
				if (shaderInstance.name.Equals(shaderName) && string.IsNullOrEmpty(shaderInstance.extensionCode))
				{
					return shaderInstance.instancedShader;
				}
			}
			if (GPUInstancerShaderBindings.m__E000.Contains(shaderName))
			{
				return Shader.Find(GPUInstancerShaderBindings.m__E001[GPUInstancerShaderBindings.m__E000.IndexOf(shaderName)]);
			}
			if (GPUInstancerShaderBindings.m__E001.Contains(shaderName))
			{
				return Shader.Find(shaderName);
			}
			if (_E002.Contains(shaderName))
			{
				return Shader.Find(shaderName);
			}
			if (!shaderName.Equals(_E4BF.SHADER_UNITY_STANDARD))
			{
				if (Application.isPlaying)
				{
					Debug.LogWarning(_ED3E._E000(121584) + shaderName + _ED3E._E000(121613));
				}
				return GetInstancedShader(_E4BF.SHADER_UNITY_STANDARD);
			}
			Debug.LogWarning(_ED3E._E000(121584) + shaderName);
			return null;
		}
		return GetExtension(extensionCode)?.GetInstancedShader(shaderInstances, shaderName);
	}

	public virtual Material GetInstancedMaterial(Material originalMaterial, string extensionCode = null)
	{
		if (string.IsNullOrEmpty(extensionCode))
		{
			if (originalMaterial == null || originalMaterial.shader == null)
			{
				Debug.LogWarning(_ED3E._E000(121646));
				return new Material(GetInstancedShader(_E4BF.SHADER_UNITY_STANDARD));
			}
			Material material = new Material(GetInstancedShader(originalMaterial.shader.name));
			material.CopyPropertiesFromMaterial(originalMaterial);
			material.name = originalMaterial.name + _ED3E._E000(121784);
			return material;
		}
		return GetExtension(extensionCode)?.GetInstancedMaterial(shaderInstances, originalMaterial);
	}

	public virtual void ResetShaderInstances()
	{
		if (shaderInstances == null)
		{
			shaderInstances = new List<ShaderInstance>();
		}
		else
		{
			shaderInstances.Clear();
		}
	}

	public virtual void ClearEmptyShaderInstances()
	{
		_ = shaderInstances;
	}

	public virtual void AddShaderInstance(string name, Shader instancedShader, bool isOriginalInstanced = false, string extensionCode = null)
	{
		shaderInstances.Add(new ShaderInstance(name, instancedShader, isOriginalInstanced, extensionCode));
	}

	public virtual bool IsShadersInstancedVersionExists(string shaderName, string extensionCode = null)
	{
		if (string.IsNullOrEmpty(extensionCode))
		{
			if (GPUInstancerShaderBindings.m__E000.Contains(shaderName) || GPUInstancerShaderBindings.m__E001.Contains(shaderName) || _E002.Contains(shaderName))
			{
				return true;
			}
			foreach (ShaderInstance shaderInstance in shaderInstances)
			{
				if (shaderInstance.name.Equals(shaderName) && string.IsNullOrEmpty(shaderInstance.extensionCode))
				{
					return true;
				}
			}
			return false;
		}
		return GetExtension(extensionCode)?.IsShadersInstancedVersionExists(shaderInstances, shaderName) ?? false;
	}
}
