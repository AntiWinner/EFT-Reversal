using System;
using UnityEngine;

public class ParticlePoolItem : MonoBehaviour
{
	private Action<ParticlePoolItem> _E000;

	private ParticleSystem _E001;

	private Transform _E002;

	private float _E003;

	private float _E004;

	private bool _E005;

	public void Awake()
	{
		_E001 = GetComponent<ParticleSystem>();
		if (_E001 == null)
		{
			base.enabled = false;
		}
		_E002 = GetComponent<Transform>();
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			_E003 = Mathf.Max(_E003, particleSystem.main.startLifetime.constant + particleSystem.main.startDelay.constant);
		}
	}

	public void Init(Action<ParticlePoolItem> callback)
	{
		_E000 = callback;
	}

	public void UseItem(Vector3 position, Vector3 rotation)
	{
		_E004 = 0f;
		_E005 = true;
		_E002.position = position;
		_E001.Play(withChildren: true);
	}

	public void Stop()
	{
		_E005 = false;
		_E001.Stop(withChildren: true);
	}

	private void Update()
	{
		if (_E005)
		{
			_E004 += Time.deltaTime;
			if (!(_E004 < _E003))
			{
				_E005 = false;
				_E001.Stop(withChildren: true);
				_E000(this);
			}
		}
	}
}
