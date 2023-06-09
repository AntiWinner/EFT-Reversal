using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.Ragfair;

public class AuxiliaryItemPanel : RequirementAuxiliaryPanel
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public AuxiliaryItemPanel _003C_003E4__this;

		public Action<RequirementAuxiliaryPanel> onPanelSelected;

		internal void _E000(NodeBaseView view, string id)
		{
			if (_003C_003E4__this._E000)
			{
				_003C_003E4__this._E21A.DeselectView();
			}
			_003C_003E4__this._E21A = view;
			_003C_003E4__this._onlyFunctionalPanel.SetActive(view.Node.Data.Item is _EA40);
			_003C_003E4__this._quantityCanvasGroup.SetUnlockStatus(value: true);
			_003C_003E4__this._E000();
			onPanelSelected(_003C_003E4__this);
		}
	}

	[SerializeField]
	private RagfairCategoriesPanel _browseCategoriesPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _estimatedPriceLabel;

	[SerializeField]
	private GameObject _onlyFunctionalPanel;

	[SerializeField]
	private UpdatableToggle _onlyFunctionalToggle;

	[SerializeField]
	private CanvasGroup _quantityCanvasGroup;

	private _EBAC _E368;

	private NodeBaseView _E21A;

	private bool _E1CA;

	public override _EBAB HandbookNode
	{
		get
		{
			if (!(_E21A == null))
			{
				return _E21A.Node;
			}
			return null;
		}
	}

	public override HandoverRequirement Requirement => new HandoverRequirement(_E21A.Node.Data.Id, base.Quantity, _onlyFunctionalToggle.isOn);

	public override bool ShowAddButton
	{
		get
		{
			if (_E21A != null)
			{
				return base.Quantity > 0;
			}
			return false;
		}
	}

	protected override int MaxQuantityValue => 100000;

	private bool _E000 => _E21A != null;

	protected override void Awake()
	{
		base.Awake();
		ItemQuantity.onEndEdit.AddListener(delegate
		{
			_E000();
		});
	}

	public void Show([CanBeNull] Item offerItem, _ECBD ragfair, SimpleContextMenu contextMenu, ERequirementType type, _EBA8 handbook, Action<RequirementAuxiliaryPanel> onPanelSelected)
	{
		base.Show(ragfair, contextMenu, type, handbook, onPanelSelected);
		_E368 = new _EBAC(handbook.StructuredKnownItems);
		_EBAC filteredNodes = new _EBAC(_E368);
		_onlyFunctionalPanel.SetActive(value: false);
		_quantityCanvasGroup.SetUnlockStatus(value: false);
		string forbiddenItem = ((offerItem != null && offerItem.CanBeRagfairForbidden) ? offerItem.TemplateId : string.Empty);
		_browseCategoriesPanel.Show(forbiddenItem, ragfair, handbook, null, _E368, filteredNodes, contextMenu, EViewListType.RequirementsWindow, EWindowType.Default, delegate(NodeBaseView view, string id)
		{
			if (this._E000)
			{
				_E21A.DeselectView();
			}
			_E21A = view;
			_onlyFunctionalPanel.SetActive(view.Node.Data.Item is _EA40);
			_quantityCanvasGroup.SetUnlockStatus(value: true);
			_E000();
			onPanelSelected(this);
		}).HandleExceptions();
		base.Quantity = 1;
		_E000();
	}

	private void _E000()
	{
		float number = 0f;
		if (HandbookNode != null)
		{
			number = (this._E000 ? (HandbookNode.Data.Price * (float)base.Quantity) : 0f);
		}
		_estimatedPriceLabel.text = _ED3E._E000(27312) + _ED3E._E000(243535).Localized() + _ED3E._E000(242165) + number.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(27308);
	}

	public override void Close()
	{
		_browseCategoriesPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E001(string arg)
	{
		_E000();
	}
}
