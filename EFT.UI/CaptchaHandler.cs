using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI;

public sealed class CaptchaHandler : UIInputNode, _E794
{
	private sealed class _E000<_E0A5>
	{
		private readonly _E0A5 m__E000;

		public readonly bool Cancelled;

		public static _E000<_E0A5> CancelledResult => new _E000<_E0A5>();

		private _E000()
		{
			_E000 = default(_E0A5);
			Cancelled = true;
		}

		public _E000(_E0A5 result)
		{
			_E000 = result;
			Cancelled = false;
		}

		public bool TryGetResult(out _E0A5 result)
		{
			result = _E000;
			return !Cancelled;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public TaskCompletionSource<IEnumerable<string>> captchaWindowTaskSource;

		public CancellationTokenSource cancellationSourceCache;

		internal void _E000(List<string> resultItems)
		{
			captchaWindowTaskSource.SetResult(resultItems);
		}

		internal void _E001()
		{
			cancellationSourceCache.Cancel(throwOnFirstException: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public TaskCompletionSource<bool> messageTaskSource;

		internal void _E000()
		{
			messageTaskSource.SetResult(result: true);
		}
	}

	private const string _E0A5 = "captcha/too frequent attempts";

	[SerializeField]
	private CaptchaWindow _captchaWindow;

	[SerializeField]
	private GameObject _lockScreen;

	private Profile _E085;

	private _E796 _E031;

	private _EB1E _E0A6;

	private bool _E0A7;

	private bool _E0A8;

	private string _E0A9;

	private CancellationTokenSource _E0AA;

	private _EC7B _E0AB;

	public void Init(_E796 session)
	{
		_E031 = session;
		_E085 = session.Profile;
		if (_E0A6 == null)
		{
			_EAA0 rootItem = (_EAA0)Singleton<_E63B>.Instance.CreateItem(_ED3E._E000(262091), _ED3E._E000(151068), null);
			_E0A6 = new _EB1E(rootItem, _ED3E._E000(262141), _ED3E._E000(262132));
		}
		_E0A7 = true;
	}

	public void Activate()
	{
		if (!_E0A8)
		{
			_E031.RegisterCaptchaHandler(this);
			_E0A8 = true;
		}
	}

	public void Deactivate()
	{
		_E031.RegisterCaptchaHandler(null);
		_E0A8 = false;
		Close();
	}

	void _E794.RequestCaptcha(object operation, Callback callback)
	{
		_E000(operation, callback).HandleExceptions();
	}

	private async Task _E000(object operation, Callback callback)
	{
		Result<ECaptchaResult> sourceResult = await RequestCaptcha();
		if (sourceResult.Succeed && sourceResult.Value == ECaptchaResult.Succeeded)
		{
			_E031.SendOperationRightNow(operation, callback);
		}
		else if (sourceResult.Succeed)
		{
			callback?.Invoke(new FailedResult(_ED3E._E000(262123), 218));
		}
		else
		{
			callback?.Invoke(sourceResult);
		}
	}

	public async Task<Result<ECaptchaResult>> RequestCaptcha()
	{
		if (!_E0A7)
		{
			Debug.LogError(_ED3E._E000(260121));
			return ECaptchaResult.Closed;
		}
		if (!_E0A8)
		{
			return ECaptchaResult.Closed;
		}
		if (_E0AA != null)
		{
			CancellationTokenSource cancellationTokenSource = _E0AA;
			_E0AA = null;
			cancellationTokenSource.Cancel(throwOnFirstException: false);
			cancellationTokenSource.Dispose();
		}
		_E0AB?.CloseSilent();
		_lockScreen.SetActive(value: true);
		_E0AA = new CancellationTokenSource();
		ShowGameObject();
		Result<ECaptchaResult> result;
		do
		{
			result = await _E001();
		}
		while (result.Succeed && result.Value == ECaptchaResult.WrongCaptcha);
		Close();
		if (result.Failed || result.Value != 0)
		{
			await _EC92.Instance.TryReturnToRootScreen();
		}
		return result;
	}

	private async Task<Result<ECaptchaResult>> _E001()
	{
		if (!(await _E005(_E031.GetCaptcha())).TryGetResult(out var result))
		{
			return ECaptchaResult.Closed;
		}
		if (result.Failed)
		{
			if (result.ErrorCode == 219)
			{
				await _E004(_ED3E._E000(260204).Localized());
			}
			return new Result<ECaptchaResult>(ECaptchaResult.Failed, result.Error, result.ErrorCode);
		}
		CaptchaData value = result.Value;
		_E0A9 = value.Type;
		if (!(await _E002(value.Code, value.Title, value.Description, value.Items)).TryGetResult(out var result2))
		{
			return ECaptchaResult.Closed;
		}
		return await _E003(result2);
	}

	private async Task<_E000<IEnumerable<string>>> _E002(string code, string title, string description, string[] items)
	{
		List<Item> items2 = items.Select((string item) => Singleton<_E63B>.Instance.GetPresetItem(item)).ToList();
		string title2 = code + _ED3E._E000(18502) + title;
		TaskCompletionSource<IEnumerable<string>> captchaWindowTaskSource = new TaskCompletionSource<IEnumerable<string>>();
		CancellationTokenSource cancellationSourceCache = _E0AA;
		_E0AB = _captchaWindow.Show(title2, description, items2, _E085, _E0A6, delegate(List<string> resultItems)
		{
			captchaWindowTaskSource.SetResult(resultItems);
		}, delegate
		{
			cancellationSourceCache.Cancel(throwOnFirstException: false);
		});
		_E000<IEnumerable<string>> obj = await _E005(captchaWindowTaskSource.Task);
		return (obj.Cancelled || captchaWindowTaskSource.Task.Result == null) ? _E000<IEnumerable<string>>.CancelledResult : obj;
	}

	private async Task<Result<ECaptchaResult>> _E003(IEnumerable<string> items)
	{
		if (items == null)
		{
			return ECaptchaResult.Closed;
		}
		if (!(await _E005(_E031.ValidateCaptcha(items, _E0A9))).TryGetResult(out var result))
		{
			return ECaptchaResult.Closed;
		}
		if (result.Failed)
		{
			return new Result<ECaptchaResult>(ECaptchaResult.Failed, result.Error, result.ErrorCode);
		}
		int errorCode = result.Value.errorCode;
		switch (errorCode)
		{
		case 0:
			await _captchaWindow.ValidAnswer();
			return ECaptchaResult.Succeeded;
		case 218:
		{
			string error = result.Value.error;
			await _E004(error);
			return new Result<ECaptchaResult>(ECaptchaResult.Failed, error, errorCode);
		}
		default:
			_captchaWindow.InvalidAnswer();
			return ECaptchaResult.WrongCaptcha;
		}
	}

	private async Task _E004(string errorMessage)
	{
		TaskCompletionSource<bool> messageTaskSource = new TaskCompletionSource<bool>();
		_EC7B obj = ItemUiContext.Instance.ShowMessageWindow(errorMessage, delegate
		{
			messageTaskSource.SetResult(result: true);
		});
		_lockScreen.SetActive(value: false);
		_E000<bool> obj2 = await _E005(messageTaskSource.Task);
		_lockScreen.SetActive(value: true);
		if (!obj2.TryGetResult(out var _))
		{
			obj.CloseSilent();
		}
	}

	private async Task<_E000<_E077>> _E005<_E077>(Task<_E077> task)
	{
		CancellationToken token = _E0AA.Token;
		return (await task.Await(token)) ? new _E000<_E077>(task.Result) : _E000<_E077>.CancelledResult;
	}

	public override void Close()
	{
		if (_E0AA != null)
		{
			CancellationTokenSource cancellationTokenSource = _E0AA;
			_E0AA = null;
			if (!cancellationTokenSource.IsCancellationRequested)
			{
				cancellationTokenSource.Cancel(throwOnFirstException: false);
			}
			cancellationTokenSource.Dispose();
			_E0A9 = string.Empty;
			_lockScreen.SetActive(value: false);
			_E0AB?.CloseSilent();
			base.Close();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape) || base.gameObject.activeSelf)
		{
			return ETranslateResult.Ignore;
		}
		Close();
		return ETranslateResult.Block;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}
}
