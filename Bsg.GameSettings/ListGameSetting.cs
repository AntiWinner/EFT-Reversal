using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bsg.GameSettings;

[Serializable]
public sealed class ListGameSetting<TType> : GameSetting<List<TType>>
{
	public delegate bool _E000(TType x, TType y);

	public delegate TType _E001(TType item);

	private readonly _ECED<List<TType>> _onListChanged = new _ECED<List<TType>>();

	private readonly _E000 _equalityCheck;

	private readonly _E001 _copyItem;

	private List<TType> _value;

	internal ListGameSetting(string key, _E001 copyItem = null, _E000 equalityCheck = null, Func<List<TType>, List<TType>> preProcessor = null)
		: base(key, preProcessor)
	{
		_value = new List<TType>();
		_copyItem = copyItem ?? ((_E001)((TType item) => item));
		_equalityCheck = equalityCheck ?? new _E000(EqualityComparer<TType>.Default.Equals);
	}

	public override List<TType> GetValue()
	{
		return _value;
	}

	public override async Task SetValue(List<TType> value)
	{
		_value = await PreProcessValue(value);
		_onListChanged.Invoke(_value);
	}

	public override bool HasSameValue(GameSetting<List<TType>> other)
	{
		List<TType> value = other.Value;
		bool flag = base.Value == null;
		bool flag2 = value == null;
		if (flag || flag2)
		{
			return flag == flag2;
		}
		if (base.Value.Count != value.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Value.Count; i++)
		{
			if (!_equalityCheck(base.Value[i], value[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override void TakeValueFrom(GameSetting<List<TType>> other)
	{
		base.Value.Clear();
		if (other.Value != null)
		{
			foreach (TType item in other.Value)
			{
				base.Value.Add(_copyItem(item));
			}
		}
		ForceApply();
	}

	public override Action Bind(Action<List<TType>> handler)
	{
		return _onListChanged.Bind(handler, base.Value);
	}

	public override Action Subscribe(Action<List<TType>> handler)
	{
		return _onListChanged.Subscribe(handler);
	}

	public override Action BindWithoutValue(_ED00 handler)
	{
		throw new Exception(_ED3E._E000(70541));
	}

	public override void ResetToDefault()
	{
		base.Value.Clear();
		ForceApply();
	}

	public override void ForceApply()
	{
		_onListChanged.Invoke(base.Value);
	}

	public TType[] ToArray()
	{
		return base.Value.ToArray();
	}
}
