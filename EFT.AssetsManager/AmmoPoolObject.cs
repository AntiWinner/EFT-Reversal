using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.AssetsManager;

[DisallowMultipleComponent]
public class AmmoPoolObject : AssetPoolObject
{
	public Shell Shell;

	private List<Transform> _E023;

	private List<Transform> _E024;

	[SerializeField]
	private ECaliber _caliber;

	[CompilerGenerated]
	private bool _E025;

	private float _E026 = -100f;

	public bool ShouldBeDestroyed
	{
		[CompilerGenerated]
		get
		{
			return _E025;
		}
		[CompilerGenerated]
		private set
		{
			_E025 = value;
		}
	}

	public override void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent)
	{
		base.OnCreatePoolObject(assetsPoolParent);
		Shell = base.gameObject.AddComponent<Shell>();
		Shell.SetCaliber(_caliber);
		_E023 = new List<Transform>();
		_E024 = new List<Transform>();
		_E38B._E003(base.transform, _ED3E._E000(208834), _E023, true);
		_E38B._E003(base.transform, _ED3E._E000(208895), _E024, true);
		SetUsed(isUsed: false);
		base.enabled = false;
		_E000();
	}

	private void _E000()
	{
		base.gameObject.layer = _E37B.ShellsLayer;
		for (int i = 0; i < _E023.Count; i++)
		{
			_E023[i].gameObject.layer = _E37B.ShellsLayer;
		}
		for (int j = 0; j < _E024.Count; j++)
		{
			_E024[j].gameObject.layer = _E37B.ShellsLayer;
		}
	}

	protected override void ReturnToPool()
	{
		_E001();
		_E000();
		ShouldBeDestroyed = false;
		base.ReturnToPool();
	}

	private void _E001()
	{
		Shell.CollisionListener = null;
		base.enabled = false;
		Shell.DisablePhysics();
	}

	public void EnablePhysics(Vector3 force, Vector3 torque, Vector3 parentForce, Vector3 weaponForward)
	{
		Shell.ActivatePhysics(base.transform.position, base.transform.rotation * force + parentForce, torque, weaponForward);
	}

	public void SetUsed(bool isUsed)
	{
		if (_E024 != null && _E023 != null)
		{
			if (_E024.Count == 0)
			{
				_E38B._E003(base.transform, _ED3E._E000(208895), _E024, false);
			}
			if (_E023.Count == 0)
			{
				_E38B._E003(base.transform, _ED3E._E000(208834), _E023, false);
			}
			for (int i = 0; i < _E024.Count; i++)
			{
				_E024[i].gameObject.SetActive(isUsed);
			}
			for (int j = 0; j < _E023.Count; j++)
			{
				_E023[j].gameObject.SetActive(!isUsed);
			}
		}
	}

	public void StartAutoDestroyCountDown(float countdownTime = 1f)
	{
		_E026 = countdownTime;
		base.enabled = true;
	}

	private void Update()
	{
		if (!(_E026 < -10f))
		{
			_E026 -= Time.deltaTime;
			if (_E026 < 0f)
			{
				_E026 = -100f;
				ShouldBeDestroyed = true;
			}
		}
	}
}
