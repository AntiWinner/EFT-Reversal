using UnityEngine;

public class SpeedTreeTest : MonoBehaviour
{
	public Shader BShader;

	private void Start()
	{
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		int subMeshCount = componentsInChildren[0].sharedMesh.subMeshCount;
		Mesh sharedMesh = _E414.Combine(componentsInChildren, Vector3.zero, subMeshCount);
		base.gameObject.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
		Material[] sharedMaterials = componentsInChildren[0].GetComponent<Renderer>().sharedMaterials;
		base.gameObject.AddComponent<MeshRenderer>().sharedMaterials = sharedMaterials;
		Material[] array = sharedMaterials;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].shader = BShader;
		}
		MeshFilter[] array2 = componentsInChildren;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.SetActive(value: false);
		}
	}

	private static void _E000(Mesh[] meshes, Renderer renderer = null)
	{
		for (int i = 0; i < meshes.Length; i++)
		{
			_E001(meshes[i], _ED3E._E000(83299) + i, renderer, i);
		}
	}

	private static GameObject _E001(Mesh mesh, string name, Renderer renderer = null, int submesh = -1)
	{
		GameObject obj = new GameObject(name);
		obj.AddComponent<MeshFilter>().sharedMesh = mesh;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		if (renderer != null)
		{
			if (submesh == -1)
			{
				meshRenderer.sharedMaterials = renderer.sharedMaterials;
				return obj;
			}
			meshRenderer.sharedMaterial = renderer.sharedMaterials[submesh];
		}
		return obj;
	}
}
