using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Map;

public sealed class ExtractionPointView : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private Image _background;

	[SerializeField]
	private Image _whiteIcon;

	[SerializeField]
	private GameObject _icon;

	[SerializeField]
	private GameObject _notGuaranteedIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _nameLeft;

	[SerializeField]
	private CustomTextMeshProUGUI _nameRight;

	[SerializeField]
	private Image _textLeft;

	[SerializeField]
	private Image _textRight;

	[CompilerGenerated]
	private ExtractionPoint _E31E;

	public ExtractionPoint ExtractionPoint
	{
		[CompilerGenerated]
		get
		{
			return _E31E;
		}
		[CompilerGenerated]
		private set
		{
			_E31E = value;
		}
	}

	public void Show(ExtractionPoint point, Action<ExtractionPoint> onDoubleClick)
	{
		ExtractionPoint = point;
		_icon.gameObject.SetActive(!point.NotGuaranteed);
		_notGuaranteedIcon.gameObject.SetActive(point.NotGuaranteed);
		_background.gameObject.SetActive(point.ShowRadius);
		string text = point.Name.Localized();
		_nameLeft.text = text;
		_nameRight.text = text;
		_textLeft.gameObject.SetActive(point.NotGuaranteed);
		_textRight.gameObject.SetActive(!point.NotGuaranteed);
		ShowGameObject();
		((RectTransform)base.transform).anchoredPosition = point.PositionOnMap;
	}

	private void Update()
	{
		Transform transform = base.transform;
		transform.localScale = new Vector3(1f / transform.parent.localScale.x, 1f / transform.parent.localScale.y, 1f);
		_background.transform.localScale = new Vector3(10f * ExtractionPoint.Radius / transform.localScale.x, 10f * ExtractionPoint.Radius / transform.localScale.y, 1f);
	}

	public void SetMainColor(Color32 color)
	{
		if (!ExtractionPoint.NotGuaranteed)
		{
			_icon.gameObject.SetActive(value: false);
			_notGuaranteedIcon.gameObject.SetActive(value: false);
			_whiteIcon.gameObject.SetActive(value: true);
			_whiteIcon.color = color;
		}
		_textLeft.color = new Color32(color.r, color.g, color.b, 180);
		_textRight.color = new Color32(color.r, color.g, color.b, 180);
		_background.color = color;
	}

	public void OnPointerClick([CanBeNull] PointerEventData eventData)
	{
	}
}
