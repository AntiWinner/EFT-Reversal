using System.Collections.Generic;
using UnityEngine;

namespace ChartAndGraph;

public class FillPathGenerator : SmoothPathGenerator
{
	public bool WithTop = true;

	public bool MatchLine = true;

	private float _E005;

	private float _E006 = 10f;

	private bool _E007;

	private List<int> m__E000 = new List<int>();

	private List<Vector2> m__E001 = new List<Vector2>();

	private List<Vector3> _E002 = new List<Vector3>();

	private bool _E008;

	private int _E009;

	private float _E00A;

	protected override float JointSizeLink
	{
		get
		{
			if (MatchLine && _E008)
			{
				return _E00A;
			}
			return base.JointSizeLink;
		}
	}

	protected override int JointSmoothingLink
	{
		get
		{
			if (MatchLine && _E008)
			{
				return _E009;
			}
			return base.JointSmoothingLink;
		}
	}

	public void SetLineSmoothing(bool hasParent, int jointSmoothing, float jointSize)
	{
		_E008 = hasParent;
		_E009 = jointSmoothing;
		_E00A = jointSize;
	}

	public void SetStrechFill(bool strech)
	{
		_E007 = strech;
	}

	public void SetGraphBounds(float bottom, float top)
	{
		_E005 = bottom;
		_E006 = top;
	}

	private int _E000(Vector3 position, float thickness, float u)
	{
		Vector3 vector = new Vector3(0f, 0f, thickness);
		Vector3 vector2 = new Vector3(position.x, _E005, position.z);
		int count = _E002.Count;
		float y = 1f;
		if (!_E007)
		{
			y = Mathf.Abs((position.y - _E005) / (_E005 - _E006));
		}
		_E002.Add(position - vector);
		_E002.Add(position + vector);
		_E002.Add(vector2 - vector);
		_E002.Add(vector2 + vector);
		_E002.Add(_E002[count + 2]);
		_E002.Add(_E002[count + 3]);
		this.m__E001.Add(new Vector2(u, y));
		this.m__E001.Add(new Vector2(u, y));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 0f));
		this.m__E001.Add(new Vector2(u, 0f));
		if (WithTop)
		{
			_E002.Add(_E002[count]);
			_E002.Add(_E002[count + 1]);
			this.m__E001.Add(new Vector2(u, y));
			this.m__E001.Add(new Vector2(u, y));
		}
		return count;
	}

	private void _E001(List<int> tringles, int from, int to)
	{
		tringles.Add(from);
		tringles.Add(to);
		tringles.Add(from + 2);
		tringles.Add(to);
		tringles.Add(to + 2);
		tringles.Add(from + 2);
		tringles.Add(from + 3);
		tringles.Add(to + 1);
		tringles.Add(from + 1);
		tringles.Add(from + 3);
		tringles.Add(to + 3);
		tringles.Add(to + 1);
		tringles.Add(from + 4);
		tringles.Add(to + 4);
		tringles.Add(from + 5);
		tringles.Add(to + 4);
		tringles.Add(to + 5);
		tringles.Add(from + 5);
		if (WithTop)
		{
			tringles.Add(from + 7);
			tringles.Add(to + 6);
			tringles.Add(from + 6);
			tringles.Add(from + 7);
			tringles.Add(to + 7);
			tringles.Add(to + 6);
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
		_E002.Clear();
		ModifyPath(path, closed);
		if (TmpCenters.Count > 1)
		{
			float num = 0f;
			int num2 = _E000(TmpCenters[0], thickness, num);
			this.m__E000.Add(num2 + 2);
			this.m__E000.Add(num2 + 1);
			this.m__E000.Add(num2);
			this.m__E000.Add(num2 + 1);
			this.m__E000.Add(num2 + 2);
			this.m__E000.Add(num2 + 3);
			int from = _E000(TmpCenters[0], thickness, num);
			for (int i = 1; i < TmpCenters.Count; i++)
			{
				Vector3 vector = TmpCenters[i - 1];
				Vector3 vector2 = TmpCenters[i];
				float magnitude = (vector2 - vector).magnitude;
				num += magnitude;
				int num3 = _E000(vector2, thickness, num);
				_E001(this.m__E000, from, num3);
				from = num3;
			}
			int num4 = _E000(TmpCenters[TmpCenters.Count - 1], thickness, num);
			this.m__E000.Add(num4);
			this.m__E000.Add(num4 + 1);
			this.m__E000.Add(num4 + 2);
			this.m__E000.Add(num4 + 3);
			this.m__E000.Add(num4 + 2);
			this.m__E000.Add(num4 + 1);
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
}
