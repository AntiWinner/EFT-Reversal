using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
	public Material[] Materials;

	private void Awake()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		Material[] materials = Materials;
		foreach (Material material in materials)
		{
			_E000(componentsInChildren, base.transform, material);
		}
	}

	private static void _E000(Renderer[] renderers, Transform combinedParent, Material material)
	{
		int num = 0;
		Renderer renderer = null;
		List<CombineInstance> list = new List<CombineInstance>();
		foreach (Renderer renderer2 in renderers)
		{
			if (renderer2.enabled && !(renderer2.sharedMaterial != material))
			{
				renderer2.enabled = false;
				if (renderer == null)
				{
					renderer = renderer2;
				}
				MeshFilter component = renderer2.GetComponent<MeshFilter>();
				Mesh sharedMesh = component.sharedMesh;
				num += sharedMesh.vertexCount;
				if (num > 65535)
				{
					_E001(list.ToArray(), combinedParent, renderer);
					list.Clear();
					num = sharedMesh.vertexCount;
				}
				list.Add(new CombineInstance
				{
					mesh = sharedMesh,
					transform = component.transform.localToWorldMatrix
				});
			}
		}
		if (list.Count > 0)
		{
			_E001(list.ToArray(), combinedParent, renderer);
			list.Clear();
		}
	}

	private static void _E001(CombineInstance[] instancesAccumulator, Transform parent, Renderer origin)
	{
		Mesh mesh = new Mesh
		{
			name = _ED3E._E000(45305)
		};
		mesh.CombineMeshes(instancesAccumulator, mergeSubMeshes: true, useMatrices: true);
		GameObject obj = new GameObject(_ED3E._E000(45286) + origin.sharedMaterial.name);
		obj.AddComponent<MeshFilter>().sharedMesh = mesh;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = origin.sharedMaterial;
		meshRenderer.probeAnchor = origin.probeAnchor;
		meshRenderer.receiveShadows = origin.receiveShadows;
		meshRenderer.reflectionProbeUsage = origin.reflectionProbeUsage;
		meshRenderer.shadowCastingMode = origin.shadowCastingMode;
		obj.transform.position = Vector3.zero;
		obj.transform.parent = parent;
	}
}
