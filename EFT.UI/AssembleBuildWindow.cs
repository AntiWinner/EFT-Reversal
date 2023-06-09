using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class AssembleBuildWindow : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EBA8 handbook;

		public _EAED inventoryController;

		public ItemUiContext itemUiContext;

		public _ECB1 insuranceCompany;

		internal void _E000(Item item, AssembleModPanel view)
		{
			view.Show(handbook[item.TemplateId], item, inventoryController, itemUiContext, insuranceCompany);
		}
	}

	[SerializeField]
	private TextMeshProUGUI _caption;

	[SerializeField]
	private TextMeshProUGUI _warningTitle;

	[SerializeField]
	private TextMeshProUGUI _warningDescription;

	[SerializeField]
	private AssembleModPanel _modPanel;

	[SerializeField]
	private RectTransform _modPanelContainer;

	[SerializeField]
	private Toggle _includePartsToggle;

	[SerializeField]
	private TextMeshProUGUI _includePartsLabel;

	[SerializeField]
	private DefaultUIButton _ignoreButton;

	[SerializeField]
	private DefaultUIButton _buyButton;

	[SerializeField]
	private Button _closeButton;

	private readonly _ECEF<Item> _E08A = new _ECEF<Item>();

	private Action<bool> _E08B;

	private Action _E08C;

	private bool _E08D = true;

	private List<Item> _E08E;

	private List<Item> _E08F;

	private void Awake()
	{
		_ignoreButton.OnClick.AddListener(delegate
		{
			_E08C?.Invoke();
			Close();
		});
		_buyButton.OnClick.AddListener(delegate
		{
			_E08B?.Invoke(_E08D);
			Close();
		});
		_closeButton.onClick.AddListener(Close);
		_includePartsLabel.text = _ED3E._E000(250076).Localized();
		_includePartsToggle.onValueChanged.AddListener(delegate(bool arg)
		{
			_E08D = arg;
			UpdateModList(_E08D ? _E08E : _E08F);
		});
		_caption.text = _ED3E._E000(250048).Localized();
		_warningTitle.text = _ED3E._E000(250105).Localized();
		_warningDescription.text = _ED3E._E000(250112).Localized();
	}

	public void Show(_EBA8 handbook, List<Item> outOfCollectionItems, List<Item> itemsInBuild, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany, Action onIgnoreButtonClicked, Action<bool> onBuyButtonClicked)
	{
		_E08E = outOfCollectionItems;
		_E08F = itemsInBuild;
		_E08C = onIgnoreButtonClicked;
		_E08B = onBuyButtonClicked;
		_ignoreButton.Interactable = outOfCollectionItems.Count((Item x) => x is Weapon) <= 0;
		ShowGameObject();
		UI.AddDisposable(new _EC71<Item, AssembleModPanel>(_E08A, _modPanel, _modPanelContainer, delegate(Item item, AssembleModPanel view)
		{
			view.Show(handbook[item.TemplateId], item, inventoryController, itemUiContext, insuranceCompany);
		}));
		UpdateModList(_E08D ? _E08E : _E08F);
	}

	public void UpdateModList(List<Item> items)
	{
		_E08A.Clear();
		_E08A.AddRange(items);
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E08C?.Invoke();
		Close();
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E08B?.Invoke(_E08D);
		Close();
	}

	[CompilerGenerated]
	private void _E002(bool arg)
	{
		_E08D = arg;
		UpdateModList(_E08D ? _E08E : _E08F);
	}
}
