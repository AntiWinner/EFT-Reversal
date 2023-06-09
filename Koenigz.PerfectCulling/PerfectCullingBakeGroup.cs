using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Koenigz.PerfectCulling;

[Serializable]
public class PerfectCullingBakeGroup
{
	public readonly struct RuntimeGroupContent
	{
		public readonly Renderer Renderer;

		public readonly ShadowCastingMode ShadowCastingMode;

		public readonly GameObject RendererObj;

		public readonly string RendererObjName;

		public RuntimeGroupContent(Renderer renderer, ShadowCastingMode shadowCastingMode)
		{
			Renderer = renderer;
			ShadowCastingMode = shadowCastingMode;
			RendererObj = renderer.gameObject;
			RendererObjName = renderer.gameObject.name;
		}
	}

	public enum GroupType
	{
		Other,
		LOD,
		User,
		Light,
		CullingLightObject,
		Door
	}

	public GroupType groupType;

	public Renderer[] renderers;

	public int serializedGroupId;

	public float lightBakeSize;

	public Vector3 lightBakePosition;

	public Vector3 lightDir;

	public float lightAngle;

	public LightType lightType;

	public CullingObject[] cullingLightObjects;

	[NonSerialized]
	public int vertexCount;

	[NonSerialized]
	public ushort runtimeGroupIndex;

	[NonSerialized]
	public int updateCounter;

	[NonSerialized]
	public Renderer[] runtimeProxies;

	[NonSerialized]
	public LODGroup runtimeLodGroup;

	[NonSerialized]
	private int m__E000;

	[NonSerialized]
	private RuntimeGroupContent[] m__E001;

	[NonSerialized]
	public bool isGroupEnabled = true;

	public const float PERSISTENT_SHADOW_OBJ_SIZE = 2.5f;

	public static int numPersistentShadowLods;

	[NonSerialized]
	public Light[] runtimeLights;

	public IEnumerator<Vector3> EnumerateCenters
	{
		get
		{
			if (Application.isPlaying)
			{
				if (this.m__E001 != null)
				{
					RuntimeGroupContent[] array = this.m__E001;
					for (int i = 0; i < array.Length; i++)
					{
						RuntimeGroupContent runtimeGroupContent = array[i];
						if (runtimeGroupContent.Renderer != null)
						{
							yield return runtimeGroupContent.Renderer.transform.position;
						}
					}
				}
				if (runtimeLights != null)
				{
					Light[] array2 = runtimeLights;
					foreach (Light light in array2)
					{
						yield return light.transform.position;
					}
				}
				yield break;
			}
			if (renderers != null)
			{
				Renderer[] array3 = renderers;
				foreach (Renderer renderer in array3)
				{
					yield return renderer.transform.position;
				}
			}
			if (cullingLightObjects != null)
			{
				CullingObject[] array4 = cullingLightObjects;
				foreach (CullingObject cullingObject in array4)
				{
					yield return cullingObject.transform.position;
				}
			}
		}
	}

	public RuntimeGroupContent[] RuntimeGroupData => this.m__E001;

