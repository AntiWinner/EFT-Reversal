using System.Collections.Generic;
using EFT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow;

public sealed class StretchWindow : UIElement
{
	[SerializeField]
	private Vector2 _maxSize;

	[SerializeField]
	private LayoutElement _stretchableObject;

	[SerializeField]
	private RectTransform _rootTransform;

	[SerializeField]
	private Dictionary<StretchArea.EStretchAreaType, StretchArea> _stretchAreas;

	private ECursorType? _E009;

	private ECursorType? _E00A;

	private void OnValidate()
	{
		if (base.transform.GetSiblingIndex() != 0)
		{
			base.transform.SetSiblingIndex(0);
		}
	}

	private void Start()
	{
		foreach (var (areaType, stretchArea2) in _stretchAreas)
		{
			stretchArea2.Init(areaType, _rootTransform, _stretchableObject, _maxSize);
			UI.AddDisposable(stretchArea2.OnCursorChange.Subscribe(_E001));
			UI.AddDisposable(stretchArea2.OnForcedCursorChange.Subscribe(_E000));
		}
	}

	private void OnDestroy()
	{
		UI.Dispose();
	}

	private void _E000(ECursorType? cursor)
	{
		_E009 = cursor;
		_E002();
	}

	private void _E001(ECursorType? cursor)
	{
		_E00A = cursor;
		_E002();
	}

	private void _E002()
	{
		_EC45.SetCursor(_E009 ?? _E00A ?? ECursorType.Idle);
	}
}
