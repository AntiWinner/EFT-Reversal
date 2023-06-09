using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

internal class ChartItemTextBlend : ChartItemLerpEffect
{
	private Text _E00C;

	private Shadow[] _E00D;

	private Dictionary<Object, float> _E00E = new Dictionary<Object, float>();

	private CanvasRenderer _E00F;

	internal override Quaternion _E008 => Quaternion.identity;

	internal override Vector3 _E007 => new Vector3(1f, 1f, 1f);

	internal override Vector3 _E009 => Vector3.zero;

	protected override void Start()
	{
		base.Start();
		_E00C = GetComponent<Text>();
		_E00D = GetComponents<Shadow>();
		Shadow[] array = _E00D;
		foreach (Shadow shadow in array)
		{
			_E00E.Add(shadow, shadow.effectColor.a);
		}
		ApplyLerp(0f);
	}

	protected override float GetStartValue()
	{
		if (_E00C != null)
		{
			return _E00C.color.a;
		}
		return 0f;
	}

	private new CanvasRenderer _E000()
	{
		if (_E00F == null)
		{
			_E00F = GetComponent<CanvasRenderer>();
		}
		return _E00F;
	}

	protected override void ApplyLerp(float value)
	{
		for (int i = 0; i < _E00D.Length; i++)
		{
			Shadow shadow = _E00D[i];
			if (_E00E.TryGetValue(shadow, out var value2))
			{
				Color effectColor = shadow.effectColor;
				effectColor.a = Mathf.Lerp(0f, value2, value);
				shadow.effectColor = effectColor;
			}
		}
		if (!(_E00C != null))
		{
			return;
		}
		Color color = _E00C.color;
		color.a = Mathf.Clamp(value, 0f, 1f);
		_E00C.color = color;
		CanvasRenderer canvasRenderer = _E000();
		if (!(canvasRenderer != null))
		{
			return;
		}
		if (value <= 0f)
		{
			if (!canvasRenderer.cull)
			{
				canvasRenderer.cull = true;
			}
		}
		else if (canvasRenderer.cull)
		{
			canvasRenderer.cull = false;
		}
	}
}
