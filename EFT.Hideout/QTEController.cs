using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public class QTEController : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public QuickTimeEvent quickTimeEvent;

		internal bool _E000(QteTemplate q)
		{
			return q.Type == quickTimeEvent.Type;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public CancellationToken cancellationToken;

		internal bool _E000()
		{
			return cancellationToken.IsCancellationRequested;
		}
	}

	[SerializeField]
	private QteTemplate[] _qteTemplates;

	[SerializeField]
	private bool _isLoop;

	private IEnumerable<QuickTimeEvent> _E060;

	private readonly _E3A4 _E061 = new _E3A4();

	private readonly List<GameObject> _E062 = new List<GameObject>(20);

	private _E83F _E063 = new _E83F();

	private bool _E064;

	[CompilerGenerated]
	private Action<bool, _E83F> _E065;

	public event Action<bool, _E83F> OnSingleQteFinished
	{
		[CompilerGenerated]
		add
		{
			Action<bool, _E83F> action = _E065;
			Action<bool, _E83F> action2;
			do
			{
				action2 = action;
				Action<bool, _E83F> value2 = (Action<bool, _E83F>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E065, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool, _E83F> action = _E065;
			Action<bool, _E83F> action2;
			do
			{
				action2 = action;
				Action<bool, _E83F> value2 = (Action<bool, _E83F>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E065, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private QTEAction _E000(QuickTimeEvent quickTimeEvent)
	{
		QteTemplate qteTemplate = _qteTemplates.FirstOrDefault((QteTemplate q) => q.Type == quickTimeEvent.Type);
		if (qteTemplate.Action == null)
		{
			throw new Exception(string.Format(_ED3E._E000(164548), quickTimeEvent.Type));
		}
		QTEAction qTEAction = UnityEngine.Object.Instantiate(qteTemplate.Action, base.transform, worldPositionStays: false);
		qTEAction.RectTransform().anchorMin = quickTimeEvent.Position;
		qTEAction.RectTransform().anchorMax = quickTimeEvent.Position;
		qTEAction.RectTransform().anchoredPosition = Vector2.zero;
		_E062.Add(qTEAction.gameObject);
		return qTEAction;
	}

	private async Task<bool> _E001(QuickTimeEvent quickTimeEvent)
	{
		CancellationToken cancellationToken = _E061.CancellationToken;
		await Task.Delay((int)(quickTimeEvent.StartDelay * 1000f));
		if (cancellationToken.IsCancellationRequested || this == null)
		{
			return false;
		}
		QTEAction qTEAction = _E000(quickTimeEvent);
		bool flag = await qTEAction.StartAction(quickTimeEvent);
		if (this == null)
		{
			return false;
		}
		_E063.SequenceResults.Add(flag);
		if (flag)
		{
			_E063.ActionsPassed++;
		}
		else
		{
			_E063.ActionsFailed++;
		}
		_E062.Remove(qTEAction.gameObject);
		UnityEngine.Object.Destroy(qTEAction.gameObject);
		_E065?.Invoke(flag, _E063);
		return flag;
	}

	public async Task<_E83F> StartQteBySequence(IEnumerable<QuickTimeEvent> qteDatas)
	{
		Stop();
		_E063 = new _E83F();
		_E060 = qteDatas;
		CancellationToken cancellationToken = _E061.CancellationToken;
		Task task = TasksExtensions.WaitUntil(() => cancellationToken.IsCancellationRequested);
		foreach (QuickTimeEvent item in _E060)
		{
			if (cancellationToken.IsCancellationRequested || this == null)
			{
				return _E063;
			}
			Task<bool> task2 = _E001(item);
			await Task.WhenAny(task2, task);
		}
		if (cancellationToken.IsCancellationRequested || this == null)
		{
			return _E063;
		}
		if (_isLoop)
		{
			return await StartQteBySequence(_E060);
		}
		_E063.Succeed = true;
		return _E063;
	}

	public void Stop()
	{
		foreach (GameObject item in _E062.Where((GameObject spawnedQte) => spawnedQte))
		{
			UnityEngine.Object.Destroy(item);
		}
		_E062.Clear();
		_E061.Dispose();
	}
}
