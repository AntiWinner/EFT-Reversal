using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class ScrollRectNoDrag : ScrollRect, ILayoutElement
{
	[SerializeField]
	public bool ControlSize;

	[SerializeField]
	public float MaxWidth;

	[SerializeField]
	public float MaxHeight;

	[SerializeField]
	public bool AutoZeroing;

	[SerializeField]
	public TextAnchor Alignment;

	private ScrollbarVisibility _E000;

	private float _E001;

	private float _E002;

	private RectTransform _E003;

	public override float preferredWidth => _E001;

	public override float preferredHeight => _E002;

	public override int layoutPriority => 1;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		if (!ControlSize || base.content == null)
		{
			_E001 = base.preferredWidth;
			return;
		}
		float preferredSize = LayoutUtility.GetPreferredSize(base.content, 0);
		preferredSize = ((preferredSize >= 0f) ? preferredSize : float.PositiveInfinity);
		_E001 = ((MaxWidth > 0f) ? Math.Min(preferredSize, MaxWidth) : preferredSize);
	}

	public override void CalculateLayoutInputVertical()
	{
		base.CalculateLayoutInputVertical();
		if (!ControlSize || base.content == null)
		{
			_E002 = base.preferredHeight;
			return;
		}
		float preferredSize = LayoutUtility.GetPreferredSize(base.content, 1);
		preferredSize = ((preferredSize >= 0f) ? preferredSize : float.PositiveInfinity);
		_E002 = ((MaxHeight > 0f) ? Math.Min(preferredSize, MaxHeight) : preferredSize);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (AutoZeroing)
		{
			Vector2 zero = Vector2.zero;
			switch (Alignment)
			{
			case TextAnchor.LowerCenter:
				zero.x = 0.5f;
				break;
			case TextAnchor.LowerRight:
				zero.x = 1f;
				break;
			case TextAnchor.MiddleLeft:
				zero.y = 0.5f;
				break;
			case TextAnchor.MiddleCenter:
				zero.x = 0.5f;
				zero.y = 0.5f;
				break;
			case TextAnchor.MiddleRight:
				zero.x = 1f;
				zero.y = 0.5f;
				break;
			case TextAnchor.UpperLeft:
				zero.y = 1f;
				break;
			case TextAnchor.UpperCenter:
				zero.x = 0.5f;
				zero.y = 1f;
				break;
			case TextAnchor.UpperRight:
				zero.x = 1f;
				zero.y = 1f;
				break;
			}
			base.normalizedPosition = zero;
		}
	}

	public override void OnInitializePotentialDrag([NotNull] PointerEventData eventData)
	{
	}

	public override void OnBeginDrag([NotNull] PointerEventData eventData)
	{
	}

	public override void OnEndDrag([NotNull] PointerEventData eventData)
	{
	}

	public override void OnDrag([NotNull] PointerEventData eventData)
	{
	}

	public override void OnScroll([NotNull] PointerEventData data)
	{
		Vector2 scrollDelta = data.scrollDelta;
		if (Math.Abs(data.scrollDelta.x) > Mathf.Epsilon && (base.horizontalScrollbar == null || !base.horizontalScrollbar.gameObject.activeInHierarchy))
		{
			scrollDelta.x = 0f;
		}
		if (Math.Abs(data.scrollDelta.y) > Mathf.Epsilon && (base.verticalScrollbar == null || !base.verticalScrollbar.gameObject.activeInHierarchy))
		{
			scrollDelta.y = 0f;
		}
		if (!scrollDelta.x.ApproxEquals(0f) || !scrollDelta.y.ApproxEquals(0f))
		{
			data.scrollDelta = scrollDelta;
			base.OnScroll(data);
		}
	}

	protected override void Awake()
	{
		_E003 = (RectTransform)base.transform;
		_E000 = base.horizontalScrollbarVisibility;
		base.Awake();
	}

	private void Update()
	{
		if (base.horizontal && base.vertical && !(base.horizontalScrollbar == null) && !(base.verticalScrollbar == null) && _E000 != 0)
		{
			Rect rect = _E003.rect;
			Rect rect2 = base.content.rect;
			if ((rect.width - rect2.width).Negative() && (rect.height - rect2.height).Negative())
			{
				base.horizontalScrollbarVisibility = ScrollbarVisibility.Permanent;
			}
			else
			{
				base.horizontalScrollbarVisibility = _E000;
			}
		}
	}

	protected override void SetContentAnchoredPosition(Vector2 position)
	{
		if (!base.horizontal)
		{
			position.x = base.content.anchoredPosition.x;
		}
		if (!base.vertical)
		{
			position.y = base.content.anchoredPosition.y;
		}
		if (!position.x.LowAccuracyApprox(base.content.anchoredPosition.x) || !position.y.LowAccuracyApprox(base.content.anchoredPosition.y))
		{
			base.content.anchoredPosition = position;
			UpdateBounds();
		}
	}
}
