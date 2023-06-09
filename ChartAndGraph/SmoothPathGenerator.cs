using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChartAndGraph;

public abstract class SmoothPathGenerator : PathGenerator
{
	public int JointSmoothing = 2;

	public float JointSize = 0.1f;

	protected List<int> SkipJoints = new List<int>();

	private readonly List<Vector3> _E00C = new List<Vector3>();

	protected List<Vector3> TmpCenters = new List<Vector3>();

	private MeshFilter _E00D;

	private Mesh _E00E;

	protected virtual int JointSmoothingLink => JointSmoothing;

	protected virtual float JointSizeLink => JointSize;

	public override void Clear()
	{
		_ED15._E00D(null, ref _E00E);
	}

	public void OnDestroy()
	{
		Clear();
	}

	protected bool EnsureMeshFilter()
	{
		if (_E00D == null)
		{
			_E00D = GetComponent<MeshFilter>();
		}
		return _E00D != null;
	}

	public void SetMesh(Mesh mesh)
	{
		mesh.hideFlags = HideFlags.DontSave;
		_E00D.sharedMesh = mesh;
		_ED15._E00D(mesh, ref _E00E);
		MeshCollider component = GetComponent<MeshCollider>();
		if (component != null)
		{
			component.sharedMesh = mesh;
		}
	}

	protected Quaternion LookRotation(Vector3 diff)
	{
		if (diff.sqrMagnitude < float.Epsilon)
		{
			return Quaternion.identity;
		}
		Vector3 normalized = new Vector3(diff.y, 0f - diff.x, 0f).normalized;
		return Quaternion.LookRotation(diff, normalized);
	}

	private void _E000(Vector3 from, Vector3 curr, Vector3 to)
	{
		if (JointSmoothingLink <= 0)
		{
			_E00C.Add(curr);
			return;
		}
		for (int i = 0; i < JointSmoothingLink; i++)
		{
			float num = (float)(i + 1) / (float)(JointSmoothingLink + 1);
			float num2 = 1f - num;
			Vector3 item = num2 * num2 * from + 2f * num2 * num * curr + num * num * to;
			_E00C.Add(item);
		}
	}

	protected void ModifyPath(Vector3[] path, bool closed, List<Vector3> res)
	{
		if (path.Length <= 1)
		{
			return;
		}
		_E00C.Clear();
		for (int i = 0; i <= path.Length; i++)
		{
			if ((i == 0 && !closed) || SkipJoints.Contains(i))
			{
				if (i < path.Length)
				{
					_E00C.Add(path[i]);
				}
			}
			else if (i < path.Length - 1 || closed)
			{
				int num = i - 1;
				if (num < 0)
				{
					num = path.Length - 1;
				}
				Vector3 vector = path[num];
				Vector3 vector2 = path[i % path.Length];
				int num2 = i + 1;
				Vector3 vector3 = path[num2 % path.Length];
				Vector3 vector4 = vector2 - vector;
				Vector3 vector5 = vector3 - vector2;
				float magnitude = vector4.magnitude;
				float magnitude2 = vector5.magnitude;
				vector4.Normalize();
				vector5.Normalize();
				float num3 = Math.Min(JointSizeLink, magnitude * 0.5f);
				float num4 = Math.Min(JointSizeLink, magnitude2 * 0.5f);
				Vector3 item = vector2 - vector4 * num3;
				Vector3 item2 = vector2 + vector5 * num4;
				Vector3 vector6 = vector2 - vector4 * num3 * 0.7f;
				Vector3 vector7 = vector2 + vector5 * num4 * 0.7f;
				if (i > 0)
				{
					_E00C.Add(item);
					_E00C.Add(vector6);
					_E000(vector6, vector2, vector7);
					_E00C.Add(vector7);
				}
				_E00C.Add(item2);
			}
		}
		if (!closed)
		{
			_E00C.Add(path[path.Length - 1]);
		}
		res.Clear();
		res.Add(_E00C[0]);
		for (int j = 1; j < _E00C.Count; j++)
		{
			Vector3 vector8 = _E00C[j - 1];
			Vector3 vector9 = _E00C[j];
			if (!((double)(vector9 - vector8).sqrMagnitude < 1E-06))
			{
				res.Add(vector9);
			}
		}
	}

	protected void ModifyPath(Vector3[] path, bool closed)
	{
		ModifyPath(path, closed, TmpCenters);
	}
}
