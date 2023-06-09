using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.HandBook;

public sealed class EntityIcon : UIElement
{
	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _colorPanel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private ItemViewStats _itemStats;

	private _E3E2 _E084;

	private Item _E085;

	private SimpleTooltip _E02A;

	private void Awake()
	{
		_iconImage.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		HoverTrigger orAddComponent = this.GetOrAddComponent<HoverTrigger>();
		_E02A = ItemUiContext.Instance.Tooltip;
		orAddComponent.OnHoverStart += delegate
		{
			if (_E085 != null)
			{
				_E02A.Show(_E085.Name.Localized());
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (_E02A.gameObject.activeSelf)
			{
				_E02A.Close();
			}
		};
	}

	public void Show(Item item)
	{
		_E085 = item;
		base.gameObject.SetActive(_E085 != null);
		if (_E085 != null)
		{
			Color color = _E085.BackgroundColor.ToColor();
			color.a = 0.3019608f;
			_colorPanel.color = color;
			_E084 = ItemViewFactory.LoadItemIcon(_E085);
			UI.AddDisposable(_E084.Changed.Bind(_E000));
			_itemStats.SetStaticInfo(_E085, examined: true);
		}
	}

	private void _E000()
	{
		if (!(_loader == null))
		{
			_loader.SetActive(_E084.Sprite == null);
			_iconImage.gameObject.SetActive(_E084.Sprite != null);
			_iconImage.sprite = _E084.Sprite;
		}
	}

	public override void Close()
	{
		if (_E02A != null)
		{
			_E02A.Close();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E001(PointerEventData eventData)
	{
		if (_E085 != null)
		{
			_E02A.Show(_E085.Name.Localized());
		}
	}

	[CompilerGenerated]
	private void _E002(PointerEventData eventData)
	{
		if (_E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
	}
}
