using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ChartAndGraph;

[ExecuteInEditMode]
[RequireComponent(typeof(AnyChart))]
public abstract class ChartSettingItemBase : MonoBehaviour, IInternalSettings
{
	[CompilerGenerated]
	private EventHandler m__E002;

	[CompilerGenerated]
	private EventHandler _E003;

	private AnyChart _E004;

	protected abstract Action<IInternalUse, bool> Assign { get; }

	private AnyChart _E005
	{
		get
		{
			if (_E004 == null)
			{
				_E004 = GetComponent<AnyChart>();
			}
			return _E004;
		}
	}

	private event EventHandler _E000
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = this.m__E002;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = this.m__E002;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private event EventHandler _E001
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E003;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E003;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	event EventHandler IInternalSettings.InternalOnDataUpdate
	{
		add
		{
			this._E000 += value;
		}
		remove
		{
			this._E000 -= value;
		}
	}

	event EventHandler IInternalSettings.InternalOnDataChanged
	{
		add
		{
			this._E001 += value;
		}
		remove
		{
			this._E001 -= value;
		}
	}

	protected void AddInnerItem(IInternalSettings item)
	{
		item.InternalOnDataChanged += _E001;
		item.InternalOnDataUpdate += _E000;
	}

	private void _E000(object sender, EventArgs e)
	{
		RaiseOnUpdate();
	}

	private void _E001(object sender, EventArgs e)
	{
		RaiseOnChanged();
	}

	protected virtual void RaiseOnChanged()
	{
		if (_E003 != null)
		{
			_E003(this, EventArgs.Empty);
		}
	}

	protected virtual void RaiseOnUpdate()
	{
		if (this.m__E002 != null)
		{
			this.m__E002(this, EventArgs.Empty);
		}
	}

	private void _E002(bool clear)
	{
		AnyChart anyChart = _E005;
		if (anyChart != null)
		{
			Assign(anyChart, clear);
		}
	}

	protected virtual void OnEnable()
	{
		_E002(clear: false);
	}

	protected virtual void OnDisable()
	{
		_E002(clear: true);
	}

	protected virtual void OnDestory()
	{
		_E002(clear: true);
	}

	protected virtual void OnValidate()
	{
		AnyChart anyChart = _E005;
		if (anyChart != null)
		{
			((IInternalUse)anyChart).CallOnValidate();
		}
	}

	protected virtual void Start()
	{
		_E002(clear: false);
	}
}
