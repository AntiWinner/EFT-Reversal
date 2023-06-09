using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class ItemIconView : UIElement
{
	[SerializeField]
	private Image _mainImage;

	[SerializeField]
	private GameObject _iconLoader;

	[SerializeField]
	private Vector3 _iconRotation = new Vector3(0f, 0f, 45f);

	[SerializeField]
	private RectTransform _mainRect;

	private Action<bool, PointerEventData> _E0F1;

	protected _E3E2 ItemIcon;

	protected Image MainImage => _mainImage;

	protected GameObject IconLoader => _iconLoader;

	private void Awake()
	{
		HoverTrigger orAddComponent = base.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate(PointerEventData eventData)
		{
			_E0F1?.Invoke(arg1: true, eventData);
		};
		orAddComponent.OnHoverEnd += OnHoverEnd;
	}

	public void Show([CanBeNull] Item item, Action<bool, PointerEventData> onHover, Sprite emptyIconSprite = null)
	{
		ShowGameObject();
		_E0F1 = onHover;
		if (item == null)
		{
			if (emptyIconSprite == null)
			{
				Debug.LogError(_ED3E._E000(258211));
			}
			else
			{
				_E001(emptyIconSprite);
			}
		}
		else
		{
			ItemIcon = ItemViewFactory.LoadItemIcon(item);
			UI.AddDisposable(ItemIcon.Changed.Bind(_E000));
		}
	}

	protected virtual void OnHoverEnd(PointerEventData eventData)
	{
		_E0F1?.Invoke(arg1: false, eventData);
	}

	private void _E000()
	{
		if (_iconLoader != null)
		{
			_iconLoader.SetActive(ItemIcon.Sprite == null);
		}
		if (!(_mainImage == null))
		{
			_mainImage.gameObject.SetActive(ItemIcon.Sprite != null);
			_mainImage.sprite = ItemIcon.Sprite;
			_mainImage.SetNativeSize();
			UpdateScale();
		}
	}

	private void _E001(Sprite emptySprite)
	{
		if (_iconLoader != null)
		{
			_iconLoader.SetActive(ItemIcon.Sprite == null);
		}
		if (!(_mainImage == null))
		{
			_mainImage.gameObject.SetActive(emptySprite != null);
			_mainImage.sprite = emptySprite;
			_mainImage.SetNativeSize();
			UpdateScale();
		}
	}

	protected virtual void UpdateScale()
	{
		if (_mainImage.gameObject.activeSelf)
		{
			Vector2 sizeDelta = ((_mainRect == null) ? ((RectTransform)_mainImage.transform) : _mainRect).sizeDelta;
			Quaternion quaternion = Quaternion.Euler(_iconRotation);
			Vector3 vector = quaternion * ((RectTransform)_mainImage.transform).rect.size;
			float val = sizeDelta.x / Mathf.Abs(vector.x);
			float val2 = sizeDelta.y / Mathf.Abs(vector.y);
			Vector3 localScale = Vector3.one * Math.Min(val, val2);
			Transform obj = _mainImage.transform;
			obj.localRotation = quaternion;
			obj.localScale = localScale;
		}
	}

	[CompilerGenerated]
	private void _E002(PointerEventData eventData)
	{
		_E0F1?.Invoke(arg1: true, eventData);
	}
}
