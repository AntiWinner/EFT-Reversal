using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class DynamicScroll : MonoBehaviour
{
	[SerializeField]
	private int _visibleCount = 5;

	[SerializeField]
	private VerticalLayoutGroup _layoutGroup;

	[SerializeField]
	private ScrollRect _scrollRect;

	[SerializeField]
	private List<DynamicScrollElement> _activeElements;

	[SerializeField]
	private List<DynamicScrollElement> _disabledElements;

	private readonly SortedList<int, DynamicScrollElement> m__E000 = new SortedList<int, DynamicScrollElement>();

	private int m__E001;

	private float? m__E002;

	private float? _E003 => this.m__E002 ?? (this.m__E002 = (base.transform as RectTransform).rect.size.y);

	private void Start()
	{
		for (int i = 0; i < _activeElements.Count; i++)
		{
			_activeElements[i].ScrollIndex = i;
			_activeElements[i].Perform(i);
			this.m__E000.Add(i, _activeElements[i]);
		}
		foreach (KeyValuePair<int, DynamicScrollElement> item in this.m__E000)
		{
			Disable(item.Value, EArrangement.Bottom);
		}
		for (int j = 0; j < _visibleCount; j++)
		{
			Enable(this.m__E000[j], EArrangement.Bottom);
		}
		_scrollRect.onValueChanged.AddListener(delegate
		{
			_E000();
		});
	}

	private void Update()
	{
	}

	private void _E000()
	{
		DynamicScrollElement dynamicScrollElement = _E001();
		if (dynamicScrollElement == null)
		{
			return;
		}
		int scrollIndex = dynamicScrollElement.ScrollIndex;
		if (scrollIndex != this.m__E001)
		{
			int num = Mathf.Clamp(this.m__E001 + _visibleCount, 0, this.m__E000.Count);
			int num2 = Mathf.Clamp(scrollIndex + _visibleCount, 0, this.m__E000.Count);
			for (int i = ((scrollIndex < this.m__E001) ? num2 : this.m__E001); i < ((scrollIndex < num && scrollIndex > this.m__E001) ? scrollIndex : num); i++)
			{
				Disable(this.m__E000[i], (scrollIndex <= this.m__E001) ? EArrangement.Bottom : EArrangement.Top);
			}
			for (int j = scrollIndex; j < num2; j++)
			{
				Enable(this.m__E000[j], (scrollIndex > this.m__E001) ? EArrangement.Bottom : EArrangement.Top);
			}
			this.m__E001 = scrollIndex;
		}
	}

	[CanBeNull]
	private DynamicScrollElement _E001()
	{
		foreach (KeyValuePair<int, DynamicScrollElement> item in this.m__E000)
		{
			KeyValuePair<bool, bool> keyValuePair = _E002(item.Value);
			if (keyValuePair.Key || keyValuePair.Value)
			{
				continue;
			}
			return item.Value;
		}
		Debug.LogError(_ED3E._E000(260242));
		return null;
	}

	private KeyValuePair<bool, bool> _E002(DynamicScrollElement element)
	{
		RectTransform obj = _layoutGroup.transform as RectTransform;
		RectTransform rectTransform = element.Transform;
		float num = obj.anchoredPosition.y - rectTransform.anchoredPosition.y;
		float f = rectTransform.anchoredPosition.y * 2f;
		bool key = !(num < Mathf.Abs(f) + element.Height);
		bool value = !(num + (base.transform as RectTransform).rect.size.y > Mathf.Abs(f));
		return new KeyValuePair<bool, bool>(key, value);
	}

	protected virtual void Disable(DynamicScrollElement scrollElement, EArrangement arrangement)
	{
		if (_activeElements.Contains(scrollElement))
		{
			if (arrangement == EArrangement.Top)
			{
				_layoutGroup.padding.top += (int)scrollElement.Height;
			}
			else
			{
				_layoutGroup.padding.bottom += (int)scrollElement.Height;
			}
			scrollElement.gameObject.SetActive(value: false);
			_activeElements.Remove(scrollElement);
			_disabledElements.Add(scrollElement);
		}
	}

	protected virtual void Enable(DynamicScrollElement scrollElement, EArrangement arrangement)
	{
		if (_disabledElements.Contains(scrollElement))
		{
			if (arrangement == EArrangement.Top)
			{
				_layoutGroup.padding.top -= (int)scrollElement.Height;
			}
			else
			{
				_layoutGroup.padding.bottom -= (int)scrollElement.Height;
			}
			scrollElement.gameObject.SetActive(value: true);
			_activeElements.Add(scrollElement);
			_disabledElements.Remove(scrollElement);
		}
	}

	[CompilerGenerated]
	private void _E003(Vector2 arg)
	{
		_E000();
	}
}
