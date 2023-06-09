using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChartAndGraph;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CylinderPathGenerator : SmoothPathGenerator
{
	public int CircleVertices = 10;

	[Range(0.01f, 10f)]
	public float HeightRatio = 1f;

	private Vector3[] _E003;

	private Vector3[] _E004;

	private List<int> m__E000 = new List<int>();

	private List<Vector2> m__E001 = new List<Vector2>();

	private List<Vector3> m__E002 = new List<Vector3>();

	private void _E000()
	{
		if (_E003 == null || _E003.Length != CircleVertices)
		{
			_E003 = new Vector3[CircleVertices];
			for (int i = 0; i < CircleVertices; i++)
			{
				float f = (float)i / (float)CircleVertices * (float)Math.PI * 2f;
				_E003[i] = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
			}
			_E004 = new Vector3[CircleVertices];
		}
	}

	private int _E001(float thickness, Quaternion angle, Vector3 center, float u)
	{
		_E000();
		_E003.CopyTo(_E004, 0);
		for (int i = 0; i < _E004.Length; i++)
		{
			_E004[i] *= thickness;
			_E004[i].y *= HeightRatio;
			_E004[i] = angle * _E004[i];
			_E004[i] += center;
			float y = (float)i / (float)(_E004.Length - 1);
			this.m__E001.Add(new Vector2(u, y));
		}
		int count = this.m__E002.Count;
		this.m__E002.AddRange(_E004);
		return count;
	}

	private void _E002(List<int> tringles, int from, int to)
	{
		if (CircleVertices <= 1)
		{
			return;
		}
		for (int i = 0; i < CircleVertices; i++)
		{
			int num = i - 1;
			if (num < 0)
			{
				num = CircleVertices - 1;
			}
			int item = from + num;
			int item2 = to + num;
			int item3 = from + i;
			int item4 = to + i;
			tringles.Add(item);
			tringles.Add(item3);
			tringles.Add(item4);
			tringles.Add(item4);
			tringles.Add(item2);
			tringles.Add(item);
		}
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
		this.m__E002.Clear();
		ModifyPath(path, closed);
		if (TmpCenters.Count <= 1)
		{
			return;
		}
		float num = 0f;
		int from = _E001(thickness, LookRotation(TmpCenters[1] - TmpCenters[0]), TmpCenters[0], num);
		if (!closed)
		{
			int num2 = _E001(thickness, LookRotation(TmpCenters[1] - TmpCenters[0]), TmpCenters[0], num);
			this.m__E002.Add(TmpCenters[0]);
			this.m__E001.Add(new Vector2(0f, 0.5f));
			for (int i = 0; i < CircleVertices; i++)
			{
				int num3 = (i + 1) % CircleVertices;
				this.m__E000.Add(num2 + CircleVertices);
				this.m__E000.Add(num2 + num3);
				this.m__E000.Add(num2 + i);
			}
		}
		Vector3 vector = Vector3.zero;
		Quaternion angle = Quaternion.identity;
		for (int j = 1; j < TmpCenters.Count; j++)
		{
			Vector3 vector2 = TmpCenters[j - 1];
			vector = TmpCenters[j];
			Vector3 diff = vector - vector2;
			float magnitude = diff.magnitude;
			num += magnitude;
			angle = LookRotation(diff);
			int num4 = _E001(thickness, angle, vector, 0f);
			_E002(this.m__E000, from, num4);
			from = num4;
		}
		if (!closed)
		{
			int num5 = _E001(thickness, angle, vector, 1f);
			this.m__E002.Add(vector);
			this.m__E001.Add(new Vector2(1f, 0.5f));
			for (int k = 0; k < CircleVertices; k++)
			{
				int num6 = (k + 1) % CircleVertices;
				this.m__E000.Add(num5 + num6);
				this.m__E000.Add(num5 + CircleVertices);
				this.m__E000.Add(num5 + k);
			}
		}
		for (int l = 0; l < this.m__E001.Count; l++)
		{
			Vector2 value = this.m__E001[l];
			value.x /= num;
			this.m__E001[l] = value;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = this.m__E002.ToArray();
		mesh.uv = this.m__E001.ToArray();
		mesh.triangles = this.m__E000.ToArray();
		mesh.RecalculateNormals();
		SetMesh(mesh);
		this.m__E000.Clear();
		this.m__E001.Clear();
		this.m__E002.Clear();
	}
}
