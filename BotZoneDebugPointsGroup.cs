using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BotZoneDebugPointsGroup
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string label;

		internal bool _E000(BotZoneDebugPoints p)
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
	protected List<BotZoneDebugPoints> _points = new List<BotZoneDebugPoints>();

	public int ActivePointsIndex;

	public string Label => _label;

	public List<BotZoneDebugPoints> Points => _points;

	public BotZoneDebugPoints List
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

	public BotZoneDebugPointsGroup(string label)
	{
		_label = label;
		Color = NextColor();
	}

	public string[] GetElementsList()
	{
		List<string> list = new List<string>();
		foreach (BotZoneDebugPoints point in _points)
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

	public void Add(string label, Vector3 point)
	{
		BotZoneDebugPoints botZoneDebugPoints = _points.FirstOrDefault((BotZoneDebugPoints p) => p.Label == label);
		if (botZoneDebugPoints == null)
		{
			botZoneDebugPoints = new BotZoneDebugPoints(label);
			_points.Add(botZoneDebugPoints);
		}
		botZoneDebugPoints.AddPoint(point);
	}

	public virtual void Draw()
	{
		if (_points != null && _points.Count > ActivePointsIndex && ActivePointsIndex >= 0)
		{
			_points[ActivePointsIndex].Draw(IsDraw, Color, Offset, Radius);
		}
	}
}
