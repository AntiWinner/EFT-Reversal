using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpeedTreeTerrainProcessor : MonoBehaviour
{
	private class _E000
	{
		public _E001[][] LodsRenderers;

		public float[] CullingRates;

		public _E000(LODGroup lodGroup)
		{
			LOD[] lODs = lodGroup.GetLODs();
			CullingRates = _E000(lODs);
			LodsRenderers = _E001(lODs);
			_E002(LodsRenderers, lODs);
		}

		private static float[] _E000(LOD[] lods)
		{
			float[] array = new float[lods.Length];
			for (int i = 0; i < lods.Length; i++)
			{
				array[i] = lods[i].screenRelativeTransitionHeight;
			}
			return array;
		}

		private static _E001[][] _E001(LOD[] lods)
		{
			_E001[][] array = new _E001[lods.Length][];
			for (int i = 0; i < lods.Length; i++)
			{
				LOD lOD = lods[i];
				array[i] = new _E001[lOD.renderers.Length];
				for (int j = 0; j < lOD.renderers.Length; j++)
				{
					array[i][j] = new _E001(lOD.renderers[j]);
				}
			}
			return array;
		}

		private static void _E002(_E001[][] lodsRenderers, LOD[] lods)
		{
			Dictionary<Renderer, int> dictionary = new Dictionary<Renderer, int>();
			for (int i = 0; i < lods.Length; i++)
			{
				LOD lOD = lods[i];
				for (int j = 0; j < lOD.renderers.Length; j++)
				{
					if (dictionary.TryGetValue(lOD.renderers[j], out var value))
					{
						lodsRenderers[i][j].SameRendererLodNum = value;
					}
					else
					{
						dictionary.Add(lOD.renderers[j], i);
					}
				}
			}
		}
	}

	private class _E001
	{
		public int VertexCount;

		public int SubMeshCount;

		public GameObject Prefab;

		public ObjType Type;

		public int SameRendererLodNum = -1;

		public Material[] Materials;

		private ShadowCastingMode m__E000;

		private bool m__E001;

		private LightProbeUsage _E002;

		private ReflectionProbeUsage _E003;

		public _E001(Renderer renderer)
		{
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			Prefab = _E001(renderer);
			if (component == null)
			{
				Type = ObjType.Billboard;
				return;
			}
			Mesh sharedMesh = component.sharedMesh;
			VertexCount = sharedMesh.vertexCount;
			SubMeshCount = sharedMesh.subMeshCount;
			Materials = renderer.materials;
			_E000(renderer);
			Type = ((!Materials[0].shader.name.Contains(_ED3E._E000(83305))) ? ObjType.Mesh : ObjType.SpeedTree);
			if (Type == ObjType.SpeedTree)
			{
				Material[] materials = Materials;
				for (int i = 0; i < materials.Length; i++)
				{
					materials[i].shader = _E414.SpeedTreeShader;
				}
			}
		}

		private void _E000(Renderer renderer)
		{
			this.m__E000 = renderer.shadowCastingMode;
			this.m__E001 = renderer.receiveShadows;
			_E002 = renderer.lightProbeUsage;
			_E003 = renderer.reflectionProbeUsage;
		}

		public void SetRendererSettings(Renderer renderer)
		{
			renderer.sharedMaterials = Materials;
			renderer.shadowCastingMode = this.m__E000;
			renderer.receiveShadows = this.m__E001;
			renderer.lightProbeUsage = _E002;
			renderer.reflectionProbeUsage = _E003;
		}

		private static GameObject _E001(Renderer renderer)
		{
			GameObject gameObject = Object.Instantiate(renderer.gameObject);
			Component[] components = gameObject.GetComponents<Component>();
			foreach (Component component in components)
			{
				if (!(component is Transform) && !(component is Tree))
				{
					Object.DestroyImmediate(component);
				}
			}
			return gameObject;
		}
	}

	private enum ObjType
	{
		SpeedTree,
		Billboard,
		Mesh
	}

	public Terrain Terrain;

	public Shader SpeedTreeShader;

	public bool SpawnTerrainPrefabs;

	public Transform SpawnTarget;

	public bool CombineOrdinaryMeshes;

	public float CellSize = 50f;

	private void OnDrawGizmosSelected()
	{
		if (!(Terrain == null))
		{
			Vector3 size = Terrain.terrainData.size;
			int num = (int)(size.x / CellSize);
			int num2 = (int)(size.z / CellSize);
			Vector2 vector = new Vector2(size.x / (float)num, size.z / (float)num2);
			Vector3 position = Terrain.GetPosition();
			Vector3 vector2 = position + size;
			Gizmos.color = Color.red;
			float num3 = position.x;
			for (int i = 0; i < num; i++)
			{
				Gizmos.DrawLine(new Vector3(num3, position.y, position.z), new Vector3(num3, position.y, vector2.z));
				num3 += vector.x;
			}
			float num4 = position.z;
			for (int j = 0; j < num; j++)
			{
				Gizmos.DrawLine(new Vector3(position.x, position.y, num4), new Vector3(vector2.x, position.y, num4));
				num4 += vector.y;
			}
		}
	}

	private static LODGroup[,][] _E000(Terrain terrain, IEnumerable<LODGroup> lodGroups, float cellSize)
	{
		Vector3 size = terrain.terrainData.size;
		int num = (int)(size.x / cellSize);
		int num2 = (int)(size.z / cellSize);
		Vector2 vector = new Vector2(size.x / (float)num, size.z / (float)num2);
		Vector2 vector2 = new Vector2(1f / vector.x, 1f / vector.y);
		List<LODGroup>[,] array = new List<LODGroup>[num, num2];
		foreach (LODGroup lodGroup in lodGroups)
		{
			Vector3 localPosition = lodGroup.transform.localPosition;
			int num3 = (int)(localPosition.x * vector2.x);
			int num4 = (int)(localPosition.z * vector2.y);
			(array[num3, num4] ?? (array[num3, num4] = new List<LODGroup>())).Add(lodGroup);
		}
		LODGroup[,][] array2 = new LODGroup[num, num2][];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (array[j, i] != null)
				{
					array2[j, i] = array[j, i].ToArray();
				}
			}
		}
		return array2;
	}

	private static Dictionary<int, _E000> _E001(IEnumerable<LODGroup> lodGroups)
	{
		Dictionary<int, _E000> dictionary = new Dictionary<int, _E000>();
		foreach (LODGroup lodGroup in lodGroups)
		{
			int key = _E00F(lodGroup);
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, new _E000(lodGroup));
			}
		}
		return dictionary;
	}

	public static void UpdateTreesInteractiveParts(Terrain terrain)
	{
		Debug.LogWarning(_ED3E._E000(83200));
	}

	private static void _E002(Terrain terrain, Transform spawnTarget)
	{
		TerrainData terrainData = terrain.terrainData;
		TreePrototype[] treePrototypes = terrainData.treePrototypes;
		TreeInstance[] treeInstances = terrainData.treeInstances;
		Vector3 size = terrainData.size;
		Vector3 position = terrain.GetPosition();
		TreeInstance[] array = treeInstances;
		for (int i = 0; i < array.Length; i++)
		{
			TreeInstance treeInstance = array[i];
			GameObject prefab = treePrototypes[treeInstance.prototypeIndex].prefab;
			Vector3 position2 = Vector3.Scale(treeInstance.position, size) + position;
			Quaternion rotation = Quaternion.AngleAxis(treeInstance.rotation * 57.29578f, Vector3.up);
			GameObject obj = Object.Instantiate(prefab, position2, rotation);
			obj.transform.localScale = Vector3.one * treeInstance.heightScale;
			obj.transform.parent = spawnTarget;
		}
	}

	private static Dictionary<int, List<LODGroup>> _E003(LODGroup[] groups)
	{
		Dictionary<int, List<LODGroup>> dictionary = new Dictionary<int, List<LODGroup>>();
		foreach (LODGroup lODGroup in groups)
		{
			int key = _E00F(lODGroup);
			if (!dictionary.TryGetValue(key, out var value))
			{
				dictionary.Add(key, value = new List<LODGroup>());
			}
			value.Add(lODGroup);
		}
		return dictionary;
	}

	private void _E004(LODGroup[,][] grid, Dictionary<int, _E000> prototypes)
	{
		int length = grid.GetLength(0);
		int length2 = grid.GetLength(1);
		for (int i = 0; i < length2; i++)
		{
			for (int j = 0; j < length; j++)
			{
				if (grid[j, i] != null)
				{
					_E005(grid[j, i], prototypes, _ED3E._E000(45975) + j + _ED3E._E000(10270) + i + _ED3E._E000(11164));
				}
			}
		}
	}

	private void _E005(LODGroup[] cellGroups, Dictionary<int, _E000> prototypes, string name)
	{
		foreach (KeyValuePair<int, List<LODGroup>> item in _E003(cellGroups))
		{
			_E000 prototype = prototypes[item.Key];
			List<LODGroup> value = item.Value;
			LOD[][] array = new LOD[value.Count][];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value[i].GetLODs();
			}
			List<Renderer>[] lodsRenderersList = _E006(prototype, array);
			foreach (LODGroup item2 in value)
			{
				Object.DestroyImmediate(item2.gameObject);
			}
			_E00D(lodsRenderersList, prototype, name);
		}
	}

	private List<Renderer>[] _E006(_E000 prototype, LOD[][] lods)
	{
		List<Renderer>[] array = new List<Renderer>[prototype.LodsRenderers.Length];
		for (int i = 0; i < prototype.LodsRenderers.Length; i++)
		{
			array[i] = new List<Renderer>();
			for (int j = 0; j < prototype.LodsRenderers[i].Length; j++)
			{
				_E001 obj = prototype.LodsRenderers[i][j];
				if (obj.SameRendererLodNum >= 0)
				{
					_E007(array, i, obj);
					continue;
				}
				switch (obj.Type)
				{
				case ObjType.Billboard:
					_E00A(array, lods, i, j, obj);
					break;
				case ObjType.SpeedTree:
					_E00B(array, lods, i, j, obj);
					break;
				case ObjType.Mesh:
					if (CombineOrdinaryMeshes)
					{
						_E009(array, lods, i, j, obj);
					}
					break;
				}
			}
		}
		return array;
	}

	private static void _E007(List<Renderer>[] lodsRenderersList, int lodNum, _E001 prototypeRenderer)
	{
		List<Renderer> obj = lodsRenderersList[prototypeRenderer.SameRendererLodNum];
		Material material = prototypeRenderer.Materials[0];
		foreach (Renderer item in obj)
		{
			if (item.sharedMaterial == material)
			{
				lodsRenderersList[lodNum].Add(item);
			}
		}
	}

	private static Mesh _E008(MeshFilter[] meshFilters, Vector3 commonPosition)
	{
		CombineInstance[] array = new CombineInstance[meshFilters.Length];
		int num = 0;
		foreach (MeshFilter meshFilter in meshFilters)
		{
			meshFilter.transform.position -= commonPosition;
			array[num++] = new CombineInstance
			{
				mesh = meshFilter.mesh,
				transform = meshFilter.transform.localToWorldMatrix
			};
		}
		Mesh mesh = new Mesh();
		mesh.name = _ED3E._E000(83284);
		mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
		return mesh;
	}

	private void _E009(List<Renderer>[] lodsRenderersList, LOD[][] lods, int lodNum, int rendererNum, _E001 prototypeRenderer)
	{
		MeshFilter[] array = new MeshFilter[lods.Length];
		for (int i = 0; i < lods.Length; i++)
		{
			LOD[] array2 = lods[i];
			array[i] = array2[lodNum].renderers[rendererNum].GetComponent<MeshFilter>();
		}
		Vector3 zero = Vector3.zero;
		MeshFilter[] array3 = array;
		foreach (MeshFilter meshFilter in array3)
		{
			zero += meshFilter.transform.position;
		}
		zero /= (float)array.Length;
		Mesh sharedMesh = _E008(array, zero);
		GameObject obj = new GameObject(_ED3E._E000(83308));
		obj.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		prototypeRenderer.SetRendererSettings(meshRenderer);
		obj.transform.position = zero;
		obj.transform.parent = Terrain.transform;
		array3 = array;
		for (int j = 0; j < array3.Length; j++)
		{
			Object.Destroy(array3[j].gameObject);
		}
		lodsRenderersList[lodNum].Add(meshRenderer);
	}

	private static void _E00A(List<Renderer>[] lodsRenderersList, LOD[][] lods, int lodNum, int rendererNum, _E001 prototypeRenderer)
	{
		for (int i = 0; i < lods.Length; i++)
		{
			Renderer renderer = lods[i][lodNum].renderers[rendererNum];
			renderer.transform.parent = null;
			lodsRenderersList[lodNum].Add(renderer);
		}
	}

	private void _E00B(List<Renderer>[] lodsRenderersList, LOD[][] lods, int lodNum, int rendererNum, _E001 prototypeRenderer)
	{
		LinkedList<MeshFilter> linkedList = new LinkedList<MeshFilter>();
		int num = 0;
		foreach (LOD[] array in lods)
		{
			num += prototypeRenderer.VertexCount;
			if (num > 65535)
			{
				lodsRenderersList[lodNum].Add(_E00C(linkedList, prototypeRenderer));
				linkedList.Clear();
				num = prototypeRenderer.VertexCount;
			}
			linkedList.AddLast(array[lodNum].renderers[rendererNum].GetComponent<MeshFilter>());
		}
		if (linkedList.Count > 0)
		{
			lodsRenderersList[lodNum].Add(_E00C(linkedList, prototypeRenderer));
			linkedList.Clear();
		}
	}

	private MeshRenderer _E00C(LinkedList<MeshFilter> meshFiltersList, _E001 prototypeRenderer)
	{
		Vector3 zero = Vector3.zero;
		foreach (MeshFilter meshFilters in meshFiltersList)
		{
			zero += meshFilters.transform.position;
		}
		zero /= (float)meshFiltersList.Count;
		Mesh sharedMesh = _E414.Combine(meshFiltersList, zero, prototypeRenderer.SubMeshCount);
		GameObject obj = Object.Instantiate(prototypeRenderer.Prefab);
		obj.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		prototypeRenderer.SetRendererSettings(meshRenderer);
		obj.transform.position = zero;
		obj.transform.parent = Terrain.transform;
		foreach (MeshFilter meshFilters2 in meshFiltersList)
		{
			Object.Destroy(meshFilters2.gameObject);
		}
		return meshRenderer;
	}

	private void _E00D(List<Renderer>[] lodsRenderersList, _E000 prototype, string name)
	{
		Vector3 zero = Vector3.zero;
		int num = 0;
		List<Renderer>[] array = lodsRenderersList;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Renderer item in array[i])
			{
				zero += item.transform.position;
				num++;
			}
		}
		zero /= (float)num;
		GameObject obj = new GameObject(name);
		LODGroup lodGroup = obj.AddComponent<LODGroup>();
		Transform transform = obj.transform;
		transform.position = zero;
		transform.SetParent(Terrain.transform);
		array = lodsRenderersList;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Renderer item2 in array[i])
			{
				item2.transform.SetParent(transform);
			}
		}
		_E00E(lodGroup, lodsRenderersList, prototype.CullingRates);
	}

	private static void _E00E(LODGroup lodGroup, List<Renderer>[] lodsRenderersList, float[] cullingRates)
	{
		LOD[] array = new LOD[lodsRenderersList.Length];
		for (int i = 0; i < array.Length; i++)
		{
			Renderer[] renderers = lodsRenderersList[i].ToArray();
			array[i] = new LOD(cullingRates[i], renderers);
		}
		lodGroup.SetLODs(array);
		lodGroup.fadeMode = LODFadeMode.CrossFade;
		lodGroup.animateCrossFading = true;
		lodGroup.size *= 0.3f;
	}

	private static int _E00F(LODGroup lodGroup)
	{
		MeshFilter componentInChildren = lodGroup.GetComponentInChildren<MeshFilter>();
		if (componentInChildren == null)
		{
			return -1;
		}
		return componentInChildren.sharedMesh.GetInstanceID();
	}
}
