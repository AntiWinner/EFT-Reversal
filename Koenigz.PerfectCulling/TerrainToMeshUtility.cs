using UnityEngine;
using UnityEngine.Rendering;

namespace Koenigz.PerfectCulling;

[RequireComponent(typeof(Terrain))]
public class TerrainToMeshUtility : MonoBehaviour
{
	private readonly string _E000 = _ED3E._E000(72211);

	[Range(1f, 512f)]
	public int MeshResolutionX = 196;

	[Range(1f, 512f)]
	public int MeshResolutionZ = 196;

	public MeshRenderer meshRendererReference;

	public void CreateOrUpdateMesh()
	{
		Terrain component = GetComponent<Terrain>();
		string text = _ED3E._E000(72189) + component.name + _ED3E._E000(72183);
		TerrainData terrainData = component.terrainData;
		if (terrainData == null)
		{
			Debug.LogError(_ED3E._E000(72165));
			return;
		}
		float num = terrainData.size.x / (float)MeshResolutionX;
		float num2 = terrainData.size.z / (float)MeshResolutionZ;
		Mesh mesh = new Mesh();
		mesh.indexFormat = IndexFormat.UInt32;
		mesh.name = text;
		Vector3[] array = new Vector3[(MeshResolutionX + 1) * (MeshResolutionZ + 1)];
		int num3 = 0;
		for (int i = 0; i <= MeshResolutionZ; i++)
		{
			int num4 = 0;
			while (num4 <= MeshResolutionX)
			{
				float y = component.SampleHeight(new Vector3((float)num4 * num, 0f, (float)i * num2) + component.transform.position);
				array[num3] = new Vector3((float)num4 * num, y, (float)i * num2);
				num4++;
				num3++;
			}
		}
		mesh.vertices = array;
		int[] array2 = new int[MeshResolutionX * MeshResolutionZ * 6];
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		while (num7 < MeshResolutionZ)
		{
			int num8 = 0;
			while (num8 < MeshResolutionX)
			{
				array2[num5] = num6;
				array2[num5 + 3] = (array2[num5 + 2] = num6 + 1);
				array2[num5 + 4] = (array2[num5 + 1] = num6 + MeshResolutionX + 1);
				array2[num5 + 5] = num6 + MeshResolutionX + 2;
				num8++;
				num5 += 6;
				num6++;
			}
			num7++;
			num6++;
		}
		mesh.triangles = array2;
		if (meshRendererReference == null)
		{
			GameObject gameObject = new GameObject(text);
			gameObject.tag = _E000;
			gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
			meshRendererReference = gameObject.AddComponent<MeshRenderer>();
			meshRendererReference.enabled = false;
		}
		else
		{
			MeshFilter component2 = meshRendererReference.GetComponent<MeshFilter>();
			if (component2.sharedMesh != null)
			{
				Object.DestroyImmediate(component2.sharedMesh);
			}
			component2.sharedMesh = mesh;
		}
		meshRendererReference.transform.SetPositionAndRotation(component.transform.position, Quaternion.identity);
	}
}
