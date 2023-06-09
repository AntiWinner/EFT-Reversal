using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class TrajectoryDebugPointsGroup
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string label;

		internal bool _E000(TrajectoryDebugPoints p)
		{
			return p.Label == label;
		}
	}

	[SerializeField]
	protected string _label;

	public bool IsDraw;

	public Color Color = Color.magenta;

	public Vector3 Offset = Vector3.zero;

	public float Radius = 0.1f;

	private List<Color> _colors = new List<Color>
	{
		Color.white,
		Color.red,
		Color.green,
		Color.blue,
		Color.cyan,
		Color.yellow,
		Color.magenta,
		Color.gray,
		Color.black
	};

	private static int _colorIndex;

	[SerializeField]
	protected List<TrajectoryDebugPoints> _points = new List<TrajectoryDebugPoints>();

	public int ActivePointsIndex;

	public float MinTime;

	public float MaxTime;

	public string Label => _label;

	public List<TrajectoryDebugPoints> Points => _points;

	public TrajectoryDebugPoints List
	{
		get
		{
			if (ActivePointsIndex >= _points.Count)
			{
				return null;
			}
			return Points[ActivePointsIndex];
		}
	}

	public float MinTimeLimit
	{
		get
		{
			float minTime = _points[0].MinTime;
			for (int i = 1; i < _points.Count; i++)
			{
				if (_points[i].MinTime < minTime)
				{
					minTime = _points[i].MinTime;
				}
			}
			return minTime;
		}
	}

	public float MaxTimeLimit
	{
		get
		{
			float num = _points[0].MaxTime;
			for (int i = 1; i < _points.Count; i++)
			{
				if (_points[i].MaxTime > num)
				{
					num = _points[i].MinTime;
				}
			}
			return num;
		}
	}

	public TrajectoryDebugPointsGroup(string label)
	{
		_label = label;
		Color = NextColor();
	}

	public string[] GetElementsList()
	{
		List<string> list = new List<string>();
		foreach (TrajectoryDebugPoints point in _points)
		{
			list.Add(point.Label + _ED3E._E000(27312) + point.Count + _ED3E._E000(36519));
		}
		return list.ToArray();
	}

	protected Color NextColor()
	{
		Color result = _colors[_colorIndex % _colors.Count];
		_colorIndex++;
		return result;
	}

	public void Reset()
	{
		_colorIndex = 0;
	}

	public virtual void Add(string label, Vector3 point, float time)
	{
		TrajectoryDebugPoints trajectoryDebugPoints = _points.FirstOrDefault((TrajectoryDebugPoints p) => p.Label == label);
		if (trajectoryDebugPoints == null)
		{
			trajectoryDebugPoints = new TrajectoryDebugPoints(label);
			_points.Add(trajectoryDebugPoints);
		}
		trajectoryDebugPoints.AddPoint(point, time);
	}

	public virtual void Draw()
	{
		if (_points != null && _points.Count > ActivePointsIndex && ActivePointsIndex >= 0)
		{
			_points[ActivePointsIndex].Draw(IsDraw, Color, Offset, Radius, MinTime, MaxTime);
		}
	}
}
