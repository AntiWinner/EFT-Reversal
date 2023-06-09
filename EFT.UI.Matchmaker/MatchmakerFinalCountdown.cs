using System;
using EFT.InputSystem;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerFinalCountdown : EftScreen<MatchmakerFinalCountdown._E000, MatchmakerFinalCountdown>
{
	public new sealed class _E000 : _EC92._E000<_E000, MatchmakerFinalCountdown>
	{
		public readonly Profile ActiveProfile;

		public readonly DateTime GameStartTime;

		public override EEftScreenType ScreenType => EEftScreenType.FinalCountdown;

		public override bool KeyScreen => true;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(Profile activeProfile, DateTime gameStartTime)
		{
			ActiveProfile = activeProfile;
			GameStartTime = gameStartTime;
		}
	}

	[SerializeField]
	private TextMeshProUGUI _time;

	[SerializeField]
	private PlayerNamePanel _namePanel;

	private new DateTime m__E000;

	public override void Show(_E000 controller)
	{
		Show(controller.ActiveProfile, controller.GameStartTime);
	}

	private void Show(Profile activeProfile, DateTime gameStartTime)
	{
		ShowGameObject();
		this.m__E000 = gameStartTime;
		_namePanel.Set(activeProfile);
	}

	private void Update()
	{
		TimeSpan timeSpan = this.m__E000 - _E5AD.Now;
		timeSpan = ((timeSpan.TotalSeconds < 0.0) ? new TimeSpan(0L) : timeSpan);
		string text = string.Format(_ED3E._E000(234180), (int)timeSpan.TotalMinutes, timeSpan.Seconds, timeSpan.Milliseconds);
		_time.SetMonospaceText(text);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}
}
