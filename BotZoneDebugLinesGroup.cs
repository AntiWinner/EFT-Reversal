using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BotZoneDebugLinesGroup
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string label;

		internal bool _E000(BotZoneDebugLines p)
		{
			return p.Label == label;
		}
	}

	[SerializeField]
	private string _label;

	public bool IsDraw;

	public Color Color = Color.magenta;

	public Vector3 Offset = Vector3.zero;

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
	private List<BotZoneDebugLines> _lines = new List<BotZoneDebugLines>();

	public int ActiveLinesIndex;

	public string Label => _label;

	public List<BotZoneDebugLines> Lines => _lines;

	public BotZoneDebugLines List
	{
		get
		{
			if (ActiveLinesIndex >= Lines.Count)
			{
				return null;
			}
			return Lines[ActiveLinesIndex];
		}
	}

	public BotZoneDebugLinesGroup(string label)
	{
		_label = label;
		Color = _E000();
	}

	public string[] GetElementsList()
	{
		List<string> list = new List<string>();
		foreach (BotZoneDebugLines line in _lines)
		{
			list.Add(line.Label + _ED3E._E000(54246) + line.Count + _ED3E._E000(36519));
		}
		return list.ToArray();
	}

	private Color _E000()
	{
		Color result = _colors[_colorIndex % _colors.Count];
		_colorIndex++;
		return result;
	}

	public void Reset()
	{
		_colorIndex = 0;
	}

	public void AddLine(string label, Vector3 pointA, Vector3 pointB)
	{
		BotZoneDebugLines botZoneDebugLines = _lines.FirstOrDefault((BotZoneDebugLines p) => p.Label == label);
		if (botZoneDebugLines == null)
		{
			botZoneDebugLines = new BotZoneDebugLines(label);
			_lines.Add(botZoneDebugLines);
		}
		botZoneDebugLines.AddLine(pointA, pointB);
	}

	public void Draw()
	{
		if (_lines != null && _lines.Count > ActiveLinesIndex && ActiveLinesIndex >= 0)
		{
			_lines[ActiveLinesIndex].Draw(IsDraw, Color, Offset);
		}
	}
}
