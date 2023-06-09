using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class GridSortPanel : MonoBehaviour
{
	private const string m__E000 = "UI/Inventory/SortAcceptConfirmation";

	[SerializeField]
	private Button _button;

	[SerializeField]
	private GameObject _loaderPanel;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private bool m__E001;

	private _EAED m__E002;

	private _EA40 m__E003;

	public void SetUnlockStatus(bool value)
	{
		_canvasGroup.SetUnlockStatus(value);
	}

	private void Awake()
	{
		_button.onClick.AddListener(_E000);
	}

	public void Show(_EAED controller, _EA40 item)
	{
		this.m__E002 = controller;
		this.m__E003 = item;
	}

	private void _E000()
	{
		EventSystem.current.SetSelectedGameObject(null);
		if (!this.m__E001)
		{
			ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(237135).Localized(), _E001, delegate
			{
			});
		}
	}

	private void _E001()
	{
		_E002().HandleExceptions();
	}

	private async Task _E002()
	{
		_E003(inProgress: true);
		_ECD8<_EB5B> obj = _EB29.Sort(this.m__E003, this.m__E002, simulate: false);
		if (obj.Succeeded)
		{
			IResult result = await this.m__E002.TryRunNetworkTransaction(obj);
			obj.Value.RaiseEvents(this.m__E002, result.Succeed ? CommandStatus.Succeed : CommandStatus.Failed);
		}
		else if (obj.Error is InventoryError inventoryError)
		{
			_E857.DisplayWarningNotification(inventoryError.GetLocalizedDescription());
		}
		_E003(inProgress: false);
	}

	private void _E003(bool inProgress)
	{
		this.m__E001 = inProgress;
		if (!(this == null))
		{
			_loaderPanel.SetActive(inProgress);
			if (inProgress)
			{
				_loaderPanel.transform.SetAsLastSibling();
			}
		}
	}
}
