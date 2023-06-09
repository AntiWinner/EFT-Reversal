using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class AddRequirementsWindow : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EC69 offerContext;

		public AddRequirementsWindow _003C_003E4__this;

		internal void _E000()
		{
			offerContext.OnItemSelectionChanged -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _requirementsCountLabel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private DefaultUIButton _addButton;

	[SerializeField]
	private AuxiliaryMoneyPanel _moneyPanel;

	[SerializeField]
	private AuxiliaryItemPanel _itemsPanel;

	private RequirementAuxiliaryPanel _E37B;

	private Action<HandoverRequirement, _EBAB> _E37C;

	private Item _E37D;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
		_addButton.OnClick.AddListener(delegate
		{
			_E37C(_E37B.Requirement, _E37B.HandbookNode);
			Close();
		});
		_closeButton.onClick.AddListener(Close);
	}

	public void Show(_ECBD ragfair, _EC69 offerContext, SimpleContextMenu contextMenu, _EBA8 handbook, Action<HandoverRequirement, _EBAB> onRequirementSelected, [CanBeNull] Item offerItem)
	{
		_E37C = onRequirementSelected;
		_E37D = offerItem;
		offerContext.OnItemSelectionChanged += _E000;
		UI.AddDisposable(delegate
		{
			offerContext.OnItemSelectionChanged -= _E000;
		});
		ShowGameObject();
		CorrectPosition();
		_moneyPanel.Show(ragfair, contextMenu, ERequirementType.Money, handbook, _E001);
		_itemsPanel.Show(_E37D, ragfair, contextMenu, ERequirementType.Item, handbook, _E001);
		_E001(_moneyPanel);
	}

	private void _E000(Item item, bool selected)
	{
		if (item.TemplateId != _E37D.TemplateId)
		{
			Close();
		}
	}

	public void SetRequirementsCountLabel(int count)
	{
		_requirementsCountLabel.text = string.Format(_ED3E._E000(242139).Localized() + _ED3E._E000(242172), count);
		CorrectPosition();
	}

	private void _E001(RequirementAuxiliaryPanel panel)
	{
		if (_E37B != null)
		{
			_E37B.Deselect();
		}
		_E37B = panel;
		_E37B.Select();
		_E002(panel.ShowAddButton);
	}

	private void _E002(bool value)
	{
		_addButton.Interactable = value;
	}

	public override void Close()
	{
		_moneyPanel.Close();
		_itemsPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E37C(_E37B.Requirement, _E37B.HandbookNode);
		Close();
	}
}
