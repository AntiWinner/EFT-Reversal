using System;
using System.Collections.Generic;
using EFT.UI.Ragfair;
using UnityEngine;

namespace EFT.HandBook;

public sealed class HandbookCategoryView : CategoryView
{
	[SerializeField]
	private RectTransform _nodesContainer;

	public override void Show(_ECBD ragfair, NodeBaseView categoryView, NodeBaseView subcategoryView, _EBAB node, EViewListType viewListType, EWindowType windowType, Dictionary<string, NodeBaseView> viewNodes, string forbiddenItem, Action<NodeBaseView, string> onCategorySelected, Action<NodeBaseView, string> onCategoryConfirmed = null)
	{
		base.Show(ragfair, categoryView, subcategoryView, node, viewListType, windowType, viewNodes, forbiddenItem, onCategorySelected, onCategoryConfirmed);
		base.ToggleObject.SetActive(!base.CanBeSelected || base.CanBeExpanded);
	}

	protected override RectTransform GetSubcategoryContainer()
	{
		return _nodesContainer;
	}
}
