using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class MerchantsList : UIScreen
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E8B2 trader;

		public MerchantsList _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this._E001(trader);
		}
	}

	[SerializeField]
	private RectTransform _tradersContainer;

	[SerializeField]
	private TraderTooltip _tooltip;

	[SerializeField]
	private TraderPanel _traderPanelTemplate;

	private _E8B2 _E0A3;

	private _E935 _E0A4;

	private _EAED _E084;

	private _E9C4 _E086;

	private Profile _E085;

	private _E796 _E031;

	public void Show(IEnumerable<_E8B2> tradersList, _E935 questController, _EAED inventoryController, _E9C4 healthController, _E796 session, Profile profile)
	{
		if (tradersList == null)
		{
			throw new Exception(_ED3E._E000(261597));
		}
		_E0A4 = questController;
		_E084 = inventoryController;
		_E086 = healthController;
		_E085 = profile;
		_E031 = session;
		ShowGameObject();
		_E000();
		UI.AddDisposable(new _EC79<_E8B2, TraderPanel>(tradersList, _traderPanelTemplate, _tradersContainer, delegate(_E8B2 trader, TraderPanel traderPanel)
		{
			if (_E0A3 == null)
			{
				traderPanel.Show(trader.Info, _E0A4, _tooltip, delegate
				{
					_E001(trader);
				});
			}
			else if (_E0A3 == trader)
			{
				_E001(trader);
			}
		}));
	}

	private async void _E000()
	{
		if (Singleton<_E60E>.Instance.NeedToCheckAvailable)
		{
			string[] availableSuites = await _E031.GetAvailableSuites();
			Singleton<_E60E>.Instance.SetAvailableSuites(availableSuites);
		}
	}

	private void _E001(_E8B2 trader)
	{
		_E0A3 = trader;
		new TraderScreensGroup._E000(trader, _E085, _E084, _E086, _E0A4, _E031).ShowScreen(EScreenState.Queued);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command == ECommand.Escape)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			Close();
			return ETranslateResult.Ignore;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		_E0A3 = null;
		if (_tooltip != null && _tooltip.gameObject.activeSelf)
		{
			_tooltip.Hide();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E002(_E8B2 trader, TraderPanel traderPanel)
	{
		if (_E0A3 == null)
		{
			traderPanel.Show(trader.Info, _E0A4, _tooltip, delegate
			{
				_E001(trader);
			});
		}
		else if (_E0A3 == trader)
		{
			_E001(trader);
		}
	}
}
