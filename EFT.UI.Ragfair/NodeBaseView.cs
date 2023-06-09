using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.HandBook;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class NodeBaseView : UIElement, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	private const float _E32E = 0.3f;

	[SerializeField]
	protected TextMeshProUGUI CategoryElementName;

	[SerializeField]
	protected TextMeshProUGUI CategoryChildCount;

	[SerializeField]
	private bool _canBeSelected;

	[SerializeField]
	private GameObject _newNodeObject;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private HorizontalLayoutGroup _layoutGroup;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _iconBack;

	[SerializeField]
	private GameObject _iconPanel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private Color _selectedBackgroundColor = new Color32(197, 195, 178, byte.MaxValue);

	[SerializeField]
	private Color32 _defaultTextColor = new Color32(197, 195, 178, byte.MaxValue);

	[SerializeField]
	private Color32 _defaultChildCountColor = new Color32(166, 176, 181, byte.MaxValue);

	[SerializeField]
	private Color32 _hoverTextColor = Color.black;

	public _EBAB Node;

	protected EViewListType ViewListType;

	protected EWindowType WindowType;

	private HoverTrigger _E32F;

	private bool _E330;

	private bool _E331;

	private float _E332;

	[CompilerGenerated]
	private _ECBD _E1C5;

	[CompilerGenerated]
	private Dictionary<string, NodeBaseView> _E333;

	[CompilerGenerated]
	private Action<NodeBaseView, string> _E334;

	[CompilerGenerated]
	private Action<NodeBaseView, string> _E335;

	[CompilerGenerated]
	private string _E336;

	[CompilerGenerated]
	private NodeBaseView _E337;

	[CompilerGenerated]
	private NodeBaseView _E338;

	[CompilerGenerated]
	private bool _E088;

	protected bool CanBeSelected => _canBeSelected;

	protected bool CanBeExpanded => _E331;

	private Color _E000
	{
		get
		{
			Color backgroundColor = Node.Data.BackgroundColor;
			if (!(backgroundColor != Color.clear))
			{
				return DefaultBackgroundColor;
			}
			return new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.35f);
		}
	}

	private Color _E001
	{
		get
		{
			Color color = this._E000;
			return new Color(color.r, color.g, color.b, 0.85f);
		}
	}

	protected virtual Color DefaultBackgroundColor => new Color32(30, 30, 30, 80);

	protected _ECBD Ragfair
	{
		[CompilerGenerated]
		get
		{
			return _E1C5;
		}
		[CompilerGenerated]
		private set
		{
			_E1C5 = value;
		}
	}

	protected Dictionary<string, NodeBaseView> ViewNodes
	{
		[CompilerGenerated]
		get
		{
			return _E333;
		}
		[CompilerGenerated]
		private set
		{
			_E333 = value;
		}
	}

	protected Action<NodeBaseView, string> OnCategorySelected
	{
		[CompilerGenerated]
		get
		{
			return _E334;
		}
		[CompilerGenerated]
		private set
		{
			_E334 = value;
		}
	}

	protected Action<NodeBaseView, string> OnCategoryConfirmed
	{
		[CompilerGenerated]
		get
		{
			return _E335;
		}
		[CompilerGenerated]
		private set
		{
			_E335 = value;
		}
	}

	protected string ForbiddenItemId
	{
		[CompilerGenerated]
		get
		{
			return _E336;
		}
		[CompilerGenerated]
		private set
		{
			_E336 = value;
		}
	}

	protected bool IsForbidden
	{
		get
		{
			if (ViewListType == EViewListType.WeaponBuild)
			{
				return false;
			}
			Item item = Node.Data.Item;
			if (item == null)
			{
				return ForbiddenItemId == Node.Data.Id;
			}
			if (!item.CanRequireOnRagfair)
			{
				return true;
			}
			if (ForbiddenItemId == item.TemplateId)
			{
				return true;
			}
			return ForbiddenItemId == Node.Data.Id;
		}
	}

	protected NodeBaseView CategoryView
	{
		[CompilerGenerated]
		get
		{
			return _E337;
		}
		[CompilerGenerated]
		private set
		{
			_E337 = value;
		}
	}

	protected NodeBaseView SubcategoryView
	{
		[CompilerGenerated]
		get
		{
			return _E338;
		}
		[CompilerGenerated]
		private set
		{
			_E338 = value;
		}
	}

	private bool _E002
	{
		[CompilerGenerated]
		get
		{
			return _E088;
		}
		[CompilerGenerated]
		set
		{
			_E088 = value;
		}
	}

	public virtual void Show([CanBeNull] _ECBD ragfair, NodeBaseView categoryView, NodeBaseView subcategoryView, _EBAB node, EViewListType viewListType, EWindowType windowType, Dictionary<string, NodeBaseView> viewNodes, string forbiddenItem, Action<NodeBaseView, string> onCategorySelected, Action<NodeBaseView, string> onCategoryConfirmed = null)
	{
		Ragfair = ragfair;
		ViewListType = viewListType;
		WindowType = windowType;
		ViewNodes = viewNodes;
		CategoryView = categoryView;
		SubcategoryView = subcategoryView;
		Node = node;
		OnCategorySelected = onCategorySelected;
		OnCategoryConfirmed = onCategoryConfirmed;
		ForbiddenItemId = forbiddenItem;
		if (ViewListType == EViewListType.Handbook && Node.Data.Type == ENodeType.Item)
		{
			return;
		}
		string text = (node.Data.FromBuild ? node.Data.Name : node.Data.Name.Localized());
		base.gameObject.name = text;
		Color backgroundColor = Node.BackgroundColor;
		_iconBack.color = backgroundColor;
		_defaultTextColor = Node.TextColor;
		if (_E32F == null)
		{
			_E32F = _background.gameObject.AddComponent<HoverTrigger>();
		}
		_E32F.Init(PointerEnterHandler, PointerExitHandler);
		CategoryElementName.text = text;
		_layoutGroup.padding.left = 30 + 30 * Node.Depth;
		if (ViewListType.IsUpdateChildStatus())
		{
			Node.OnCountUpdated += OffersCountUpdatedHandler;
			UI.AddDisposable(delegate
			{
				Node.OnCountUpdated -= OffersCountUpdatedHandler;
			});
		}
		if (ViewListType == EViewListType.Handbook)
		{
			_canBeSelected = false;
			_E331 = false;
			foreach (_EBAB child in Node.Children)
			{
				if (child != null)
				{
					if (child.Data.Type == ENodeType.Item)
					{
						_canBeSelected = true;
					}
					if (child.Data.Type == ENodeType.Category)
					{
						_E331 = true;
					}
				}
			}
			Node.OnNewStatusUpdated += _E000;
			UI.AddDisposable(delegate
			{
				Node.OnNewStatusUpdated -= _E000;
			});
			_E000(Node.New);
		}
		OffersCountUpdatedHandler(Node);
		DeselectView();
		string icon = Node.Data.Icon;
		bool flag = !string.IsNullOrEmpty(icon) && icon != _ED3E._E000(197369);
		_icon.gameObject.SetActive(value: false);
		_loader.gameObject.SetActive(flag);
		_iconPanel.gameObject.SetActive(flag);
		if (_ECBD.IconsLoader == null || !flag || _E330)
		{
			return;
		}
		_ECBD.IconsLoader.GetIcon(icon, delegate(Sprite arg)
		{
			if (!(this == null) && !(base.gameObject == null))
			{
				_E330 = true;
				_icon.gameObject.SetActive(value: true);
				_loader.gameObject.SetActive(value: false);
				_icon.sprite = arg;
				_icon.SetNativeSize();
			}
		});
	}

	public virtual void PointerEnterHandler([NotNull] PointerEventData eventData)
	{
		if (!this._E002)
		{
			_background.color = this._E001;
		}
	}

	public virtual void PointerExitHandler([NotNull] PointerEventData eventData)
	{
		if (!this._E002)
		{
			DeselectView();
		}
	}

	public void OnPointerUp([NotNull] PointerEventData eventData)
	{
	}

	public void OnPointerDown([NotNull] PointerEventData eventData)
	{
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (!CanBeSelected || (Ragfair != null && Ragfair.GettingOffers))
		{
			return;
		}
		if (this._E002)
		{
			if (Time.time - _E332 <= 0.3f)
			{
				OnCategoryConfirmed?.Invoke(this, Node.Data.Id);
				_E332 = 0f;
			}
			else
			{
				_E332 = Time.time;
			}
		}
		else
		{
			_E332 = Time.time;
			SelectView();
			OnCategorySelected(this, Node.Data.Id);
		}
	}

	public void DeselectView()
	{
		this._E002 = false;
		_background.color = this._E000;
		CategoryElementName.color = _defaultTextColor;
		CategoryChildCount.color = _defaultChildCountColor;
	}

	public void SelectView()
	{
		this._E002 = true;
		_background.color = _selectedBackgroundColor;
		CategoryElementName.color = _hoverTextColor;
		CategoryChildCount.color = _hoverTextColor;
	}

	protected virtual void OffersCountUpdatedHandler(_EBAB node)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		int count = node.Count;
		CategoryChildCount.text = _ED3E._E000(27312) + count + _ED3E._E000(27308);
		if (Ragfair.FilterRule.HandbookId == node.Data?.Id)
		{
			base.gameObject.SetActive(value: true);
			return;
		}
		if (ViewListType != EViewListType.WishList)
		{
			CategoryChildCount.gameObject.SetActive(count > 0);
		}
		base.gameObject.SetActive(Node.CanShowNodeView(ViewListType));
	}

	private void _E000(bool isNew)
	{
		if (!(this == null) && !(_newNodeObject == null))
		{
			_newNodeObject.SetActive(isNew);
		}
	}

	[CompilerGenerated]
	private void _E001()
	{
		Node.OnCountUpdated -= OffersCountUpdatedHandler;
	}

	[CompilerGenerated]
	private void _E002()
	{
		Node.OnNewStatusUpdated -= _E000;
	}

	[CompilerGenerated]
	private void _E003(Sprite arg)
	{
		if (!(this == null) && !(base.gameObject == null))
		{
			_E330 = true;
			_icon.gameObject.SetActive(value: true);
			_loader.gameObject.SetActive(value: false);
			_icon.sprite = arg;
			_icon.SetNativeSize();
		}
	}
}
