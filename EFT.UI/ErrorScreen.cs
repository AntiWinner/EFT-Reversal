using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Comfort.Common;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class ErrorScreen : Window<_EC7B>
{
	public enum EButtonType
	{
		OkButton,
		CancelButton,
		QuitButton
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public _EC7B context;

		public ErrorScreen _003C_003E4__this;

		internal void _E000()
		{
			context.OnAccept -= _003C_003E4__this._E001;
			context.OnDecline -= _003C_003E4__this._E002;
			context.OnCloseSilent -= _003C_003E4__this._E002;
		}
	}

	private const string m__E000 = "ERROR";

	[SerializeField]
	private TextMeshProUGUI _errorDescription;

	[SerializeField]
	private DefaultUIButton _exitButton;

	private Action m__E001;

	private Coroutine m__E002;

	private string m__E003 = string.Empty;

	protected override bool DragEnabled => false;

	protected override void Awake()
	{
		base.Awake();
		_exitButton.OnClick.AddListener(_E002);
	}

	internal _EC7B Show(string title, string message, Action closeManuallyCallback = null, float waitingTime = 0f, Action timeOutCallback = null, EButtonType buttonType = EButtonType.OkButton)
	{
		if (!MonoBehaviourSingleton<PreloaderUI>.Instance.CanShowErrorScreen)
		{
			return new _EC7B();
		}
		ItemUiContext.Instance.CloseAllWindows();
		this.m__E001 = timeOutCallback ?? closeManuallyCallback;
		_EC7B context = Show(closeManuallyCallback);
		context.OnAccept += _E001;
		context.OnDecline += _E002;
		context.OnCloseSilent += _E002;
		UI.AddDisposable(delegate
		{
			context.OnAccept -= _E001;
			context.OnDecline -= _E002;
			context.OnCloseSilent -= _E002;
		});
		string text = buttonType switch
		{
			EButtonType.OkButton => _ED3E._E000(249891), 
			EButtonType.CancelButton => _ED3E._E000(249950), 
			EButtonType.QuitButton => _ED3E._E000(249941), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		_exitButton.SetHeaderText(text, _exitButton.HeaderSize);
		if (Singleton<GUISounds>.Instantiated)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
		}
		base.RectTransform.anchoredPosition = Vector2.zero;
		base.Caption.text = (string.IsNullOrEmpty(title) ? _ED3E._E000(249938) : title);
		this.m__E003 = message.SubstringIfNecessary(500);
		_errorDescription.text = this.m__E003;
		if (this.m__E002 != null)
		{
			StopCoroutine(this.m__E002);
		}
		if (waitingTime > 0f)
		{
			this.m__E002 = StartCoroutine(_E000(_E5AD.Now.AddSeconds(waitingTime)));
		}
		return context;
	}

	private IEnumerator _E000(DateTime endTime)
	{
		while (_E5AD.Now < endTime)
		{
			_errorDescription.text = string.Format(_ED3E._E000(249928), this.m__E003, (int)(endTime - _E5AD.Now).TotalSeconds, _ED3E._E000(124724).Localized());
			yield return null;
		}
		_E001();
	}

	private void Update()
	{
		if (base.isActiveAndEnabled)
		{
			UIEventSystem.Instance.Enable();
		}
	}

	public override void Close()
	{
		_E002();
	}

	private void _E001()
	{
		_E003(timeOut: true);
	}

	private void _E002()
	{
		_E003(timeOut: false);
	}

	private void _E003(bool timeOut)
	{
		if (Open)
		{
			if (this.m__E002 != null)
			{
				StopCoroutine(this.m__E002);
			}
			_EC7B windowContext = base.WindowContext;
			Open = false;
			base.Close();
			if (timeOut)
			{
				this.m__E001?.Invoke();
				windowContext.AcceptAndClose();
			}
			else
			{
				CloseAction?.Invoke();
				windowContext.Decline();
			}
		}
	}
}
