using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BotZoneDebugPoints
{
	[SerializeField]
	private string _label;

	[SerializeField]
	private List<Vector3> Points = new List<Vector3>();

	public int Count;

	public string Label => _label;

	public BotZoneDebugPoints(string label)
	{
		_label = label;
	}

	public void Clear()
	{
		Count = 0;
		Points.Clear();
	}

	public void AddPoint(Vector3 point)
	{
		Count++;
		Points.Add(point);
	}

	public void Draw(bool isDraw, Color color, Vector3 offset, float radius)
	{
		if (!isDraw)
		{
			return;
		}
		Gizmos.color = color;
		foreach (Vector3 point in Points)
		{
			Gizmos.DrawSphere(point + offset, radius);
		}
	}
}
