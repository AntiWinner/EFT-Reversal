using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class SubcategoryView : NodeBaseView
{
	private const int _E339 = 32;

	private const int _E33A = 25;

	private const int _E33B = 16;

	private const int _E33C = 14;

	[SerializeField]
	private LayoutElement _mainLayoutElement;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	public override void Show(_ECBD ragfair, NodeBaseView categoryView, NodeBaseView subcategoryView, _EBAB node, EViewListType viewListType, EWindowType windowType, Dictionary<string, NodeBaseView> viewNodes, string forbiddenItem, Action<NodeBaseView, string> onCategorySelected, Action<NodeBaseView, string> onCategoryConfirmed = null)
	{
		base.Show(ragfair, categoryView, subcategoryView, node, viewListType, windowType, viewNodes, forbiddenItem, onCategorySelected, onCategoryConfirmed);
		if (_canvasGroup != null)
		{
			_canvasGroup.SetUnlockStatus(!base.IsForbidden);
		}
		_E000(node);
	}

	private void _E000(_EBAB node)
	{
		bool flag = node.Parent == null || node.Children.Count > 0;
		_mainLayoutElement.preferredHeight = (flag ? 32 : 25);
		int num = (flag ? 16 : 14);
		CategoryElementName.fontSize = num;
		CategoryChildCount.fontSize = num;
	}

	protected override void OffersCountUpdatedHandler(_EBAB node)
	{
		if (!(this == null) && !(base.gameObject == null))
		{
			if (ViewListType == EViewListType.WeaponBuild && WindowType == EWindowType.OpenBuild)
			{
				base.gameObject.SetActive(value: true);
				CategoryChildCount.gameObject.SetActive(value: false);
			}
			else
			{
				base.OffersCountUpdatedHandler(node);
			}
		}
	}
}
