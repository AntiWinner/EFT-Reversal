using System.Collections.Generic;
using UnityEngine;

namespace ChartAndGraph;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BoxPathGenerator : SmoothPathGenerator
{
	[Range(0f, 10f)]
	public float HeightRatio = 1f;

	private List<int> m__E000 = new List<int>();

	private List<Vector2> m__E001 = new List<Vector2>();

	private List<Vector3> _E002 = new List<Vector3>();

	private int _E000(float thickness, Quaternion rotation, Vector3 center, float u)
	{
		Vector3 vector = new Vector3(thickness, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, thickness * HeightRatio, 0f);
		int count = _E002.Count;
		_E002.Add(center + rotation * (vector + vector2));
		_E002.Add(center + rotation * (-vector + vector2));
		_E002.Add(center + rotation * (-vector - vector2));
		_E002.Add(center + rotation * (vector - vector2));
		_E002.Add(_E002[count]);
		_E002.Add(_E002[count + 1]);
		_E002.Add(_E002[count + 2]);
		_E002.Add(_E002[count + 3]);
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 1f));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 1f));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 1f));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 1f));
		return count;
	}

	private void _E001(List<int> tringles, int from, int to)
	{
		tringles.Add(from + 1);
		tringles.Add(to);
		tringles.Add(from);
		tringles.Add(to + 1);
		tringles.Add(to);
		tringles.Add(from + 1);
		tringles.Add(from + 2);
		tringles.Add(to + 5);
		tringles.Add(from + 5);
		tringles.Add(to + 2);
		tringles.Add(to + 5);
		tringles.Add(from + 2);
		tringles.Add(from + 3);
		tringles.Add(to + 6);
		tringles.Add(from + 6);
		tringles.Add(to + 3);
		tringles.Add(to + 6);
		tringles.Add(from + 3);
		tringles.Add(from + 4);
		tringles.Add(to + 7);
		tringles.Add(from + 7);
		tringles.Add(to + 4);
		tringles.Add(to + 7);
		tringles.Add(from + 4);
	}

	public override void Generator(Vector3[] path, float thickness, bool closed)
	{
		if (!EnsureMeshFilter())
		{
			return;
		}
		Clear();
		if (path.Length <= 1)
		{
			return;
		}
		this.m__E000.Clear();
		this.m__E001.Clear();
		_E002.Clear();
		ModifyPath(path, closed);
		if (TmpCenters.Count <= 1)
		{
			return;
		}
		float num = 0f;
		if (!closed)
		{
			int num2 = _E000(thickness, LookRotation(TmpCenters[1] - TmpCenters[0]), TmpCenters[0], num);
			this.m__E000.Add(num2 + 3);
			this.m__E000.Add(num2 + 1);
			this.m__E000.Add(num2);
			this.m__E000.Add(num2 + 3);
			this.m__E000.Add(num2 + 2);
			this.m__E000.Add(num2 + 1);
		}
		int from = _E000(thickness, LookRotation(TmpCenters[1] - TmpCenters[0]), TmpCenters[0], num);
		Quaternion quaternion = Quaternion.identity;
		Vector3 vector = Vector3.zero;
		for (int i = 1; i < TmpCenters.Count; i++)
		{
			Vector3 vector2 = TmpCenters[i - 1];
			vector = TmpCenters[i];
			Vector3 diff = vector - vector2;
			float magnitude = diff.magnitude;
			if (!(magnitude <= 0.0001f))
			{
				num += magnitude;
				quaternion = LookRotation(diff);
				if (i + 1 < TmpCenters.Count)
				{
					Vector3 vector3 = TmpCenters[i + 1];
					Quaternion b = LookRotation(vector3 - vector);
					quaternion = Quaternion.Lerp(quaternion, b, 0.5f);
				}
				int num3 = _E000(thickness, quaternion, vector, num);
				_E001(this.m__E000, from, num3);
				from = num3;
			}
		}
		if (!closed)
		{
			int num4 = _E000(thickness, quaternion, vector, num);
			this.m__E000.Add(num4);
			this.m__E000.Add(num4 + 1);
			this.m__E000.Add(num4 + 3);
			this.m__E000.Add(num4 + 1);
			this.m__E000.Add(num4 + 2);
			this.m__E000.Add(num4 + 3);
		}
		for (int j = 0; j < this.m__E001.Count; j++)
		{
			Vector2 value = this.m__E001[j];
			value.x /= num;
			this.m__E001[j] = value;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = _E002.ToArray();
		mesh.uv = this.m__E001.ToArray();
		mesh.triangles = this.m__E000.ToArray();
		mesh.RecalculateNormals();
		SetMesh(mesh);
		this.m__E000.Clear();
		this.m__E001.Clear();
		_E002.Clear();
	}
}
