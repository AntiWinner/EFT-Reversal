using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
public class TextFormatting : IInternalSettings
{
	[SerializeField]
	private string prefix = "";

	[SerializeField]
	private string suffix = "";

	[CompilerGenerated]
	private EventHandler OnDataUpdate;

	[CompilerGenerated]
	private EventHandler OnDataChanged;

	public string Suffix
	{
		get
		{
			return suffix;
		}
		set
		{
			suffix = value;
			RaiseOnUpdate();
		}
	}

	public string Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			prefix = value;
			RaiseOnUpdate();
		}
	}

	private event EventHandler _E000
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = OnDataUpdate;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataUpdate, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = OnDataUpdate;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataUpdate, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private event EventHandler _E001
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = OnDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = OnDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataChanged, value2, eventHandler2);
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

	protected virtual void RaiseOnChanged()
	{
		if (OnDataChanged != null)
		{
			OnDataChanged(this, EventArgs.Empty);
		}
	}

	protected virtual void RaiseOnUpdate()
	{
		if (OnDataUpdate != null)
		{
			OnDataUpdate(this, EventArgs.Empty);
		}
	}

	private static string _E000(string str, string category, string group)
	{
		if (!str.Contains(_ED3E._E000(238655)) && !str.Contains(_ED3E._E000(238650)))
		{
			return str;
		}
		return str.Replace(_ED3E._E000(238645), category).Replace(_ED3E._E000(238633), group).Replace(_ED3E._E000(238650), Environment.NewLine);
	}

	private static void _E001(StringBuilder builder, string category, string group)
	{
		builder.Replace(_ED3E._E000(238645), category).Replace(_ED3E._E000(238633), group).Replace(_ED3E._E000(238650), Environment.NewLine);
	}

	private static string _E002(string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			return str;
		}
		return "";
	}

	public void Format(StringBuilder builder, string data, string category, string group)
	{
		builder.Length = 0;
		builder.Append(_E002(Prefix));
		builder.Append(data);
		builder.Append(_E002(Suffix));
		_E001(builder, category, group);
	}

	public string Format(string data, string category, string group)
	{
		return _E000(_E002(Prefix) + data + _E002(Suffix), category, group);
	}
}
