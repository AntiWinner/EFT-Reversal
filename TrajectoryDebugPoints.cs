using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrajectoryDebugPoints
{
	[SerializeField]
	private string _label;

	[SerializeField]
	private List<Vector3> Points = new List<Vector3>();

	[SerializeField]
	private List<float> Times = new List<float>();

	public int Count;

	public string Label => _label;

	public float MinTime
	{
		get
		{
			if (Times.Count == 0)
			{
				return 0f;
			}
			return Times[0];
		}
	}

	public float MaxTime
	{
		get
		{
			if (Times.Count == 0)
			{
				return 0f;
			}
			return Times[Times.Count - 1];
		}
	}

	public TrajectoryDebugPoints(string label)
	{
		_label = label;
	}

	public void Clear()
	{
		Count = 0;
		Points.Clear();
	}

	public void AddPoint(Vector3 point, float time)
	{
		Count++;
		Points.Add(point);
		Times.Add(time);
	}

	public void Draw(bool isDraw, Color color, Vector3 offset, float radius, float minTime, float maxTime)
	{
		if (!isDraw)
		{
			return;
		}
		Gizmos.color = color;
		Vector3? vector = null;
		for (int i = 0; i < Points.Count; i++)
		{
			Vector3 vector2 = Points[i];
			float num = Times[i];
			if (num >= minTime && num <= maxTime)
			{
				if (vector.HasValue)
				{
					Gizmos.DrawLine(vector.Value + offset, vector2 + offset);
				}
				Gizmos.DrawSphere(vector2 + offset, radius);
				vector = vector2;
			}
		}
	}
}
