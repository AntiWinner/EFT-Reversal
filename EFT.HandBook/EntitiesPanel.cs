using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

namespace EFT.HandBook;

public sealed class EntitiesPanel : UIElement
{
	[SerializeField]
	private HandbookItemPreview _itemPreview;

	[SerializeField]
	private Transform _entityListContent;

	[SerializeField]
	private EntityListElement _entityListElement;

	private _EC71<_EBAB, EntityListElement> _E07E;

	private readonly _ECEF<_EBAB> _E07F = new _ECEF<_EBAB>();

	private _EC71<_EBAB, EntityListElement> _E080;

	private _EBA8 _E081;

	private List<string> _E082;

	private EntityListElement _E083;

	public void Show(_EBA8 handbook, ItemUiContext itemUiContext, List<string> questItems)
	{
		_E081 = handbook;
		_E082 = questItems;
		ShowGameObject();
		_itemPreview.Show(itemUiContext);
		_E080 = _E000(_E07F);
	}

	public void ShowEntity(_EBAB node)
	{
		ClearEntity();
		_E07E = _E000(new _ECEF<_EBAB>(node.Children));
	}

	private _EC71<_EBAB, EntityListElement> _E000(_ED05<_EBAB> nodes)
	{
		return UI.AddDisposable(new _EC6F<_EBAB, EntityListElement>(nodes, _EC6A.Instance, _entityListElement, _entityListContent, delegate(_EBAB item, EntityListElement view)
		{
			Item item2 = item.Data.Item;
			if (item2 != null && (!item2.QuestItem || _E082.Contains(item2.TemplateId)))
			{
				view.Show(item, _E001);
			}
		}));
	}

	public void AddToFilteredList(_EBAB node)
	{
		_E07F.Add(node);
	}

	private void _E001(_EBAB node)
	{
		EntityListElement entityListElement = null;
		if (_E07E != null && _E07E.Contains(node))
		{
			entityListElement = _E07E[node];
		}
		else if (_E080 != null && _E080.Contains(node))
		{
			entityListElement = _E080[node];
		}
		if (!(entityListElement == null))
		{
			if (_E083 != null)
			{
				_E083.DeselectView();
			}
			_E083 = entityListElement;
			HandbookData data = node.Data;
			if (data.Type == ENodeType.Item)
			{
				_itemPreview.ChooseEntity(data.Item);
				_E081.ReadEncyclopedia(node);
			}
			else
			{
				ShowEntity(node);
			}
		}
	}

	public void ClearEntity()
	{
		_itemPreview.ClearEntity();
		_E07E?.Dispose();
		if (_E07F.Count > 0)
		{
			_E07F.Clear();
		}
	}

	public override void Close()
	{
		ClearEntity();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002(_EBAB item, EntityListElement view)
	{
		Item item2 = item.Data.Item;
		if (item2 != null && (!item2.QuestItem || _E082.Contains(item2.TemplateId)))
		{
			view.Show(item, _E001);
		}
	}
}
