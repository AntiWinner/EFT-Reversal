using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BotZoneDebug : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string groupName;

		internal bool _E000(BotZoneDebugPointsGroup p)
		{
			return p.Label == groupName;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string groupName;

		internal bool _E000(BotZoneDebugLinesGroup p)
		{
			return p.Label == groupName;
		}
	}

	[SerializeField]
	private List<BotZoneDebugPointsGroup> _points = new List<BotZoneDebugPointsGroup>();

	[SerializeField]
	private List<BotZoneDebugLinesGroup> _lines = new List<BotZoneDebugLinesGroup>();

	public List<BotZoneDebugPointsGroup> Points => _points;

	public List<BotZoneDebugLinesGroup> Lines => _lines;

	public void Clear()
	{
		_points.Clear();
		_lines.Clear();
	}

	public void Add(string groupName, Vector3 point)
	{
		Add(groupName, _ED3E._E000(30808), point);
	}

	public void Add(string groupName, string label, Vector3 point)
	{
		BotZoneDebugPointsGroup botZoneDebugPointsGroup = _points.FirstOrDefault((BotZoneDebugPointsGroup p) => p.Label == groupName);
		if (botZoneDebugPointsGroup == null)
		{
			botZoneDebugPointsGroup = new BotZoneDebugPointsGroup(groupName);
			_points.Add(botZoneDebugPointsGroup);
		}
		botZoneDebugPointsGroup.Add(label, point);
	}

	public void AddLine(string groupName, Vector3 pointA, Vector3 pointB)
	{
		AddLine(groupName, _ED3E._E000(30808), pointA, pointB);
	}

	public void AddLine(string groupName, string label, Vector3 pointA, Vector3 pointB)
	{
		BotZoneDebugLinesGroup botZoneDebugLinesGroup = _lines.FirstOrDefault((BotZoneDebugLinesGroup p) => p.Label == groupName);
		if (botZoneDebugLinesGroup == null)
		{
			botZoneDebugLinesGroup = new BotZoneDebugLinesGroup(groupName);
			_lines.Add(botZoneDebugLinesGroup);
		}
		botZoneDebugLinesGroup.AddLine(label, pointA, pointB);
	}

	private void OnDrawGizmos()
	{
		foreach (BotZoneDebugPointsGroup point in _points)
		{
			point.Draw();
		}
		foreach (BotZoneDebugLinesGroup line in _lines)
		{
			line.Draw();
		}
	}

	public void DrawPoints(List<Vector3> points, Color color, float delta)
	{
		Gizmos.color = color;
		foreach (Vector3 point in points)
		{
			Gizmos.DrawSphere(point + Vector3.up * delta, 0.1f);
		}
	}
}
