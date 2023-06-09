using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BaseFracture : MonoBehaviour
{
	protected static readonly Vector4[] InitPlanes = new Vector4[6]
	{
		Vector3.right,
		Vector3.up,
		Vector3.forward,
		Vector3.left,
		Vector3.down,
		Vector3.back
	};

	[SerializeField]
	private bool _immediate;

	[SerializeField]
	protected RangedInt Shards;

	[SerializeField]
	protected int ShardsPerFrame;

	protected List<Shard> FinishedShards = new List<Shard>();

	protected Vector3 MaxBounds;

	protected Vector3 MinBounds;

	protected Vector3[] NewVertices;

	protected Vector2[] NewUVs;

	protected int[] NewTriangles;

	protected List<Vector3> Vertices = new List<Vector3>();

	protected List<int> PlaneIndices = new List<int>();

	protected readonly List<Vector4> Planes = new List<Vector4>();

	[CompilerGenerated]
	private bool _E000;

	private bool _E001
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	internal List<Shard> _E002 => FinishedShards;

	protected abstract IEnumerator SpreadFracture(Vector3[] points);

	protected abstract void ImmediateFracture(Vector3[] points);

	protected void BeginFracture(Vector3 point, float size = 0.5f)
	{
		if (!_E001)
		{
			_E001 = true;
			Vector3[] array = new Vector3[Shards.Random()];
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = Random.insideUnitSphere * size;
				array[i] = base.transform.InverseTransformPoint(point) + vector;
			}
			if (_immediate)
			{
				ImmediateFracture(array);
			}
			else
			{
				StartCoroutine(SpreadFracture(array));
			}
		}
	}

	public void Destroy(Vector3 point, float force)
	{
		BeginFracture(point, force);
	}

	protected void InitializeDestruction()
	{
		MaxBounds = base.transform.lossyScale / 2f;
		MinBounds = -MaxBounds;
		GetComponent<Collider>().enabled = false;
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	protected void FinalizeDestruction()
	{
		foreach (Shard finishedShard in FinishedShards)
		{
			finishedShard.GetComponent<Rigidbody>().isKinematic = false;
		}
		Object.Destroy(base.gameObject);
	}

	protected void GetVerticesInPlane()
	{
		Vertices.Clear();
		PlaneIndices.Clear();
		for (int i = 0; i < Planes.Count; i++)
		{
			for (int j = i + 1; j < Planes.Count; j++)
			{
				Vector3 vector = Vector3.Cross(Planes[i], Planes[j]);
				if (!((double)vector.sqrMagnitude > 0.001))
				{
					continue;
				}
				for (int k = j + 1; k < Planes.Count; k++)
				{
					Vector3 vector2 = Vector3.Cross(Planes[j], Planes[k]);
					Vector3 vector3 = Vector3.Cross(Planes[k], Planes[i]);
					if (!((double)vector2.sqrMagnitude > 0.001) || !((double)vector3.sqrMagnitude > 0.001))
					{
						continue;
					}
					float num = Vector3.Dot(Planes[i], vector2);
					if (!((double)Mathf.Abs(num) > 0.001))
					{
						continue;
					}
					Vector3 vector4 = (vector2 * Planes[i].w + vector3 * Planes[j].w + vector * Planes[k].w) * -1f / num;
					int l;
					for (l = 0; l < Planes.Count; l++)
					{
						Vector4 vector5 = Planes[l];
						if ((double)Vector3.Dot(vector5, vector4) + (double)vector5.w > 0.001)
						{
							break;
						}
					}
					if (l == Planes.Count)
					{
						Vertices.Add(vector4);
						PlaneIndices.Add(i);
						PlaneIndices.Add(j);
						PlaneIndices.Add(k);
					}
				}
			}
		}
		Vertices = _E419.DedupCollectionWithRandom(Vertices);
		PlaneIndices = _E419.DedupCollection(PlaneIndices);
		PlaneIndices.Sort();
	}

	protected void CreateShardMesh(Vector3 point)
	{
		Shard shard = Shard._E002(base.gameObject, NewVertices, NewTriangles, NewUVs);
		if (!(shard == null))
		{
			shard.transform.position = base.transform.position + base.transform.TransformDirection(point);
			shard.transform.rotation = base.transform.rotation;
			shard.GetComponent<Rigidbody>().mass = GetComponent<Rigidbody>().mass / (float)Shards.Random();
			shard.GetComponent<Rigidbody>().isKinematic = true;
			FinishedShards.Add(shard);
		}
	}
}
