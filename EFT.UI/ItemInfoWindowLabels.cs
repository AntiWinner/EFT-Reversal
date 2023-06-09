using System.Runtime.CompilerServices;
using EFT.UI.WeaponModding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class ItemInfoWindowLabels : UIElement
{
	private const float _E12D = 0.12f;

	[SerializeField]
	private GameObject _previewPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _itemType;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private CustomTextMeshProUGUI _weightText;

	private DragTrigger _E12E;

	private ScrollTrigger _E12F;

	private WeaponPreview _E130;

	private DragTrigger _E000
	{
		get
		{
			if (_E12E == null)
			{
				_E12E = _previewPanel.AddComponent<DragTrigger>();
			}
			return _E12E;
		}
	}

	private ScrollTrigger _E001
	{
		get
		{
			if (_E12F == null)
			{
				_E12F = _previewPanel.AddComponent<ScrollTrigger>();
			}
			return _E12F;
		}
	}

	internal void Show(WeaponPreview weaponPreview, bool canDrag = true)
	{
		_E130 = weaponPreview;
		if (canDrag)
		{
			this._E000.onDrag += _E004;
			UI.AddDisposable(delegate
			{
				_E12E.onDrag -= _E004;
			});
		}
		this._E001.OnOnScroll += _E005;
		UI.AddDisposable(delegate
		{
			_E12F.OnOnScroll -= _E005;
		});
		ShowGameObject();
	}

	internal void _E000(string value)
	{
		_caption.text = value;
	}

	internal void _E001(string value, bool addToText = false)
	{
		_itemType.text = (addToText ? (_itemType.text + value) : value);
	}

	internal void _E002(string value)
	{
		_description.text = value;
	}

	internal void _E003(string value)
	{
		_weightText.text = value;
	}

	private void _E004(PointerEventData pointerData)
	{
		_E130.Rotate(pointerData.delta.x, pointerData.delta.y, 0f, 0f);
	}

	private void _E005(PointerEventData pointerData)
	{
		_E130.Zoom(pointerData.scrollDelta.y * 0.12f);
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E12E.onDrag -= _E004;
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E12F.OnOnScroll -= _E005;
	}
}
