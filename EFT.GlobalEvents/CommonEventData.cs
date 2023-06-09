using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.GlobalEvents;

public class CommonEventData : _EBDD, _EBDC, IDisposable
{
	private const int POOL_SIZE = 20;

	private readonly Dictionary<Type, List<_EBB0>> _events = new Dictionary<Type, List<_EBB0>>();

	[JsonIgnore]
	private readonly Dictionary<Type, List<_EBB0>> _eventsToApply = new Dictionary<Type, List<_EBB0>>();

	[JsonIgnore]
	private readonly Dictionary<Type, _E385<_EBB0>> _pools = new Dictionary<Type, _E385<_EBB0>>();

	[JsonIgnore]
	private readonly Dictionary<Type, int> _serializeSentEventMap = new Dictionary<Type, int>();

	[JsonIgnore]
	private readonly Dictionary<int, Type> _deserializeSentEventMap = new Dictionary<int, Type>();

	[JsonIgnore]
	private readonly Dictionary<string, Type> _nameEventMap = new Dictionary<string, Type>();

	private int _nextEventID;

	public bool IsUpdated { get; set; }

	public CommonEventData()
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			foreach (Type type in types)
			{
				if (typeof(_EBB0).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
				{
					Add(type);
				}
			}
		}
	}

	private void Add<T>() where T : _EBB0
	{
		Add(typeof(T));
	}

	private void Add(Type type)
	{
		if (!_pools.ContainsKey(type))
		{
			_E385<_EBB0> value = new _E385<_EBB0>(20, () => (_EBB0)Activator.CreateInstance(type));
			_pools.Add(type, value);
		}
		if (!_nameEventMap.ContainsKey(type.Name))
		{
			_nameEventMap.Add(type.Name, type);
		}
		if (typeof(_EBB1).IsAssignableFrom(type) && type.IsClass)
		{
			int hashCode = type.Name.GetHashCode();
			if (!_serializeSentEventMap.ContainsKey(type))
			{
				_serializeSentEventMap.Add(type, hashCode);
			}
			if (!_deserializeSentEventMap.ContainsKey(hashCode))
			{
				_deserializeSentEventMap.Add(hashCode, type);
			}
		}
	}

	public T Create<T>() where T : _EBAD
	{
		T obj = (T)Create(typeof(T));
		obj.ID = _nextEventID;
		_nextEventID++;
		return obj;
	}

	private _EBB0 Create(Type type)
	{
		if (!_pools.ContainsKey(type))
		{
			Add(type);
		}
		return _pools[type].Withdraw();
	}

	public void Dispatch(Dictionary<string, object> eventData)
	{
		eventData.TryGetValue("Event", out var value);
		string text = Convert.ToString(value);
		if (_nameEventMap.ContainsKey(text))
		{
			Type type = _nameEventMap[text];
			_EBB0 obj = Create(type);
			obj.ParseParams(eventData);
			Dispatch(type, obj);
		}
		else
		{
			Debug.LogError("Not found event from Json, key: " + text);
		}
	}

	public void Dispatch<T>(T eventObject) where T : _EBB0
	{
		Dispatch(typeof(T), eventObject);
	}

	public void Dispatch(Type type, _EBB0 eventObject)
	{
		if (_eventsToApply.ContainsKey(type))
		{
			if (_eventsToApply[type] == null)
			{
				_eventsToApply[type] = new List<_EBB0> { eventObject };
			}
			else
			{
				_eventsToApply[type].Add(eventObject);
			}
		}
		else
		{
			_eventsToApply.Add(type, new List<_EBB0> { eventObject });
		}
	}

	public bool TryGet<T>(out List<T> eventList) where T : _EBB0
	{
		return TryGet(typeof(T), out eventList);
	}

	public bool TryGet(string eventName, out List<_EBB0> eventList)
	{
		return TryGet(_nameEventMap[eventName], out eventList);
	}

	private bool TryGet<T>(Type type, out List<T> eventList)
	{
		if (_events.TryGetValue(type, out var value))
		{
			if (value.Count == 0)
			{
				eventList = null;
				return false;
			}
			eventList = new List<T>(value.Count);
			foreach (T item in value)
			{
				eventList.Add(item);
			}
			return true;
		}
		eventList = null;
		return false;
	}

	public void Apply()
	{
		foreach (KeyValuePair<Type, List<_EBB0>> item in _eventsToApply)
		{
			if (_serializeSentEventMap.ContainsKey(item.Key))
			{
				List<_EBB0> value = item.Value;
				if (value != null && value.Count > 0)
				{
					IsUpdated = true;
				}
			}
			if (_events.ContainsKey(item.Key))
			{
				_events[item.Key].AddRange(item.Value);
			}
			else
			{
				_events.Add(item.Key, new List<_EBB0>(item.Value));
			}
			item.Value?.Clear();
		}
	}

	public void Clear()
	{
		foreach (KeyValuePair<Type, List<_EBB0>> @event in _events)
		{
			foreach (_EBB0 item in @event.Value)
			{
				if (_pools.TryGetValue(@event.Key, out var value))
				{
					value.Return(item);
				}
			}
			@event.Value?.Clear();
		}
	}

	public void Serialize(ref _E528 writerStream, bool isFullUpdate = false)
	{
		List<_EBB1> list = new List<_EBB1>();
		foreach (KeyValuePair<Type, List<_EBB0>> @event in _events)
		{
			if (!_serializeSentEventMap.ContainsKey(@event.Key) || @event.Value.Count <= 0)
			{
				continue;
			}
			foreach (_EBB0 item in @event.Value)
			{
				list.Add(item as _EBB1);
			}
		}
		foreach (_EBB1 item2 in list)
		{
			writerStream.Write(value: true);
			int value = _serializeSentEventMap[item2.GetType()];
			writerStream.Write(value);
			item2.Serialize(ref writerStream);
		}
		writerStream.Write(value: false);
	}

	public void Deserialize(ref _E524 readerStream)
	{
		while (readerStream.ReadBool())
		{
			int key = readerStream.ReadInt32();
			if (_deserializeSentEventMap.ContainsKey(key))
			{
				Type type = _deserializeSentEventMap[key];
				_EBB1 obj = (_EBB1)Create(type);
				obj.Deserialize(ref readerStream);
				if (obj is _EBAD obj2)
				{
					obj2.Invoke();
				}
				else
				{
					Dispatch(type, obj);
				}
			}
		}
	}

	public void Dispose()
	{
		foreach (KeyValuePair<Type, List<_EBB0>> @event in _events)
		{
			foreach (_EBB0 item in @event.Value)
			{
				_pools[item.GetType()].Dispose();
			}
			@event.Value?.Clear();
		}
		_events.Clear();
		foreach (_E385<_EBB0> value in _pools.Values)
		{
			value.Dispose();
		}
		_pools.Clear();
	}
}
