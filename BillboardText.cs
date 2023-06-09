using System;
using System.Runtime.CompilerServices;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;

public class BillboardText : MonoBehaviour
{
	private RectTransform _E000;

	[CompilerGenerated]
	private Text _E001;

	internal TextDirection _E002;

	public RectTransform RectTransformOverride;

	[CompilerGenerated]
	private object _E003;

	[NonSerialized]
	public float Scale = 1f;

	public bool parentSet;

	public RectTransform parent;

	public bool Recycled;

	private CanvasRenderer[] _E004;

	public Text UIText
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public object UserData
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public RectTransform Rect
	{
		get
		{
			if (UIText == null)
			{
				return null;
			}
			if (RectTransformOverride != null)
			{
				return RectTransformOverride;
			}
			if (_E000 == null)
			{
				_E000 = UIText.GetComponent<RectTransform>();
			}
			return _E000;
		}
	}

	public void SetVisible(bool visible)
	{
		bool cull = !visible;
		RectTransform rect = Rect;
		if (!(rect == null))
		{
			if (_E004 == null)
			{
				_E004 = rect.GetComponentsInChildren<CanvasRenderer>();
			}
			for (int i = 0; i < _E004.Length; i++)
			{
				_E004[i].cull = cull;
			}
		}
	}
}
