using System.Runtime.CompilerServices;
using EFT.EnvironmentEffect;
using UnityEngine;

[_E2E2(-100)]
[DisallowMultipleComponent]
public class WeatherObstacle : MonoBehaviour
{
	private static WeatherObstacle m__E000;

	[CompilerGenerated]
	private MeshCollider m__E001;

	private Mesh m__E002;

	public static WeatherObstacle Instance => WeatherObstacle.m__E000;

	public MeshCollider MeshCollider
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	private void Awake()
	{
		WeatherObstacle.m__E000 = this;
		this.m__E002 = _E000();
		MeshCollider = base.gameObject.GetOrAddComponent<MeshCollider>();
		MeshCollider.sharedMesh = this.m__E002;
		_E001();
	}

	private Mesh _E000()
	{
		DryPlane[] componentsInChildren = base.gameObject.GetComponentsInChildren<DryPlane>();
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i] = new CombineInstance
			{
				mesh = componentsInChildren[i].Mesh,
				transform = componentsInChildren[i].transform.localToWorldMatrix
			};
		}
		Mesh mesh = new Mesh();
		mesh.name = _ED3E._E000(84907);
		mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
		return mesh;
	}

	private void _E001()
	{
		for (int num = base.transform.childCount - 1; num >= 0; num--)
		{
			Object.Destroy(base.transform.GetChild(num).gameObject);
		}
	}

	[ContextMenu("Combine")]
	private void _E002()
	{
		WeatherObstacle.m__E000 = this;
		this.m__E002 = _E000();
		MeshCollider = base.gameObject.GetOrAddComponent<MeshCollider>();
		MeshCollider.sharedMesh = this.m__E002;
	}

	private void OnDestroy()
	{
		Object.Destroy(MeshCollider);
		WeatherObstacle.m__E000 = null;
	}
}
