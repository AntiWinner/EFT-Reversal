using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class SideSelectionScreen : EftScreen<SideSelectionScreen._E000, SideSelectionScreen>
{
	public new sealed class _E000 : _EC90<_E000, SideSelectionScreen>
	{
		public readonly Profile BearProfile;

		public readonly Profile UsecProfile;

		[CompilerGenerated]
		private new readonly _E77E._E000 m__E000;

		[CompilerGenerated]
		private readonly string[] _E001;

		public override EEftScreenType ScreenType => EEftScreenType.SelectPlayerSide;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E77E._E000 ProfileData
		{
			[CompilerGenerated]
			get
			{
				return m__E000;
			}
		}

		public string[] AvailableCustomizations
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
		}

		public _E000(Profile bearProfile, Profile usecProfile, _E77E._E000 profileData, string[] availableCustomizations)
		{
			BearProfile = bearProfile;
			UsecProfile = usecProfile;
			m__E000 = profileData;
			_E001 = availableCustomizations;
		}
	}

	[SerializeField]
	private SideSelectionState _sideSelectionState;

	[SerializeField]
	private HeadSelectionState _headSelectionState;

	[SerializeField]
	private PlayerProfilePreview _bearPreview;

	[SerializeField]
	private PlayerProfilePreview _usecPreview;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private new List<_EC44> m__E000 = new List<_EC44>();

	private int m__E001 = -1;

	private bool m__E002;

	private void Awake()
	{
		_backButton.OnClick.AddListener(_E005);
		_nextButton.OnClick.AddListener(_E004);
		_nextButton.gameObject.SetActive(value: false);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.BearProfile, controller.UsecProfile, controller.ProfileData, controller.AvailableCustomizations);
	}

	private void Show(Profile bearProfile, Profile usecProfile, _E77E._E000 profileData, string[] availableCustomizations)
	{
		UI.Dispose();
		this.m__E001 = -1;
		this.m__E000 = new List<_EC44> { _sideSelectionState, _headSelectionState };
		foreach (_EC44 item in this.m__E000)
		{
			item.Close();
			UI.AddDisposable(item);
		}
		Dictionary<EPlayerSide, PlayerProfilePreview> previews = new Dictionary<EPlayerSide, PlayerProfilePreview>
		{
			{
				EPlayerSide.Bear,
				_bearPreview
			},
			{
				EPlayerSide.Usec,
				_usecPreview
			}
		};
		_headSelectionState.Init(profileData, availableCustomizations, previews);
		_sideSelectionState.Init(profileData, bearProfile, usecProfile, previews);
		_bearPreview.Show(bearProfile, 0.5f).HandleExceptions();
		_usecPreview.Show(usecProfile, 3f).HandleExceptions();
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: false);
		Display();
		_E000(0).HandleExceptions();
	}

	private async Task _E000(int stateIndex)
	{
		this.m__E002 = false;
		_nextButton.Interactable = this.m__E002;
		_backButton.Interactable = this.m__E002;
		List<Task> list = new List<Task>();
		if (this.m__E001 >= 0)
		{
			_EC44 obj = this.m__E000[this.m__E001];
			obj.OnStateReady -= _E001;
			list.Add(obj.HideState());
		}
		this.m__E001 = stateIndex;
		_EC44 obj2 = this.m__E000[this.m__E001];
		obj2.OnStateReady += _E001;
		list.Add(obj2.ShowState());
		await Task.WhenAll(list);
		this.m__E002 = true;
		_nextButton.Interactable = this.m__E002;
		_backButton.Interactable = this.m__E002;
	}

	private void _E001(bool ready)
	{
		_nextButton.gameObject.SetActive(ready);
	}

	private void _E002()
	{
		_backButton.Interactable = false;
		_nextButton.Interactable = false;
		ScreenController._E000();
	}

	private void _E003()
	{
		SoftHide(_canvasGroup, delegate
		{
			ScreenController.CloseScreen();
		});
	}

	private void _E004()
	{
		if (this.m__E001 < this.m__E000.Count - 1)
		{
			_E000(this.m__E001 + 1).HandleExceptions();
		}
		else
		{
			_E002();
		}
	}

	private void _E005()
	{
		if (this.m__E001 > 0)
		{
			_E000(this.m__E001 - 1).HandleExceptions();
		}
		else
		{
			_E003();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && this.m__E002)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			_E005();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		this.m__E001 = -1;
		base.Close();
		foreach (_EC44 item in this.m__E000)
		{
			item.Close();
			item.OnStateReady -= _E001;
		}
	}

	[CompilerGenerated]
	private void _E006()
	{
		ScreenController.CloseScreen();
	}
}
