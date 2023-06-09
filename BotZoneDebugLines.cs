using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BotZoneDebugLines
{
	[SerializeField]
	private string _label;

	[SerializeField]
	private List<Vector3> lineA = new List<Vector3>();

	[SerializeField]
	private List<Vector3> lineB = new List<Vector3>();

	public int Count;

	public string Label => _label;

	public BotZoneDebugLines(string label)
	{
		_label = label;
	}

	public void Clear()
	{
		Count = 0;
		lineA.Clear();
		lineB.Clear();
	}

	public void AddLine(Vector3 a, Vector3 b)
	{
		Count++;
		lineA.Add(a);
		lineB.Add(b);
	}

	public void Draw(bool isDraw, Color color, Vector3 offset)
	{
		if (isDraw)
		{
			Gizmos.color = color;
			for (int i = 0; i < lineA.Count; i++)
			{
				Gizmos.DrawLine(lineA[i] + offset, lineB[i] + offset);
			}
		}
	}
}
