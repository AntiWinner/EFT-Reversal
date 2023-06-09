using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class StretchArea : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	[Flags]
	public enum EStretchAreaType
	{
		Top = 1,
		Bottom = 2,
		Left = 4,
		Right = 8,
		TopLeft = 5,
		TopRight = 9,
		BottomLeft = 6,
		BottomRight = 0xA
	}

	[NonSerialized]
	public readonly _ECED<ECursorType?> OnCursorChange = new _ECED<ECursorType?>();

	[NonSerialized]
	public readonly _ECED<ECursorType?> OnForcedCursorChange = new _ECED<ECursorType?>();

	private EStretchAreaType _E000;

	private LayoutElement _E001;

	private RectTransform _E002;

	private RectTransform _E003;

	private Vector2 _E004;

	private ECursorType _E005;

	private ECursorType _E006;

	private bool _E007;

	private bool _E008;

	private Vector2 _E009;

	private Vector2 _E00A;

	private Vector2 _E00B;

	public void Init(EStretchAreaType areaType, RectTransform rootTransform, LayoutElement stretchableObject, Vector2 maxSize)
	{
		_E000 = areaType;
		_E004 = maxSize;
		_E002 = rootTransform;
		_E001 = stretchableObject;
		_E003 = (RectTransform)_E001.transform;
		bool flag = _E000.HasFlag(EStretchAreaType.Top);
		bool flag2 = _E000.HasFlag(EStretchAreaType.Bottom);
		bool flag3 = _E000.HasFlag(EStretchAreaType.Left);
		bool flag4 = _E000.HasFlag(EStretchAreaType.Right);
		_E007 = flag4 || flag3;
		_E008 = flag || flag2;
		_E009 = new Vector2(flag4 ? 1f : (-1f), flag ? 1f : (-1f));
		_E00A = _E002.pivot;
		_E00A.x = (flag4 ? _E00A.x : (1f - _E00A.x));
		_E00A.y = (flag ? _E00A.y : (1f - _E00A.y));
		_E00A = new Vector2(_E007 ? _E00A.x : 0f, _E008 ? _E00A.y : 0f);
		switch (_E000)
		{
		case EStretchAreaType.Top:
		case EStretchAreaType.Bottom:
			_E005 = ECursorType.StretchVertical;
			break;
		case EStretchAreaType.Left:
		case EStretchAreaType.Right:
			_E005 = ECursorType.StretchHorizontal;
			break;
		case EStretchAreaType.TopLeft:
		case EStretchAreaType.BottomRight:
			_E005 = ECursorType.StretchCorner;
			break;
		case EStretchAreaType.BottomLeft:
		case EStretchAreaType.TopRight:
			_E005 = ECursorType.StretchCorner2;
			break;
		case EStretchAreaType.Top | EStretchAreaType.Bottom:
		case EStretchAreaType.TopLeft | EStretchAreaType.Bottom:
			break;
		}
	}

	public void OnDrag([NotNull] PointerEventData eventData)
	{
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Clamp(eventData.position.x, 0f, Screen.width);
		zero.y = Mathf.Clamp(eventData.position.y, 0f, Screen.height);
		Vector3 lossyScale = _E003.lossyScale;
		Vector2 vector = zero - _E00B;
		if (!vector.magnitude.IsZero())
		{
			Vector2 divisible = new Vector2(_E007 ? vector.x : 0f, _E008 ? vector.y : 0f);
			Vector2 vector2 = new Vector2(_E003.rect.width, _E003.rect.height);
			Vector2 vector3 = divisible.Divide((Vector2)lossyScale) * _E009;
			float preferredWidth = Mathf.Clamp(_E001.preferredWidth + vector3.x, 0f, _E004.x);
			float preferredHeight = Mathf.Clamp(_E001.preferredHeight + vector3.y, 0f, _E004.y);
			_E001.preferredWidth = preferredWidth;
			_E001.preferredHeight = preferredHeight;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_E003);
			Vector2 vector4 = new Vector2(_E003.rect.width, _E003.rect.height);
			divisible = (vector4 - vector2) * _E009;
			if (!divisible.x.IsZero())
			{
				_E00B.x += divisible.x * lossyScale.x;
			}
			if (!divisible.y.IsZero())
			{
				_E00B.y += divisible.y * lossyScale.y;
			}
			_E001.preferredWidth = vector4.x;
			_E001.preferredHeight = vector4.y;
			_E002.anchoredPosition += divisible * _E00A;
			_E002.CorrectPositionResolution();
		}
	}

	public void OnPointerDown([NotNull] PointerEventData eventData)
	{
		_E00B = eventData.position;
		OnForcedCursorChange.Invoke(_E005);
	}

	public void OnPointerUp([NotNull] PointerEventData eventData)
	{
		OnForcedCursorChange.Invoke(null);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		OnCursorChange.Invoke(_E005);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		OnCursorChange.Invoke(null);
	}
}
