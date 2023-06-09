using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Ragfair;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.HandBook;

public sealed class HandbookScreen : EftScreen<HandbookScreen._E000, HandbookScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, HandbookScreen>
	{
		public readonly _EAED InventoryController;

		public readonly _E796 Session;

		public readonly _EBA8 Handbook;

		public readonly _E9C4 HealthController;

		public override EEftScreenType ScreenType => EEftScreenType.Handbook;

		public _E000(_EAED inventoryController, _E796 session, _EBA8 handbook, _E9C4 healthController)
		{
			InventoryController = inventoryController;
			Session = session;
			Handbook = handbook;
			HealthController = healthController;
		}
	}

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private HandbookCategoriesPanel _categoriesPanel;

	private new ItemUiContext m__E000;

	private NodeBaseView m__E001;

	private void Awake()
	{
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.InventoryController, controller.Session, controller.Handbook, controller.HealthController);
	}

	private void Show(_EAED inventoryController, _E796 session, _EBA8 handbook, _E9C4 healthController)
	{
		ShowGameObject();
		this.m__E000 = ItemUiContext.Instance;
		this.m__E000.Configure(inventoryController, session.Profile, session, session.InsuranceCompany, session.RagFair, null, healthController, new _EA40[1] { inventoryController.Inventory.Stash }, EItemUiContextType.RagfairScreen, ECursorResult.ShowCursor);
		_EBAC encyclopediaNodes = handbook.EncyclopediaNodes;
		_EBAC filteredNodes = new _EBAC(encyclopediaNodes);
		_categoriesPanel.Show(session.RagFair, handbook, encyclopediaNodes, filteredNodes, this.m__E000, session.Profile.Stats.CarriedQuestItems, delegate(NodeBaseView view, string id)
		{
			if (this.m__E001 != null)
			{
				this.m__E001.DeselectView();
			}
			this.m__E001 = view;
		}).HandleExceptions();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		_categoriesPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E001(NodeBaseView view, string id)
	{
		if (this.m__E001 != null)
		{
			this.m__E001.DeselectView();
		}
		this.m__E001 = view;
	}
}
