using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ItemsListWindow : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemsListWindow _003C_003E4__this;

		public _EBA8 handbook;

		public _EAED inventoryController;

		public ItemUiContext itemUiContext;

		public _ECB1 insuranceCompany;

		internal AssembleModPanel _E000(Item arg)
		{
			return _003C_003E4__this._assemblePanelTemplate;
		}

		internal Transform _E001(Item arg)
		{
			return _003C_003E4__this._assemblePanelContainer;
		}

		internal void _E002(Item item, AssembleModPanel panel)
		{
			_EBAB obj = handbook[item.TemplateId];
			if (obj != null)
			{
				panel.Show(obj, item, inventoryController, itemUiContext, insuranceCompany);
			}
		}
	}

	private const string _E169 = "LOOT FROM SCAVS";

	private const string _E16A = "Scavs has brought you:";

	[SerializeField]
	private CustomTextMeshProUGUI _captionLabel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private AssembleModPanel _assemblePanelTemplate;

	[SerializeField]
	private RectTransform _assemblePanelContainer;

	[SerializeField]
	private CustomTextMeshProUGUI _messageLabel;

	[SerializeField]
	private DefaultUIButton _receiveButtonSpawner;

	private void Awake()
	{
		_closeButton.onClick.AddListener(Close);
		_receiveButtonSpawner.OnClick.AddListener(Close);
	}

	public void Show(IEnumerable<Item> newItems, _EBA8 handbook, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany)
	{
		ShowGameObject();
		_captionLabel.text = _ED3E._E000(248907).Localized();
		_messageLabel.text = _ED3E._E000(248955).Localized();
		UI.AddDisposable(new _EC79<Item, AssembleModPanel>(newItems, (Item arg) => _assemblePanelTemplate, (Item arg) => _assemblePanelContainer, delegate(Item item, AssembleModPanel panel)
		{
			_EBAB obj = handbook[item.TemplateId];
			if (obj != null)
			{
				panel.Show(obj, item, inventoryController, itemUiContext, insuranceCompany);
			}
		}));
	}
}
