using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Bsg.GameSettings;

[Serializable]
public class StateGameSetting<T> : GameSetting<T>
{
	private readonly _ECF5<T> _internalState = new _ECF5<T>();

	private T _defaultValue;

	public override T GetValue()
	{
		return _internalState.Value;
	}

	public override async Task SetValue(T value)
	{
		_ECF5<T> internalState = _internalState;
		internalState.Value = await PreProcessValue(value);
	}

	internal StateGameSetting(string key, T defaultValue, Func<T, Task<T>> asyncPreProcessor)
		: base(key, asyncPreProcessor)
	{
		_E000(defaultValue);
	}

	internal StateGameSetting(string key, T defaultValue, Func<T, T> preProcessor)
		: base(key, preProcessor)
	{
		_E000(defaultValue);
	}

	private void _E000(T defaultValue)
	{
		_defaultValue = defaultValue;
		ResetToDefault();
	}

	public override bool HasSameValue(GameSetting<T> other)
	{
		return EqualityComparer<T>.Default.Equals(base.Value, other.Value);
	}

	public override void TakeValueFrom(GameSetting<T> other)
	{
		base.Value = other.Value;
	}

	public override Action Bind(Action<T> handler)
	{
		return _internalState.Bind(handler);
	}

	public override Action Subscribe(Action<T> handler)
	{
		return _internalState.Subscribe(handler);
	}

	public override Action BindWithoutValue(_ED00 handler)
	{
		return _internalState.BindWithoutValue(handler);
	}

	public sealed override void ResetToDefault()
	{
		_internalState.Value = _defaultValue;
	}

	public override void ForceApply()
	{
		if (_internalState.Handlers == null)
		{
			return;
		}
		foreach (Action<T> handler in _internalState.Handlers)
		{
			try
			{
				handler(base.Value);
			}
			catch (Exception ex)
			{
				Debug.LogError(_ED3E._E000(70510) + base.Key + _ED3E._E000(12201) + ex);
			}
		}
	}
}
