using System;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public abstract class DialogWindow<TContext> : Window<TContext> where TContext : _EC7B, new()
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TContext windowContext;

		public DialogWindow<TContext> _003C_003E4__this;

		internal void _E000()
		{
			windowContext.OnCloseSilent -= _003C_003E4__this.CloseSilent;
			windowContext.OnAccept -= _003C_003E4__this.Accept;
			windowContext.OnDecline -= _003C_003E4__this.Decline;
		}

		internal void _E001()
		{
			_003C_003E4__this.WindowTransform.SetInCenter();
		}
	}

	private const int m__E000 = 24;

	private const string _E001 = "OK";

	[SerializeField]
	protected DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _cancelButton;

	private Action _E002;

	private string _E003;

	private int _E004;

	private bool _E005 = true;

	protected virtual bool CloseOnAccept => true;

	protected DefaultUIButton AcceptButton => _acceptButton;

	protected TContext Show(string title, Action acceptAction, [CanBeNull] Action cancelAction)
	{
		TContext windowContext = Show(cancelAction);
		_E002 = acceptAction;
		base.transform.SetAsLastSibling();
		windowContext.OnCloseSilent += CloseSilent;
		windowContext.OnAccept += Accept;
		windowContext.OnDecline += Decline;
		UI.AddDisposable(delegate
		{
			windowContext.OnCloseSilent -= CloseSilent;
			windowContext.OnAccept -= Accept;
			windowContext.OnDecline -= Decline;
		});
		if (!string.IsNullOrEmpty(title))
		{
			base.Caption.text = title;
		}
		if (base.CloseButton != null)
		{
			base.CloseButton.gameObject.SetActive(CloseAction != null);
		}
		if (_cancelButton != null)
		{
			_cancelButton.gameObject.SetActive(CloseAction != null);
		}
		if (CloseAction == null)
		{
			_acceptButton.SetHeaderText(_ED3E._E000(249891), 24);
		}
		else
		{
			_acceptButton.SetHeaderText(_E003, _E004);
		}
		base.WindowTransform.SetInCenter();
		this.WaitOneFrame(delegate
		{
			base.WindowTransform.SetInCenter();
		});
		return base.WindowContext;
	}

	protected override void Awake()
	{
		base.Awake();
		_E003 = _acceptButton.HeaderText;
		_E004 = _acceptButton.HeaderSize;
		_acceptButton.OnClick.AddListener(Accept);
		if (_cancelButton != null)
		{
			_cancelButton.OnClick.AddListener(Decline);
		}
	}

	protected virtual void Update()
	{
		if (_E005)
		{
			if (Input.GetKeyDown(KeyCode.Y))
			{
				Accept();
			}
			else if (Input.GetKeyDown(KeyCode.N))
			{
				Decline();
			}
		}
	}

	protected virtual void SetAcceptActive(bool value)
	{
		_E005 = value;
		_acceptButton.Interactable = _E005;
	}

	public virtual void Accept()
	{
		if (CloseOnAccept)
		{
			Close(ECloseState.Accept);
			return;
		}
		base.WindowContext.OnAccept -= Accept;
		base.WindowContext.Accept();
		_E002?.Invoke();
		base.WindowContext.OnAccept += Accept;
	}

	public virtual void Decline()
	{
		Close(ECloseState.Decline);
	}

	protected virtual void CloseSilent()
	{
		Close(ECloseState.Silent);
	}

	private void Close(ECloseState state)
	{
		if (Open)
		{
			TContext windowContext = base.WindowContext;
			Open = false;
			Close();
			Action action;
			switch (state)
			{
			case ECloseState.Accept:
				action = _E002;
				windowContext.AcceptAndClose();
				break;
			case ECloseState.Decline:
				action = CloseAction ?? _E002;
				windowContext.Decline();
				break;
			case ECloseState.Silent:
				action = null;
				windowContext.CloseSilent();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			CloseAction = null;
			_E002 = null;
			action?.Invoke();
		}
	}

	public override void Close()
	{
		if (Open)
		{
			Decline();
		}
		else
		{
			base.Close();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		switch (command)
		{
		case ECommand.Escape:
			Decline();
			return ETranslateResult.Block;
		case ECommand.EnterHideout:
			Accept();
			return ETranslateResult.Block;
		default:
			return InputNode.GetDefaultBlockResult(command);
		}
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}
}
