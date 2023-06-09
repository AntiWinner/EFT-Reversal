using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public abstract class SerializableEnumDictionary<TEnum, T> : Dictionary<TEnum, T>, ISerializationCallbackReceiver where TEnum : struct, Enum
{
	[SerializeField]
	[HideInInspector]
	private List<string> _keys = new List<string>();

	[HideInInspector]
	[SerializeField]
	private List<T> _values = new List<T>();

	protected SerializableEnumDictionary()
		: base(_E3A5<TEnum>.Count, _E3A5<TEnum>.EqualityComparer)
	{
	}

	[CanBeNull]
	public T GetValueOrDefault(TEnum key)
	{
		if (!TryGetValue(key, out var value))
		{
			return default(T);
		}
		return value;
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		Clear();
		for (int i = 0; i < _keys.Count; i++)
		{
			if (_E3A5<TEnum>.TryDeserializeValue(_keys[i], out var value))
			{
				base[value] = _values[i];
			}
		}
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			var (value, item) = enumerator.Current;
			_keys.Add(_E3A5<TEnum>.SerializeValue(value));
			_values.Add(item);
		}
	}
}
