using System;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI.Screens;

public abstract class UIScreen : UIInputNode
{
	private sealed class _E000 : _E315
	{
		public _E000()
			: base(_ED3E._E000(257548), LoggerMode.Add)
		{
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public UIScreen _003C_003E4__this;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this._E0B0 = null;
			callback();
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public UIScreen _003C_003E4__this;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this._E0B1 = null;
			callback();
		}
	}

	private static readonly _E000 _E0AF = new _E000();

	private Coroutine _E0B0;

	private Coroutine _E0B1;

	protected CanvasGroup CanvasGroup => this.GetOrAddComponent<CanvasGroup>();

	protected virtual void OnDestroy()
	{
		UI.Dispose();
	}

	private void _E000(CanvasGroup group, Action callback)
	{
		base.gameObject.SetActive(value: true);
		group.SoftChange(status: true, ref _E0B0, delegate
		{
			_E0B0 = null;
			callback();
		});
	}

	protected void SoftHide(CanvasGroup group, Action callback)
	{
		group.SoftChange(status: false, ref _E0B1, delegate
		{
			_E0B1 = null;
			callback();
		});
	}

	public override void ShowGameObject(bool instant = false)
	{
		if (instant)
		{
			base.ShowGameObject(instant: true);
			_E0AF.LogInfo(_ED3E._E000(257524), base.gameObject.name);
		}
		else
		{
			_E000(CanvasGroup, delegate
			{
				_E0AF.LogInfo(_ED3E._E000(257524), base.gameObject.name);
			});
		}
	}

	public override void HideGameObject()
	{
		_E0AF.LogInfo(_ED3E._E000(257504), base.gameObject.name);
		base.HideGameObject();
	}

	public override void Close()
	{
		if (_E0B0 != null)
		{
			StaticManager.KillCoroutine(_E0B0);
			_E0B0 = null;
		}
		if (_E0B1 != null)
		{
			StaticManager.KillCoroutine(_E0B1);
			_E0B1 = null;
		}
		base.Close();
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E0AF.LogInfo(_ED3E._E000(257524), base.gameObject.name);
	}
}
