using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[UsedImplicitly]
public sealed class TraderDialogWindow : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E8BB dialogController;

		public TraderDialogWindow _003C_003E4__this;

		internal void _E000()
		{
			dialogController.OnDialogChanged -= _003C_003E4__this._E002;
			dialogController.OnBlockDialog -= _003C_003E4__this._E006;
		}
	}

	[SerializeField]
	private TraderDialogHistoryView _dialogHistoryView;

	[SerializeField]
	private ToggleButton _historyButton;

	[SerializeField]
	private TraderDialogWindowOptionRow _dialogRow;

	[SerializeField]
	private TextMeshProUGUI _traderText;

	[SerializeField]
	private TextMeshProUGUI _traderName;

	[SerializeField]
	private Image _linesIcon;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private RectTransform _linesContainer;

	private _E8BB _E224;

	private readonly _E3A4 _E225 = new _E3A4();

	private readonly List<TraderDialogWindowOptionRow> _E21D = new List<TraderDialogWindowOptionRow>();

	public void Show(_E8BB dialogController)
	{
		UI.Dispose();
		UI.AddDisposable(_E225);
		UI.SubscribeEvent(_historyButton.OnToggle, _E001);
		_E224 = dialogController;
		_E5CB.TraderSettings settings = dialogController.Trader.Settings;
		_traderName.text = settings.Nickname.Localized();
		settings.GetAndAssignAvatar(_linesIcon, base.CancellationToken);
		dialogController.OnDialogChanged += _E002;
		dialogController.OnBlockDialog += _E006;
		UI.AddDisposable(delegate
		{
			dialogController.OnDialogChanged -= _E002;
			dialogController.OnBlockDialog -= _E006;
		});
		_E002(dialogController.CurrentDialog);
	}

	private void _E000()
	{
		_historyButton.SetValueSilently(isOn: false);
		_dialogHistoryView.Close();
	}

	private void _E001(bool isActive)
	{
		if (isActive)
		{
			_dialogHistoryView.Show(_E224.History);
		}
		else
		{
			_dialogHistoryView.Close();
		}
	}

	private void _E002(_E8CC dialogList)
	{
		_E225.Dispose();
		_E225.AddDisposable(_E000);
		if (dialogList == null)
		{
			HideGameObject();
			return;
		}
		ShowGameObject();
		_E005(dialogList.Description);
		_E003(dialogList.Lines);
	}

	private void _E003(IEnumerable<_E8C4> dialogLines)
	{
		int num = 0;
		int count = _E21D.Count;
		foreach (_E8C4 dialogLine in dialogLines)
		{
			TraderDialogWindowOptionRow traderDialogWindowOptionRow = ((num < count) ? _E21D[num] : _E004());
			_E225.AddDisposable(traderDialogWindowOptionRow);
			traderDialogWindowOptionRow.Show(dialogLine);
			num++;
		}
		_canvasGroup.interactable = true;
		_canvasGroup.alpha = 1f;
		_canvasGroup.blocksRaycasts = true;
	}

	private TraderDialogWindowOptionRow _E004()
	{
		TraderDialogWindowOptionRow traderDialogWindowOptionRow = Object.Instantiate(_dialogRow, _linesContainer, worldPositionStays: false);
		_E21D.Add(traderDialogWindowOptionRow);
		return traderDialogWindowOptionRow;
	}

	private void _E005(string traderText)
	{
		_traderText.text = traderText;
	}

	private void _E006()
	{
		_canvasGroup.interactable = false;
		_canvasGroup.alpha = 0.5f;
		_canvasGroup.blocksRaycasts = false;
	}
}
