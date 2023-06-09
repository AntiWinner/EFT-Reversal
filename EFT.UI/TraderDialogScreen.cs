using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.Trading;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class TraderDialogScreen : EftScreen<TraderDialogScreen._E000, TraderDialogScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, TraderDialogScreen>
	{
		public readonly Profile Profile;

		public readonly string TraderId;

		public readonly _E935 QuestController;

		public readonly _E8B7 AnimationController;

		public override EEftScreenType ScreenType => EEftScreenType.TraderDialog;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Enabled;

		public _E000(Profile profile, string traderId, _E935 questController, _E8B7 animationController)
		{
			Profile = profile;
			TraderId = traderId;
			QuestController = questController;
			AnimationController = animationController;
		}

		public void HideDialog()
		{
			if (_E002 != null)
			{
				_E002._E003();
			}
		}
	}

	[SerializeField]
	private SubtitlesView _subtitlesView;

	[SerializeField]
	private TraderDialogWindow _dialogWindow;

	private new Profile m__E000;

	private string m__E001;

	private _E935 m__E002;

	private _E8BB m__E003;

	private _E8B7 m__E004;

	public override void Show(_E000 controller)
	{
		this.m__E000 = controller.Profile;
		this.m__E001 = controller.TraderId;
		this.m__E002 = controller.QuestController;
		this.m__E004 = controller.AnimationController;
		UI.AddDisposable(_dialogWindow);
		UI.AddDisposable(_subtitlesView);
		_E000();
		_E001();
		if (MonoBehaviourSingleton<GameUI>.Instantiated)
		{
			ExtractionTimersPanel timerPanel = MonoBehaviourSingleton<GameUI>.Instance.TimerPanel;
			timerPanel.Reveal();
			UI.AddDisposable(timerPanel.Hide);
		}
		ShowGameObject();
	}

	private void _E000()
	{
		_subtitlesView.Show(ESubtitlesSource.LighthouseKeeper);
	}

	private void _E001()
	{
		this.m__E003 = new _E8BB(this.m__E000, this.m__E001, this.m__E002, this.m__E004);
		this.m__E003.OnActionFinished += _E002;
		UI.AddDisposable(delegate
		{
			this.m__E003.OnActionFinished -= _E002;
		});
		UI.AddDisposable(this.m__E003);
		_dialogWindow.Show(this.m__E003);
	}

	private void _E002(_E8CC dialog)
	{
		if (dialog.Type == ETraderDialogType.Quit)
		{
			ScreenController.CloseScreen();
		}
	}

	private void _E003()
	{
		this.m__E003.Dispose();
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.ResetLookDirection) || command.IsCommand(ECommand.DisplayTimer) || command.IsCommand(ECommand.DisplayTimerAndExits))
		{
			return ETranslateResult.Ignore;
		}
		this.m__E003?.TranslateCommand(command);
		return ETranslateResult.BlockAll;
	}

	[CompilerGenerated]
	private void _E004()
	{
		this.m__E003.OnActionFinished -= _E002;
	}
}
