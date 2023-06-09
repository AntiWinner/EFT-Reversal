using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.HandBook;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class CategoryView : NodeBaseView
{
	[SerializeField]
	private Toggle _toggle;

	[SerializeField]
	private Transform _subcategoryContainer;

	[SerializeField]
	private Image _toggleImage;

	[SerializeField]
	private Sprite _closeSprite;

	[SerializeField]
	private Sprite _openSprite;

	[SerializeField]
	private List<NodeBaseView> _views = new List<NodeBaseView>();

	private bool _E32B;

	private _EC6F<_EBAB, NodeBaseView> _E32C;

	protected GameObject ToggleObject => _toggleImage.gameObject;

	protected override Color DefaultBackgroundColor => new Color32(70, 70, 70, 80);

	private void Awake()
	{
		_toggle.onValueChanged.AddListener(delegate(bool value)
		{
			if (value)
			{
				OpenCategory();
			}
			else
			{
				CloseCategory();
			}
		});
	}

	public override void Show(_ECBD ragfair, NodeBaseView categoryView, NodeBaseView subcategoryView, _EBAB node, EViewListType viewListType, EWindowType windowType, Dictionary<string, NodeBaseView> viewNodes, string forbiddenItem, Action<NodeBaseView, string> onCategorySelected, Action<NodeBaseView, string> onCategoryConfirmed = null)
	{
		base.Show(ragfair, categoryView, subcategoryView, node, viewListType, windowType, viewNodes, forbiddenItem, onCategorySelected, onCategoryConfirmed);
		_toggleImage.sprite = _closeSprite;
		CloseCategory();
	}

	public override void PointerEnterHandler(PointerEventData eventData)
	{
		_toggleImage.sprite = _openSprite;
		base.PointerEnterHandler(eventData);
	}

	public override void PointerExitHandler(PointerEventData eventData)
	{
		if (!_E32B)
		{
			_toggleImage.sprite = _closeSprite;
		}
		base.PointerExitHandler(eventData);
	}

	[CanBeNull]
	public IEnumerable<NodeBaseView> OpenCategory()
	{
		if (ViewListType == EViewListType.Handbook && base.CanBeSelected && !base.CanBeExpanded)
		{
			return null;
		}
		if (_E32B)
		{
			return _views;
		}
		_E32B = true;
		_toggle.isOn = true;
		_E32C = UI.AddDisposable(new _EC6F<_EBAB, NodeBaseView>(new _ECEF<_EBAB>(Node.Children), _EC6A.Instance, (_EBAB x) => _E000(x.Data.Type), (_EBAB x) => GetSubcategoryContainer(), delegate(_EBAB item, NodeBaseView view)
		{
			view.Show(base.Ragfair, base.CategoryView, base.SubcategoryView, item, ViewListType, WindowType, base.ViewNodes, base.ForbiddenItemId, base.OnCategorySelected, base.OnCategoryConfirmed);
			_views.Add(view);
			if (!base.ViewNodes.ContainsKey(item.Data.Id))
			{
				base.ViewNodes.Add(item.Data.Id, view);
			}
		}));
		return _views;
	}

	private NodeBaseView _E000(ENodeType type)
	{
		if (type != 0)
		{
			return base.SubcategoryView;
		}
		return base.CategoryView;
	}

	protected virtual RectTransform GetSubcategoryContainer()
	{
		return (RectTransform)_subcategoryContainer;
	}

	public void CloseCategory()
	{
		if (!_E32B)
		{
			return;
		}
		_E32B = false;
		_toggle.isOn = false;
		_toggleImage.sprite = _closeSprite;
		foreach (NodeBaseView view in _views)
		{
			if (base.ViewNodes.ContainsKey(view.Node.Data.Id))
			{
				base.ViewNodes.Remove(view.Node.Data.Id);
			}
		}
		_views.Clear();
		_E32C.Dispose();
	}

	protected override void OffersCountUpdatedHandler(_EBAB node)
	{
		if (!(this == null) && !(base.gameObject == null))
		{
			base.OffersCountUpdatedHandler(node);
			_toggle.gameObject.SetActive(node.Count > 0);
		}
	}

	public override void Close()
	{
		CloseCategory();
		_E32B = false;
		base.Close();
	}

	[CompilerGenerated]
	private void _E001(bool value)
	{
		if (value)
		{
			OpenCategory();
		}
		else
		{
			CloseCategory();
		}
	}

	[CompilerGenerated]
	private NodeBaseView _E002(_EBAB x)
	{
		return _E000(x.Data.Type);
	}

	[CompilerGenerated]
	private Transform _E003(_EBAB x)
	{
		return GetSubcategoryContainer();
	}

	[CompilerGenerated]
	private void _E004(_EBAB item, NodeBaseView view)
	{
		view.Show(base.Ragfair, base.CategoryView, base.SubcategoryView, item, ViewListType, WindowType, base.ViewNodes, base.ForbiddenItemId, base.OnCategorySelected, base.OnCategoryConfirmed);
		_views.Add(view);
		if (!base.ViewNodes.ContainsKey(item.Data.Id))
		{
			base.ViewNodes.Add(item.Data.Id, view);
		}
	}
}
