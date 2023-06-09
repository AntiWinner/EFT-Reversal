using System;
using System.Collections.Generic;
using Comfort.Common;
using UnityEngine;

public abstract class ComponentSystem<T, TS> : MonoBehaviour, _E3A8<T> where T : _E3A7 where TS : ComponentSystem<T, TS>
{
	protected List<T> Components;

	protected abstract bool HasUpdate { get; }

	protected abstract bool HasLateUpdate { get; }

	public static void Register(GameObject globalDotNotDestroy)
	{
		Singleton<_E3A8<T>>.Create(globalDotNotDestroy.AddComponent<TS>());
	}

	protected virtual void Awake()
	{
		Components = new List<T>(64);
	}

	protected virtual void Update()
	{
		if (!HasUpdate)
		{
			return;
		}
		for (int i = 0; i < Components.Count; i++)
		{
			try
			{
				UpdateComponent(Components[i]);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (!HasLateUpdate)
		{
			return;
		}
		for (int i = 0; i < Components.Count; i++)
		{
			try
			{
				LateUpdateComponent(Components[i]);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	protected abstract void UpdateComponent(T component);

	protected abstract void LateUpdateComponent(T component);

	public virtual void Register(T component)
	{
		Components.Add(component);
	}

	public virtual void Unregister(T component)
	{
		Components.Remove(component);
	}
}
