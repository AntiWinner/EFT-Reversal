using System;
using System.Collections.Generic;
using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class TraderHoverPanel : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private Image _buttonImage;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _selectedColor;

	[SerializeField]
	private Color _defaultTextColor;

	[SerializeField]
	private CustomTextMeshProUGUI[] _labels;

	private readonly Dictionary<CustomTextMeshProUGUI, Color> _E202 = new Dictionary<CustomTextMeshProUGUI, Color>();

	private Action _E06D;

	private void Awake()
	{
		CustomTextMeshProUGUI[] labels = _labels;
		foreach (CustomTextMeshProUGUI customTextMeshProUGUI in labels)
		{
			_E202.Add(customTextMeshProUGUI, customTextMeshProUGUI.color);
		}
	}

	public void Show(Action onClick)
	{
		_E06D = onClick;
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_buttonImage.color = _selectedColor;
		foreach (KeyValuePair<CustomTextMeshProUGUI, Color> item in _E202)
		{
			item.Key.color = _defaultTextColor;
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_buttonImage.color = _defaultColor;
		foreach (KeyValuePair<CustomTextMeshProUGUI, Color> item in _E202)
		{
			item.Key.color = item.Value;
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuTraderPress);
		_E06D?.Invoke();
	}

	public override void Close()
	{
		_E202.Clear();
		base.Close();
	}
}
