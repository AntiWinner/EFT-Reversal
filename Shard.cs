using UnityEngine;

public class Shard : MonoBehaviour
{
	private MeshFilter m__E000;

	private MeshCollider m__E001;

	public Mesh Mesh => this.m__E000.mesh;

	private void Awake()
	{
		base.gameObject.AddComponent<MeshRenderer>();
		base.gameObject.AddComponent<Rigidbody>();
		this.m__E000 = base.gameObject.AddComponent<MeshFilter>();
		this.m__E001 = base.gameObject.AddComponent<MeshCollider>();
		this.m__E001.convex = true;
		this.m__E000.mesh = new Mesh
		{
			name = _ED3E._E000(85581)
		};
		base.gameObject.SetActive(value: false);
	}

	internal void _E000()
	{
		base.transform.parent = null;
		base.gameObject.SetActive(value: true);
	}

	private void _E001(Vector3[] verts, int[] tris, Vector2[] uvs)
	{
		int num = 0;
		Vector3[] array = new Vector3[tris.Length];
		Vector2[] array2 = new Vector2[tris.Length];
		int[] array3 = new int[tris.Length];
		for (int i = 0; i < tris.Length; i += 3)
		{
			array[num] = verts[tris[i]];
			array3[num] = num;
			array2[num] = new Vector2(1f, 0f);
			int num2 = num + 1;
			array[num2] = verts[tris[i + 1]];
			array3[num2] = num2;
			array2[num2] = new Vector2(0f, 0f);
			int num3 = num2 + 1;
			array[num3] = verts[tris[i + 2]];
			array3[num3] = num3;
			array2[num3] = new Vector2(1f, 1f);
			num = num3 + 1;
		}
		Mesh.vertices = array;
		Mesh.triangles = array3;
		Mesh.uv = array2;
		Mesh.RecalculateNormals();
		this.m__E001.sharedMesh = Mesh;
		_E000();
	}

	internal static Shard _E002(GameObject parent, Vector3[] newVertices, int[] newTriangles, Vector2[] newUVs)
	{
		Shard nextShard = ShardPool.NextShard;
		if (nextShard == null)
		{
			Debug.LogError(_ED3E._E000(85568));
			return null;
		}
		nextShard.name = _ED3E._E000(85600);
		nextShard._E001(newVertices, newTriangles, newUVs);
		nextShard.GetComponent<Renderer>().material = parent.GetComponent<Renderer>().material;
		if (parent.GetComponent<Rigidbody>() != null)
		{
			nextShard.GetComponent<Rigidbody>().mass = parent.GetComponent<Rigidbody>().mass;
			nextShard.GetComponent<Rigidbody>().velocity = parent.GetComponent<Rigidbody>().velocity;
		}
		return nextShard;
	}
}
