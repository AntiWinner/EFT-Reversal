using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MyParticleSystem : MonoBehaviour
{
	public string Name;

	public ParticleSystem System;

	private int _initialPoolSize = 15;

	private Transform _parent;

	private int _maxPoolSize = 60;

	private List<ParticlePoolItem> _usedSystems = new List<ParticlePoolItem>();

	private Queue<ParticlePoolItem> _freeSystems = new Queue<ParticlePoolItem>();

	private int _poolSize;

	public int InitialPoolSize
	{
		get
		{
			return _initialPoolSize;
		}
		set
		{
			_initialPoolSize = Mathf.Max(0, value);
		}
	}

	public int MaxPoolSize
	{
		get
		{
			return _maxPoolSize;
		}
		set
		{
			_maxPoolSize = Mathf.Max(InitialPoolSize, value);
		}
	}

	public void Start()
	{
		_parent = base.transform;
		for (int i = 0; i < InitialPoolSize; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(System.gameObject);
			obj.transform.parent = _parent;
			ParticlePoolItem particlePoolItem = obj.AddComponent<ParticlePoolItem>();
			particlePoolItem.Init(_E000);
			_freeSystems.Enqueue(particlePoolItem);
			_poolSize++;
		}
	}

	private void _E000(ParticlePoolItem item)
	{
		_usedSystems.Remove(item);
		_freeSystems.Enqueue(item);
	}

	public void Play(Vector3 pos, Vector3 rot)
	{
		if (_freeSystems.Count != 0)
		{
			ParticlePoolItem particlePoolItem = _freeSystems.Dequeue();
			_usedSystems.Add(particlePoolItem);
			particlePoolItem.UseItem(pos, rot);
		}
		else if (_poolSize < MaxPoolSize)
		{
			GameObject obj = UnityEngine.Object.Instantiate(System.gameObject);
			obj.transform.parent = _parent;
			ParticlePoolItem particlePoolItem = obj.AddComponent<ParticlePoolItem>();
			particlePoolItem.Init(_E000);
			_usedSystems.Add(particlePoolItem);
			particlePoolItem.UseItem(pos, rot);
			_poolSize++;
		}
		else
		{
			ParticlePoolItem particlePoolItem = _usedSystems[0];
			_usedSystems.RemoveAt(0);
			particlePoolItem.Stop();
			particlePoolItem.UseItem(pos, rot);
			_usedSystems.Add(particlePoolItem);
		}
	}
}
