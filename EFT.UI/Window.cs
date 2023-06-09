using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public abstract class Window<TContext> : UIInputNode, IPointerDownHandler, IEventSystemHandler where TContext : _EC7C, new()
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TContext windowContext;

		public Window<TContext> _003C_003E4__this;

		internal void _E000()
		{
			windowContext.OnClose -= _003C_003E4__this.Close;
		}
	}

	[SerializeField]
	private RectTransform _windowTransform;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private TextMeshProUGUI _caption;

	[SerializeField]
	private Button _closeButton;

	[CompilerGenerated]
	private TContext _E0AD;

	protected bool Open;

	protected Action CloseAction;

	protected TContext WindowContext
	{
		[CompilerGenerated]
		get
		{
			return _E0AD;
		}
		[CompilerGenerated]
		private set
		{
			_E0AD = value;
		}
	}

	protected virtual bool DragEnabled => true;

	protected virtual bool ClickPutsOnTop => true;

	protected TextMeshProUGUI Caption => _caption;

	protected RectTransform WindowTransform => _windowTransform;

	protected Button CloseButton => _closeButton;

	protected virtual void Awake()
	{
		if (DragEnabled)
		{
			_captionPanel.AddComponent<UIDragComponent>().Init(_windowTransform, ClickPutsOnTop);
		}
		if (_closeButton != null)
		{
			_closeButton.onClick.AddListener(Close);
		}
	}

	protected TContext Show(Action onClosed)
	{
		UI.Dispose();
		Open = true;
		TContext windowContext = CreateContext();
		WindowContext = windowContext;
		CloseAction = onClosed;
		windowContext.OnClose += Close;
		UI.AddDisposable(delegate
		{
			windowContext.OnClose -= Close;
		});
		ShowGameObject();
		CorrectPosition();
		return WindowContext;
	}

	protected virtual TContext CreateContext()
	{
		return new TContext();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape))
		{
			return ETranslateResult.Ignore;
		}
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
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

	public void OnPointerDown(PointerEventData eventData)
	{
		if (ClickPutsOnTop)
		{
			_windowTransform.SetAsLastSibling();
		}
	}

	public override void Close()
	{
		TContext windowContext = WindowContext;
		WindowContext = null;
		base.Close();
		if (EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		if (Open)
		{
			Open = false;
			CloseAction?.Invoke();
			windowContext?.Close();
		}
	}
}
