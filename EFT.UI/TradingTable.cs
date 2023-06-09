using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class TradingTable : UIElement
{
	[SerializeField]
	private TradingTableGridView _tableGridView;

	[SerializeField]
	private Button _clearTableButton;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private _E8AF _E10A;

	private void Awake()
	{
		_clearTableButton.onClick.AddListener(delegate
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
			_E10A.ClearSellItems();
		});
	}

	public void Show(_E8AF assortment, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		_E10A = assortment;
		UI.BindEvent(_E10A.TransactionChanged, delegate
		{
			bool transactionInProgress = _E10A.TransactionInProgress;
			_canvasGroup.interactable = !transactionInProgress;
			_canvasGroup.alpha = (transactionInProgress ? 0.3f : 1f);
			_canvasGroup.blocksRaycasts = !transactionInProgress;
		});
		ShowGameObject();
		_tableGridView.Show(assortment.SellingTableGrid, assortment, inventoryController, itemUiContext);
	}

	public override void Close()
	{
		_tableGridView.Hide();
		_E10A.ClearPreparedItems();
		_E10A.ClearSellItems();
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
		_E10A.ClearSellItems();
	}

	[CompilerGenerated]
	private void _E001()
	{
		bool transactionInProgress = _E10A.TransactionInProgress;
		_canvasGroup.interactable = !transactionInProgress;
		_canvasGroup.alpha = (transactionInProgress ? 0.3f : 1f);
		_canvasGroup.blocksRaycasts = !transactionInProgress;
	}
}
