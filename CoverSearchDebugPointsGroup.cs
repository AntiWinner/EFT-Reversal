using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoverSearchDebugPointsGroup : BotZoneDebugPointsGroup
{
	[SerializeField]
	private List<float> _searchRadiuses = new List<float>();

	public float SpecialRadius = 1f;

	public bool DrawAll;

	public float SearchRadius
	{
		get
		{
			if (_searchRadiuses.Count <= ActivePointsIndex)
			{
				return 0f;
			}
			return _searchRadiuses[ActivePointsIndex];
		}
	}

	public CoverSearchDebugPointsGroup(string label)
		: base(label)
	{
		_label = label;
		Color = NextColor();
	}

	public void SetRadius(string label, float radius)
	{
		for (int i = 0; i < base.Points.Count; i++)
		{
			if (base.Points[i].Label == label)
			{
				while (_searchRadiuses.Count <= i)
				{
					_searchRadiuses.Add(0f);
				}
				_searchRadiuses[i] = radius;
			}
		}
	}

	public override void Draw()
	{
		if (_points == null || _points.Count <= ActivePointsIndex || ActivePointsIndex < 0)
		{
			return;
		}
		if (DrawAll)
		{
			for (int i = 0; i <= ActivePointsIndex; i++)
			{
				_points[i].Draw(IsDraw, Color, Offset, Radius);
			}
		}
		else
		{
			_points[ActivePointsIndex].Draw(IsDraw, Color, Offset, Radius);
		}
	}
}
