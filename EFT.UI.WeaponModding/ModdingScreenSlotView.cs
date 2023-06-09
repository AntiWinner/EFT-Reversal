using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.WeaponModding;

public sealed class ModdingScreenSlotView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemView itemView;

		internal void _E000()
		{
			itemView.Kill();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E54D itemTemplates;

		public IEnumerable<Type> excludeTypes;

		internal Type _E000(string node)
		{
			if (!_EA59.TypeTable.ContainsKey(node))
			{
				if (!itemTemplates.ContainsKey(node))
				{
					return null;
				}
				return _EA59.TypeTable[itemTemplates[node]._parent];
			}
			return _EA59.TypeTable[node];
		}

		internal Type _E001(string node)
		{
			if (!_EA59.TypeTable.ContainsKey(node))
			{
				if (!itemTemplates.ContainsKey(node))
				{
					return null;
				}
				return _EA59.TypeTable[itemTemplates[node]._parent];
			}
			return _EA59.TypeTable[node];
		}

		internal bool _E002(Type t)
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E002
			{
				t = t
			};
			return excludeTypes.All((Type excludeType) => excludeType != CS_0024_003C_003E8__locals0.t);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Type t;

		internal bool _E000(Type excludeType)
		{
			return excludeType != t;
		}
	}

	[SerializeField]
	private Transform _filtersContainer;

	[SerializeField]
	private Image _boneIcon;

	[SerializeField]
	private RectTransform _slotCircleIcon;

	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private GameObject _boneNamePanel;

	[SerializeField]
	private TextMeshProUGUI _boneName;

	[SerializeField]
	private Image _boneNameBackground;

	[SerializeField]
	private Button _dropDownButton;

	[SerializeField]
	private RectTransform _tooltipHoverArea;

	[SerializeField]
	private RectTransform _moddingItemContainer;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private EmptyItemView _emptyItemViewTemplate;

	public RectTransform MenuAnchor;

	public TextMeshProUGUI DebugText;

	private _EA40 _E085;

	private DropDownMenu _E242;

	private _EAE6 _E19A;

	private Transform _E243;

	private _EC5D _E244;

	private SimpleTooltip _E02A;

	private bool _E0F2;

	private Sprite _E245;

	private Sprite _E246;

	private ItemUiContext _E089;

	private _EB65 _E133;

	private Slot _E247;

	[CompilerGenerated]
	private Color _E248;

	[CompilerGenerated]
	private EModClass _E249;

	[CompilerGenerated]
	private string _E24A;

	public Color Color
	{
		[CompilerGenerated]
		get
		{
			return _E248;
		}
		[CompilerGenerated]
		private set
		{
			_E248 = value;
		}
	}

	private bool _E000 => _E085.MissingVitalParts.Contains(_E247);

	private EModClass _E001
	{
		[CompilerGenerated]
		get
		{
			return _E249;
		}
		[CompilerGenerated]
		set
		{
			_E249 = value;
		}
	}

	private string _E002
	{
		[CompilerGenerated]
		get
		{
			return _E24A;
		}
		[CompilerGenerated]
		set
		{
			_E24A = value;
		}
	}

	private void Start()
	{
		_E246 = _E905.Pop<Sprite>(_ED3E._E000(257547));
		_E245 = _E905.Pop<Sprite>(_ED3E._E000(257581));
		HoverTrigger orAddComponent = _tooltipHoverArea.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += _E002;
		orAddComponent.OnHoverEnd += _E001;
		_dropDownButton.onClick.AddListener(_E003);
	}

	public void Show(_EB65 itemContext, _EC5D moddingScreen, Slot slot, _EA40 item, DropDownMenu menu, Transform modBone, _EAE6 itemController, ItemUiContext itemUiContext)
	{
		ShowGameObject();
		_E247 = slot;
		_E133 = itemContext.CreateModdingChild(slot.ParentItem, new _EB20(slot));
		UI.AddDisposable(_E133);
		_E242 = menu;
		_E19A = itemController;
		_E243 = modBone;
		_E244 = moddingScreen;
		_E085 = item;
		_E089 = itemUiContext;
		_E02A = _E089.Tooltip;
		_E242.OnMenuOpen += _E005;
		_E242.OnMenuClosed += _E004;
		_lineRenderer.useWorldSpace = true;
		EWeaponModType modType = EWeaponModType.mod_tactical;
		try
		{
			modType = (EWeaponModType)Enum.Parse(typeof(EWeaponModType), slot.ID);
		}
		catch
		{
		}
		this._E001 = (_E085.VitalParts.Contains(_E247) ? EModClass.Master : ItemViewFactory.GetSlotModClass(modType));
		bool flag = this._E001 == EModClass.Gear || this._E001 == EModClass.Master;
		_boneNamePanel.gameObject.SetActive(flag);
		if (flag)
		{
			this._E002 = ItemViewFactory.GetModSlotName(modType).Localized().ToUpper();
			_boneName.text = this._E002;
		}
		_E008(slot);
		Item containedItem = slot.ContainedItem;
		if (containedItem == null)
		{
			EmptyItemView emptyItemView = UnityEngine.Object.Instantiate(_emptyItemViewTemplate, _moddingItemContainer, worldPositionStays: false);
			emptyItemView.Show(_E133);
			emptyItemView.Interactable = false;
			UI.AddDisposable(emptyItemView);
		}
		else
		{
			_E000(_E133, containedItem, _E19A as _EB1E, _moddingItemContainer);
		}
		_E007();
		SetLockedStatus(item.VitalParts.Contains(slot) && _E19A.IsItemEquipped(item));
	}

	private void _E000(_EB66 itemContext, Item item, _EB1E itemController, Transform container)
	{
		ItemView itemView = _E089.CreateItemView(item, itemContext, ItemRotation.Horizontal, itemController, null, null, null, slotView: false, canSelect: false, searched: true);
		Transform obj = itemView.transform;
		obj.localPosition = Vector3.zero;
		obj.rotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		obj.SetParent(container, worldPositionStays: false);
		UI.AddDisposable(delegate
		{
			itemView.Kill();
		});
	}

	private void _E001(PointerEventData pointerData)
	{
		if (_E0F2)
		{
			_E02A.Close();
			return;
		}
		if (!_E242.Open || _E133 != _E242.ItemContext)
		{
			_dropDownButton.gameObject.SetActive(value: false);
		}
		_E244.HideModHighlight(overriding: true);
	}

	private void _E002(PointerEventData pointerData)
	{
		if (_E0F2)
		{
			_E02A.Show(_ED3E._E000(247203).Localized());
			return;
		}
		_dropDownButton.gameObject.SetActive(value: true);
		_E244.HighlightMod(_E243, Color, overriding: true);
		base.transform.SetAsLastSibling();
	}

	private void _E003()
	{
		if (_E242.Open)
		{
			_E242.Close();
			if (_E133 == _E242.ItemContext)
			{
				return;
			}
		}
		_E242.Show(_E133, this, _E19A as _EB1E);
	}

	public void SetLockedStatus(bool locked)
	{
		_E0F2 = locked;
		_canvasGroup.alpha = (_E0F2 ? 0.3f : 1f);
		_canvasGroup.blocksRaycasts = !_E0F2;
		_canvasGroup.interactable = !_E0F2;
	}

	private void _E004(ModdingScreenSlotView slotView)
	{
		if (!(slotView != this))
		{
			_E006(isOpen: false);
		}
	}

	private void _E005(ModdingScreenSlotView slotView)
	{
		_E006(slotView == this);
		if (slotView != this)
		{
			_dropDownButton.gameObject.SetActive(value: false);
		}
	}

	private void _E006(bool isOpen)
	{
		_dropDownButton.image.sprite = (isOpen ? _E245 : _E246);
	}

	private void _E007()
	{
		switch (this._E001)
		{
		case EModClass.Functional:
			Color = (this._E000 ? new Color32(214, 53, 53, byte.MaxValue) : new Color32(242, 242, 242, byte.MaxValue));
			break;
		case EModClass.Gear:
			Color = (this._E000 ? new Color32(214, 53, 53, byte.MaxValue) : new Color32(167, 249, 142, byte.MaxValue));
			break;
		case EModClass.Master:
			Color = (this._E000 ? new Color32(214, 53, 53, byte.MaxValue) : new Color32(16, 93, 165, byte.MaxValue));
			break;
		}
		_boneNameBackground.color = (this._E000 ? ((Color)new Color32(179, 27, 27, 220)) : Color.black);
		_boneName.color = (this._E000 ? Color.black : Color.white);
		_lineRenderer.startColor = Color;
		_lineRenderer.endColor = Color;
		string text = "";
		switch (this._E001)
		{
		case EModClass.Functional:
			text = ((_E247.ContainedItem != null) ? _ED3E._E000(257658) : _ED3E._E000(257622));
			break;
		case EModClass.Gear:
			text = ((_E247.ContainedItem != null) ? _ED3E._E000(257666) : _ED3E._E000(257690));
			break;
		case EModClass.Master:
			text = ((_E247.ContainedItem != null) ? _ED3E._E000(257743) : _ED3E._E000(257702));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EModClass.None:
		case EModClass.All:
		case EModClass.Auxiliary:
			break;
		}
		_boneIcon.sprite = _E905.Pop<Sprite>(_ED3E._E000(257772) + text);
		_boneIcon.SetNativeSize();
	}

	public void CheckVisibility(EModClass visibleClasses)
	{
		bool flag = (visibleClasses & this._E001) != 0;
		base.gameObject.SetActive(flag);
		if (!flag && _E242.Open)
		{
			if (_E242.Open)
			{
				_E242.Close();
			}
			_dropDownButton.gameObject.SetActive(value: false);
		}
	}

	public void SetBoneScreenPosition(Vector2 boneScreenPosition)
	{
		_boneIcon.rectTransform.anchoredPosition = boneScreenPosition;
		Vector3 position = _E244.ScreenToWorldPoint(_boneIcon.transform.TransformPoint(Vector3.zero));
		position.z += 1f;
		_lineRenderer.SetPosition(0, position);
	}

	public void SetSlotScreenPosition(Vector2 slotScreenPosition)
	{
		_slotCircleIcon.anchoredPosition = slotScreenPosition;
		Vector3 position = _E244.ScreenToWorldPoint(_slotCircleIcon.transform.TransformPoint(Vector3.zero));
		position.z += 1f;
		_lineRenderer.SetPosition(1, position);
	}

	public void SetDebugValue(string debugValue)
	{
		DebugText.text = debugValue;
	}

	private void _E008(Slot slot)
	{
		_E54D itemTemplates = Singleton<_E63B>.Instance.ItemTemplates;
		IEnumerable<Type> source = from node in slot.Filters.SelectMany((ItemFilter x) => x.Filter).ToArray()
			select (!_EA59.TypeTable.ContainsKey(node)) ? ((!itemTemplates.ContainsKey(node)) ? null : _EA59.TypeTable[itemTemplates[node]._parent]) : _EA59.TypeTable[node];
		string[] source2 = slot.Filters.Where((ItemFilter f) => f.ExcludedFilter != null).SelectMany((ItemFilter x) => x.ExcludedFilter).ToArray();
		IEnumerable<Type> excludeTypes = source2.Select((string node) => (!_EA59.TypeTable.ContainsKey(node)) ? ((!itemTemplates.ContainsKey(node)) ? null : _EA59.TypeTable[itemTemplates[node]._parent]) : _EA59.TypeTable[node]);
		foreach (Sprite item in (from sprite in source.Where((Type t) => excludeTypes.All((Type excludeType) => excludeType != t)).Select(ItemViewFactory.LoadModIconSprite)
			where sprite != null
			select sprite).Distinct())
		{
			Image image = UnityEngine.Object.Instantiate(_E905.Pop<Image>(_ED3E._E000(257818)), _filtersContainer, worldPositionStays: false);
			image.sprite = item;
			image.SetNativeSize();
		}
	}

	private void _E009()
	{
		for (int num = _filtersContainer.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.DestroyImmediate(_filtersContainer.GetChild(num).gameObject);
		}
	}

	public override void Close()
	{
		_E009();
		_E242.OnMenuOpen -= _E005;
		_E242.OnMenuClosed -= _E004;
		base.Close();
	}
}
