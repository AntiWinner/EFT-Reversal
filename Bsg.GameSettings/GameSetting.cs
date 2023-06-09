using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Diz.Binding;

namespace Bsg.GameSettings;

[Serializable]
public abstract class GameSetting<T> : IGameSetting, IBindable<T>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Func<T, T> preProcessor;

		internal Task<T> _E000(T value)
		{
			return Task.FromResult(preProcessor(value));
		}
	}

	protected static readonly Type SettingType = typeof(T);

	protected static readonly bool IsNullable = default(T) == null;

	private readonly Func<T, Task<T>> _asyncPreProcessor;

	public string Key { get; }

	public bool IsAvailableToEdit { get; set; }

	public T Value
	{
		get
		{
			return GetValue();
		}
		set
		{
			SetValue(value).HandleExceptions();
		}
	}

	object IGameSetting.ObjectValue
	{
		get
		{
			return Value;
		}
		set
		{
			Value = (T)value;
		}
	}

	Type IGameSetting.ObjectType => SettingType;

	public abstract T GetValue();

	public abstract Task SetValue(T value);

	bool IGameSetting.HasSameValue(IGameSetting other)
	{
		if (other is GameSetting<T> other2)
		{
			return HasSameValue(other2);
		}
		throw new InvalidOperationException(_ED3E._E000(70427) + SettingType.FullName);
	}

	void IGameSetting.TakeValueFrom(IGameSetting other)
	{
		if (other is GameSetting<T> other2)
		{
			TakeValueFrom(other2);
			return;
		}
		throw new InvalidOperationException(_ED3E._E000(70427) + SettingType.FullName);
	}

	protected GameSetting(string key, Func<T, T> preProcessor)
	{
		Key = key;
		IsAvailableToEdit = true;
		if (preProcessor != null)
		{
			_asyncPreProcessor = (T value) => Task.FromResult(preProcessor(value));
		}
	}

	protected GameSetting(string key, Func<T, Task<T>> asyncPreProcessor)
	{
		Key = key;
		IsAvailableToEdit = true;
		_asyncPreProcessor = asyncPreProcessor;
	}

	protected Task<T> PreProcessValue(T value)
	{
		if (!IsAvailableToEdit)
		{
			throw new Exception(_ED3E._E000(70439) + Key + _ED3E._E000(70488));
		}
		if (_asyncPreProcessor == null)
		{
			return Task.FromResult(value);
		}
		return _asyncPreProcessor(value);
	}

	public abstract bool HasSameValue(GameSetting<T> other);

	public abstract void TakeValueFrom(GameSetting<T> other);

	public abstract Action Bind(Action<T> handler);

	public abstract Action Subscribe(Action<T> handler);

	public abstract Action BindWithoutValue(_ED00 handler);

	public abstract void ResetToDefault();

	public abstract void ForceApply();

	public override string ToString()
	{
		return _ED3E._E000(70466) + Key + _ED3E._E000(70519) + SettingType.Name + _ED3E._E000(30697) + Value.ToString();
	}

	public static implicit operator T(GameSetting<T> setting)
	{
		return setting.Value;
	}
}
