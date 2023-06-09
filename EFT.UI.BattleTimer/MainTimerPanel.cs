using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace EFT.UI.BattleTimer;

public sealed class MainTimerPanel : TimerPanel
{
	private const int _E28D = 600;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private TextMeshProUGUI _currentState;

	[SerializeField]
	private Color _warningColor;

	private EMainTimerState _E28E;

	public bool ForcePull;

	protected override bool DisplayTime
	{
		get
		{
			if (_canvasGroup != null)
			{
				return _canvasGroup.alpha > 0f;
			}
			return false;
		}
	}

	public void Show(DateTime dateTime, StringBuilder stringBuilder, EMainTimerState state)
	{
		Show(dateTime, stringBuilder);
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E000));
		SetState(state);
	}

	public void SetState(EMainTimerState state)
	{
		_E28E = state;
		_E000();
	}

	private void _E000()
	{
		if (!(this == null))
		{
			_currentState.text = _E28E.ToString().Localized();
		}
	}

	protected override void UpdateTimer()
	{
		base.UpdateTimer();
		bool flag = TimeSpan.TotalSeconds < 600.0;
		_canvasGroup.ignoreParentGroups = flag;
		_canvasGroup.blocksRaycasts = !flag;
		if (flag && !ForcePull)
		{
			ForcePull = true;
			DisplayTimer();
			if (base.TimerText.color != _warningColor)
			{
				base.TimerText.color = _warningColor;
			}
		}
	}

	public void DisplayTimer()
	{
		_canvasGroup.alpha = 1f;
		base.UpdateTimer();
	}

	public void HideTimer()
	{
		_canvasGroup.alpha = 0f;
	}

	public IEnumerator Co_HideTimer(Action callback)
	{
		if (!ForcePull)
		{
			while (_canvasGroup.alpha > 0f)
			{
				_canvasGroup.alpha -= Time.deltaTime;
				yield return null;
			}
			HideTimer();
		}
		callback();
	}
}