	public bool IsEnabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return isGroupEnabled;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			isGroupEnabled = value;
			Toggle(value);
		}
	}

	public float Volume
	{
		get
		{
			Vector3 lhs = Vector3.zero;
			if (renderers == null || renderers.Length == 0)
			{
				return 0f;
			}
			Renderer[] array = renderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null)
				{
					lhs = Vector3.Max(lhs, renderer.bounds.size);
				}
			}
			return Mathf.Max(lhs.x, Mathf.Max(lhs.y, lhs.z));
		}
	}

	public void DeleteRuntimeProxies()
	{
		Renderer[] array = runtimeProxies;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(array[i].gameObject);
		}
		runtimeProxies = null;
	}

	public void CreateRuntimeProxies()
	{
		if (runtimeProxies != null)
		{
			return;
		}
		List<Renderer> list = new List<Renderer>();
		Material rainbowProxyMaterial = PerfectCullingResourcesLocator.Instance.RainbowProxyMaterial;
		RuntimeGroupContent[] array = this.m__E001;
		for (int i = 0; i < array.Length; i++)
		{
			RuntimeGroupContent runtimeGroupContent = array[i];
			if (runtimeGroupContent.Renderer is MeshRenderer)
			{
				MeshRenderer meshRenderer = _E4A3.CloneRenderer(runtimeGroupContent.Renderer as MeshRenderer, rainbowProxyMaterial);
				meshRenderer.enabled = !isGroupEnabled;
				list.Add(meshRenderer);
			}
		}
		runtimeProxies = list.ToArray();
	}

	internal void _E000()
	{
		if (!Application.isPlaying)
		{
			throw new InvalidOperationException(_ED3E._E000(71884));
		}
		Material autotestRendererMaterialOpaque = PerfectCullingResourcesLocator.Instance.AutotestRendererMaterialOpaque;
		Material autotestRendererMaterialTransparent = PerfectCullingResourcesLocator.Instance.AutotestRendererMaterialTransparent;
		Material autotestProxyMaterialOpaque = PerfectCullingResourcesLocator.Instance.AutotestProxyMaterialOpaque;
		Material autotestProxyMaterialTransparent = PerfectCullingResourcesLocator.Instance.AutotestProxyMaterialTransparent;
		if (groupType == GroupType.LOD)
		{
			LODGroup original = _E001();
			Renderer[] array = renderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null)
				{
					renderer.enabled = false;
				}
			}
			renderers = _E4A3._E002(original, autotestRendererMaterialOpaque, autotestRendererMaterialTransparent).ToArray();
			Init();
			runtimeProxies = _E4A3._E002(original, autotestProxyMaterialOpaque, autotestProxyMaterialTransparent).ToArray();
		}
		else
		{
			_ = groupType;
		}
	}

	internal LODGroup _E001()
	{
		Renderer[] array = renderers;
		foreach (Renderer renderer in array)
		{
			if (!(renderer == null))
			{
				LODGroup lODGroup = _E4A3.ContainsComponentOnParentInHierarchy<LODGroup>(renderer.transform);
				if (lODGroup != null)
				{
					return lODGroup;
				}
				lODGroup = renderer.GetComponent<LODGroup>();
				if (lODGroup != null)
				{
					return lODGroup;
				}
			}
		}
		return null;
	}

	public void ValidateRuntime()
	{
		if (this.m__E001 == null)
		{
			return;
		}
		for (int i = 0; i < this.m__E000; i++)
		{
			RuntimeGroupContent runtimeGroupContent = this.m__E001[i];
			if (runtimeGroupContent.Renderer == null)
			{
				if (runtimeGroupContent.RendererObj != null)
				{
					Debug.LogError(_ED3E._E000(71914) + runtimeGroupContent.RendererObjName, runtimeGroupContent.RendererObj);
				}
				else
				{
					Debug.LogError(_ED3E._E000(71953) + runtimeGroupContent.RendererObjName);
				}
			}
		}
	}

	private Bounds _E002()
	{
		Renderer[] array = renderers;
		foreach (Renderer renderer in array)
		{
			if (renderer != null && (!(renderer is MeshRenderer) || !((renderer as MeshRenderer).GetComponent<MeshFilter>().sharedMesh == null)))
			{
				return renderer.bounds;
			}
		}
		Debug.LogError(_ED3E._E000(71983));
		return default(Bounds);
	}

	public (bool, Bounds) GetGroupBounds()
	{
		if (renderers != null && renderers.Length != 0)
		{
			Bounds item = _E002();
			Renderer[] array = renderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null && (!(renderer is MeshRenderer) || !((renderer as MeshRenderer).GetComponent<MeshFilter>().sharedMesh == null)))
				{
					item.Encapsulate(renderer.bounds);
				}
			}
			return (true, item);
		}
		return (false, default(Bounds));
	}

	public bool CheckGroup()
	{
		bool result = false;
		Renderer[] array = renderers;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public int Init()
	{
		int num = 0;
		this.m__E001 = new RuntimeGroupContent[renderers.Length];
		Renderer[] array;
		if (groupType == GroupType.LOD && renderers != null)
		{
			array = renderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null)
				{
					runtimeLodGroup = renderer.transform.parent.GetComponent<LODGroup>();
					break;
				}
			}
		}
		if (cullingLightObjects != null && cullingLightObjects.Length != 0)
		{
			List<Light> list = new List<Light>();
			CullingObject[] array2 = cullingLightObjects;
			foreach (CullingObject cullingObject in array2)
			{
				if (!(cullingObject == null))
				{
					Light component = cullingObject.GetComponent<Light>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			runtimeLights = list.ToArray();
		}
		array = renderers;
		foreach (Renderer renderer2 in array)
		{
			if (renderer2 == null)
			{
				num++;
			}
			else if (renderer2.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
			{
				_ = renderer2.bounds.size;
				numPersistentShadowLods++;
			}
			else
			{
				PushRuntimeRenderer(renderer2);
			}
		}
		return num;
	}

	public bool ContainsRuntimeRenderer(Renderer r)
	{
		for (int i = 0; i < this.m__E000; i++)
		{
			if (r == this.m__E001[i].Renderer)
			{
				return true;
			}
		}
		return false;
	}

	public void PushRuntimeRenderer(Renderer renderer)
	{
		_E003(new RuntimeGroupContent(renderer, renderer.shadowCastingMode));
	}

	public bool PopRuntimeRenderer(Renderer renderer)
	{
		int num = -1;
		for (int i = 0; i < this.m__E000; i++)
		{
			if (this.m__E001[i].Renderer == renderer)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return false;
		}
		if (this.m__E000 >= 2)
		{
			ref RuntimeGroupContent reference = ref this.m__E001[num];
			ref RuntimeGroupContent reference2 = ref this.m__E001[this.m__E000 - 1];
			RuntimeGroupContent runtimeGroupContent = this.m__E001[this.m__E000 - 1];
			RuntimeGroupContent runtimeGroupContent2 = this.m__E001[num];
			reference = runtimeGroupContent;
			reference2 = runtimeGroupContent2;
		}
		_E004();
		return true;
	}

	private void _E003(RuntimeGroupContent groupContent)
	{
		if (this.m__E000 >= this.m__E001.Length)
		{
			Array.Resize(ref this.m__E001, Mathf.Max(1, this.m__E000) * 2);
		}
		this.m__E001[this.m__E000] = groupContent;
		this.m__E000++;
	}

	private RuntimeGroupContent _E004()
	{
		if (this.m__E000 <= 0)
		{
			return default(RuntimeGroupContent);
		}
		this.m__E000--;
		return this.m__E001[this.m__E000];
	}

	public void ClearRuntimeRenderers()
	{
		this.m__E000 = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Toggle(bool rendererEnabled)
	{
		if (runtimeProxies != null)
		{
			Renderer[] array = runtimeProxies;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = !rendererEnabled;
			}
		}
		if (cullingLightObjects != null)
		{
			CullingObject[] array2 = cullingLightObjects;
			foreach (CullingObject cullingObject in array2)
			{
				if (cullingObject != null)
				{
					cullingObject.SetAutocullVisibility(rendererEnabled);
				}
			}
		}
		for (int j = 0; j < this.m__E000; j++)
		{
			this.m__E001[j].Renderer.enabled = rendererEnabled;
		}
	}

	public void ForeachRenderer(Action<Renderer> actionForRenderer)
	{
		Renderer[] array = renderers;
		foreach (Renderer obj in array)
		{
			actionForRenderer(obj);
		}
	}

	public bool CollectMeshStats()
	{
		int num = 0;
		if (renderers != null)
		{
			Renderer[] array = renderers;
			foreach (Renderer renderer in array)
			{
				if (renderer == null)
				{
					Debug.LogWarning(_ED3E._E000(72005));
					return false;
				}
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component == null)
				{
					Debug.LogWarning(_ED3E._E000(72047) + renderer.name, renderer.gameObject);
				}
				else if (component.sharedMesh == null)
				{
					Debug.LogWarning(_ED3E._E000(72070) + renderer.name, renderer.gameObject);
				}
				else
				{
					num += component.sharedMesh.vertexCount;
				}
			}
			vertexCount = num;
		}
		return true;
	}

	public int GetRuntimeRendererCount()
	{
		return this.m__E000;
	}
}
