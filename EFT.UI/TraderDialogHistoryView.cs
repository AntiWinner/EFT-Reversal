using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.Trading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[UsedImplicitly]
public sealed class TraderDialogHistoryView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TraderDialogBubble bubble;

		public Queue<TraderDialogBubble> pool;

		internal void _E000()
		{
			bubble.Close();
			pool.Enqueue(bubble);
		}
	}

	[SerializeField]
	private TraderDialogBubble _traderBubbleTemplate;

	[SerializeField]
	private TraderDialogBubble _playerBubbleTemplate;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private Scrollbar _scrollBar;

	private _E8BC _E220;

	private readonly Queue<TraderDialogBubble> _E221 = new Queue<TraderDialogBubble>();

	private readonly Queue<TraderDialogBubble> _E222 = new Queue<TraderDialogBubble>();

	private readonly _E3A4 _E223 = new _E3A4();

	public void Show(_E8BC history)
	{
		UI.Dispose();
		_E220 = history;
		_E220.OnLineAdded += _E001;
		UI.AddDisposable(_E223);
		UI.AddDisposable(delegate
		{
			_E220.OnLineAdded -= _E001;
		});
		_E000();
		ShowGameObject();
	}

	private void _E000()
	{
		_E223.Dispose();
		_scrollBar.value = 0f;
		foreach (_E8BC._E000 item in _E220)
		{
			_E001(item);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.RectTransform);
	}

	private void _E001(_E8BC._E000 line)
	{
		TraderDialogBubble bubble = null;
		Queue<TraderDialogBubble> pool = ((line.DialogSide == EDialogSide.Trader) ? _E221 : _E222);
		try
		{
			if (pool.Count > 0)
			{
				bubble = pool.Dequeue();
				bubble.transform.SetAsLastSibling();
			}
			else
			{
				TraderDialogBubble original = ((line.DialogSide == EDialogSide.Trader) ? _traderBubbleTemplate : _playerBubbleTemplate);
				bubble = UnityEngine.Object.Instantiate(original, _container, worldPositionStays: false);
			}
		}
		finally
		{
			if (bubble == null)
			{
				throw new Exception(_ED3E._E000(254165));
			}
			bubble.Show(line);
			_E223.AddDisposable(delegate
			{
				bubble.Close();
				pool.Enqueue(bubble);
			});
		}
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E220.OnLineAdded -= _E001;
	}
}
